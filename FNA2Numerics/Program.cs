using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FNA2Numerics
{
    static class Program
    {
        static HashSet<string> methodForward = new HashSet<string>();
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Usage: Do both command\nFNA2Numerics.exe FNA.dll\nFNA2Numerics.exe game.exe");
                return;
            }
            string path = args[0];

            Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream("FNA2Numerics.forward.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    methodForward.Add(line);
                }
            }
            if (path.EndsWith("FNA.dll"))
            {
                ModuleDefinition moduleDefinition = ModuleDefinition.ReadModule(path, new ReaderParameters() { ReadWrite = true });
                ProcessInternal(moduleDefinition);
                moduleDefinition.Write();
            }
            else
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(path));
                ModuleDefinition moduleDefinition = ModuleDefinition.ReadModule(path, new ReaderParameters() { ReadWrite = true });
                Collection<Resource> resources = moduleDefinition.Resources;
                for (int i = 0; i < resources.Count; i++)
                {
                    EmbeddedResource embeddedResource = resources[i] as EmbeddedResource;
                    if (embeddedResource.Name.EndsWith(".dll"))
                    {
                        Console.WriteLine(embeddedResource.Name);
                        ModuleDefinition embeddedModuleDefinition = ModuleDefinition.ReadModule(embeddedResource.GetResourceStream());
                        ProcessExport(embeddedModuleDefinition);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            embeddedModuleDefinition.Write(memoryStream);
                            embeddedModuleDefinition.Dispose();
                            resources[i] = new EmbeddedResource(embeddedResource.Name, embeddedResource.Attributes, memoryStream.ToArray());
                        }
                    }
                }
                ProcessExport(moduleDefinition);
                moduleDefinition.Write();
            }
        }

        static readonly string[] xnaTypes = new string[] { "Vector2", "Vector3", "Vector4", "Matrix", "Plane", "Quaternion" };
        static readonly string[] numericsTypes = new string[] { "Vector2", "Vector3", "Vector4", "Matrix4x4", "Plane", "Quaternion" };
        static readonly TypeReference[] extensionTypeReferences = new TypeReference[xnaTypes.Length];
        static readonly TypeReference[] typeReferences = new TypeReference[numericsTypes.Length];

        static TypeReference ReplaceType(TypeReference typeReference)
        {
            ArrayType arrayType = typeReference as ArrayType;
            if (arrayType != null)
            {
                TypeReference replace = ReplaceType(arrayType.ElementType);
                if (replace != null)
                {
                    return new ArrayType(replace, arrayType.Rank);
                }
                return null;
            }
            GenericInstanceType genericInstanceType = typeReference as GenericInstanceType;
            if (genericInstanceType != null)
            {
                for (int i = 0; i < genericInstanceType.GenericArguments.Count; i++)
                {
                    TypeReference replace = ReplaceType(genericInstanceType.GenericArguments[i]);
                    if (replace != null)
                    {
                        genericInstanceType.GenericArguments[i] = replace;
                    }
                }
                return null;
            }
            TypeReference checkTypeReference = typeReference;
            if (typeReference.IsByReference)
            {
                checkTypeReference = ((TypeSpecification)typeReference).ElementType;
            }
            TypeDefinition typeDefinition = checkTypeReference as TypeDefinition;
            if (typeDefinition != null)
            {
                if (checkTypeReference.Namespace == "Microsoft.Xna.Framework")
                {
                    int index = Array.IndexOf(xnaTypes, checkTypeReference.Name);
                    if (index != -1)
                    {
                        if (typeReference.IsByReference)
                        {
                            return new ByReferenceType(typeReferences[index]);
                        }
                        else
                        {
                            return typeReferences[index];
                        }
                    }
                }
            }
            return null;
        }

        static void ProcessExport(ModuleDefinition moduleDefinition)
        {
            AssemblyNameReference Numerics = new AssemblyNameReference("FNA.Numerics", null);
            moduleDefinition.AssemblyReferences.Add(Numerics);
            for (int i = 0; i < numericsTypes.Length; i++)
            {
                typeReferences[i] = moduleDefinition.ImportReference(new TypeReference("System.Numerics", numericsTypes[i], moduleDefinition, Numerics) { IsValueType = true });
            }
            for (int i = 0; i < xnaTypes.Length; i++)
            {
                extensionTypeReferences[i] = moduleDefinition.ImportReference(new TypeReference("FNA.Numerics", xnaTypes[i] + "Extension", moduleDefinition, Numerics));
            }
            foreach (MemberReference memberReference in moduleDefinition.GetMemberReferences())
            {
                MethodReference methodReference = memberReference as MethodReference;
                if (methodReference != null && methodReference.Name != ".ctor")
                {
                    TypeReference typeReference = methodReference.DeclaringType;
                    if (typeReference.Namespace == "Microsoft.Xna.Framework")
                    {
                        int index = Array.IndexOf(xnaTypes, typeReference.Name);
                        if (index != -1 && !methodForward.Contains(StringFromMethod(methodReference)))
                        {
                            if (methodReference.HasThis)
                            {
                                methodReference.HasThis = false;
                                methodReference.Parameters.Add(new ParameterDefinition(new ByReferenceType(typeReferences[index])));
                            }
                            methodReference.DeclaringType = extensionTypeReferences[index];
                        }
                    }
                }
            }
            for (int index = 0; index < xnaTypes.Length; index++)
            {
                TypeReference typeReference;
                moduleDefinition.TryGetTypeReference("Microsoft.Xna.Framework." + xnaTypes[index], out typeReference);
                if (typeReference != null)
                {
                    typeReference.Scope = Numerics;
                    typeReference.Namespace = "System.Numerics";
                    typeReference.Name = numericsTypes[index];
                }
            }
        }

        static void ProcessInternal(ModuleDefinition moduleDefinition)
        {
            AssemblyNameReference Numerics = new AssemblyNameReference("FNA.Numerics", null);
            moduleDefinition.AssemblyReferences.Add(Numerics);
            for (int i = 0; i < numericsTypes.Length; i++)
            {
                typeReferences[i] = moduleDefinition.ImportReference(new TypeReference("System.Numerics", numericsTypes[i], moduleDefinition, Numerics) { IsValueType = true });
            }
            for (int i = 0; i < xnaTypes.Length; i++)
            {
                extensionTypeReferences[i] = moduleDefinition.ImportReference(new TypeReference("FNA.Numerics", xnaTypes[i] + "Extension", moduleDefinition, Numerics));
            }
            foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
            {
                foreach (TypeDefinition nestedType in typeDefinition.NestedTypes)
                {
                    ProcessClass(nestedType);
                }
                ProcessClass(typeDefinition);
            }
            for (int i = moduleDefinition.Types.Count - 1; i >= 0; i--)
            {
                if (Array.IndexOf(xnaTypes, moduleDefinition.Types[i].Name) != -1)
                {
                    moduleDefinition.Types.RemoveAt(i);
                }
            }
            TypeDefinition ContentTypeReaderManager = moduleDefinition.GetType("Microsoft.Xna.Framework.Content", "ContentTypeReaderManager");
            foreach(MethodDefinition methodDefinition in ContentTypeReaderManager.Methods)
            {
                if (methodDefinition.Name == "PrepareType")
                {
                    MethodReference replace = moduleDefinition.ImportReference(typeof(string).GetMethod("Replace", new Type[] { typeof(string), typeof(string) }));
                    Instruction i0 = methodDefinition.Body.Instructions[0];
                    ILProcessor processor = methodDefinition.Body.GetILProcessor();

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldarg_0));

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics.Vector2, FNA.Numerics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Call, replace));

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework.Vector3, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics.Vector3, FNA.Numerics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Call, replace));

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework.Vector4, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics.Vector4, FNA.Numerics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Call, replace));

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework.Plane, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics.Plane, FNA.Numerics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Call, replace));

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework.Quaternion, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics.Quaternion, FNA.Numerics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Call, replace));

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework.Matrix, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics.Matrix4x4, FNA.Numerics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
                    processor.InsertBefore(i0, processor.Create(OpCodes.Call, replace));

                    processor.InsertBefore(i0, processor.Create(OpCodes.Starg_S, (byte)0));
                }
            }
        }

        static void ProcessClass(TypeDefinition typeDefinition)
        {
            ModuleDefinition moduleDefinition = typeDefinition.Module;
            TypeReference replace;

            if (typeDefinition.BaseType != null)
            {
                ReplaceType(typeDefinition.BaseType);
            }
            foreach (FieldDefinition fieldDefinition in typeDefinition.Fields)
            {
                replace = ReplaceType(fieldDefinition.FieldType);
                if (replace != null)
                {
                    fieldDefinition.FieldType = replace;
                }
            }
            foreach (PropertyDefinition propertyDefinition in typeDefinition.Properties)
            {
                replace = ReplaceType(propertyDefinition.PropertyType);
                if (replace != null)
                {
                    propertyDefinition.PropertyType = replace;
                }
            }
            foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
            {
                replace = ReplaceType(methodDefinition.ReturnType);
                if (replace != null)
                {
                    methodDefinition.ReturnType = replace;
                }
                foreach (ParameterDefinition parameterDefinition in methodDefinition.Parameters)
                {
                    replace = ReplaceType(parameterDefinition.ParameterType);
                    if (replace != null)
                    {
                        parameterDefinition.ParameterType = replace;
                    }
                }
                if (methodDefinition.Body != null)
                {
                    foreach (VariableDefinition variableDefinition in methodDefinition.Body.Variables)
                    {
                        replace = ReplaceType(variableDefinition.VariableType);
                        if (replace != null)
                        {
                            variableDefinition.VariableType = replace;
                        }
                    }
                    foreach (Instruction instruction in methodDefinition.Body.Instructions)
                    {
                        if (instruction.OpCode.OperandType == OperandType.InlineTok)
                        {
                            TypeReference tr = instruction.Operand as TypeReference;
                            if (tr != null)
                            {
                                replace = ReplaceType(tr);
                                if (replace != null)
                                {
                                    instruction.Operand = replace;
                                }
                            }
                        }
                        else if (instruction.OpCode.OperandType == OperandType.InlineType)
                        {
                            replace = ReplaceType((TypeReference)instruction.Operand);
                            if (replace != null)
                            {
                                instruction.Operand = replace;
                            }
                        }
                        else if (instruction.OpCode.OperandType == OperandType.InlineField)
                        {
                            FieldDefinition fd = instruction.Operand as FieldDefinition;
                            if (fd != null)
                            {
                                replace = ReplaceType(fd.FieldType);
                                if (replace != null)
                                {
                                    fd.FieldType = replace;
                                }
                                replace = ReplaceType(fd.DeclaringType);
                                if (replace != null)
                                {
                                    instruction.Operand = moduleDefinition.ImportReference(new FieldReference(fd.Name, fd.FieldType, replace));
                                }
                            }
                        }
                        else if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt)
                        {
                            GenericInstanceMethod genericInstanceMethod = instruction.Operand as GenericInstanceMethod;
                            if (genericInstanceMethod != null)
                            {
                                for (int i = 0; i < genericInstanceMethod.GenericArguments.Count; i++)
                                {
                                    replace = ReplaceType(genericInstanceMethod.GenericArguments[i]);
                                    if (replace != null)
                                    {
                                        genericInstanceMethod.GenericArguments[i] = replace;
                                    }
                                }
                                continue;
                            }
                            MethodDefinition md = instruction.Operand as MethodDefinition;
                            if (md != null)
                            {
                                replace = ReplaceType(md.ReturnType);
                                if (replace != null)
                                {
                                    md.ReturnType = replace;
                                }
                                if (md.DeclaringType.Namespace == "Microsoft.Xna.Framework")
                                {
                                    int index = Array.IndexOf(xnaTypes, md.DeclaringType.Name);
                                    if (index != -1)
                                    {
                                        TypeReference declaringType;
                                        if (md.IsConstructor || methodForward.Contains(StringFromMethod(md)))
                                        {
                                            declaringType = typeReferences[index];
                                        }
                                        else
                                        {
                                            declaringType = extensionTypeReferences[index];
                                        }
                                        MethodReference mdRef = new MethodReference(md.Name, md.ReturnType, declaringType);
                                        mdRef.HasThis = md.HasThis;
                                        foreach (ParameterDefinition pd in md.Parameters)
                                        {
                                            mdRef.Parameters.Add(pd);
                                        }
                                        instruction.Operand = moduleDefinition.ImportReference(mdRef);
                                    }
                                }
                            }
                        }
                        if (instruction.OpCode.OperandType == OperandType.InlineMethod)
                        {
                            MethodReference md = (MethodReference)instruction.Operand;
                            replace = ReplaceType(md.DeclaringType);
                            if (replace != null)
                            {
                                MethodReference mdRef = new MethodReference(md.Name, md.ReturnType, replace);
                                mdRef.HasThis = md.HasThis;
                                foreach (ParameterDefinition pd in md.Parameters)
                                {
                                    mdRef.Parameters.Add(pd);
                                }
                                instruction.Operand = moduleDefinition.ImportReference(mdRef);
                            }
                        }
                    }
                }
            }
        }

        static string StringFromMethod(MethodReference methodReference)
        {
            bool flag = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(methodReference.DeclaringType.Name);
            sb.Append('.');
            sb.Append(methodReference.Name);
            sb.Append('(');
            foreach (ParameterDefinition parameterDefinition in methodReference.Parameters)
            {
                flag = true;
                sb.Append(parameterDefinition.ParameterType.Name);
                sb.Append(',');
            }
            if (flag)
            {
                sb.Length -= 1;
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}