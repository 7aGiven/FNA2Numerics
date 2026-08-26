using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

        static void ProcessExport(ModuleDefinition moduleDefinition)
        {
            AssemblyNameReference Numerics = null;
            foreach (AssemblyNameReference assemblyNameReference in moduleDefinition.AssemblyReferences)
            {
                if (assemblyNameReference.Name == "FNA")
                {
                    Numerics = assemblyNameReference;
                }
            }
            if (Numerics == null)
            {
                Console.Error.WriteLine("the file not depend FNA.dll");
                return;
            }
            for (int i = 0; i < numericsTypes.Length; i++)
            {
                typeReferences[i] = moduleDefinition.ImportReference(new TypeReference("System.Numerics", numericsTypes[i], moduleDefinition, Numerics) { IsValueType = true });
            }
            for (int i = 0; i < numericsTypes.Length; i++)
            {
                extensionTypeReferences[i] = moduleDefinition.ImportReference(new TypeReference("FNA.Numerics", numericsTypes[i] + "Extension", moduleDefinition, Numerics) { IsValueType = true });
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
            AssemblyNameReference Numerics = AssemblyNameReference.Parse("System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            moduleDefinition.AssemblyReferences.Add(Numerics);
            for (int i = 0; i < numericsTypes.Length; i++)
            {
                moduleDefinition.ExportedTypes.Add(new ExportedType("System.Numerics", numericsTypes[i], null, Numerics) { IsForwarder = true });
                typeReferences[i] = moduleDefinition.ImportReference(new TypeReference("System.Numerics", numericsTypes[i], moduleDefinition, Numerics) { IsValueType = true });
            }
            foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
            {
                foreach (TypeDefinition nestedType in typeDefinition.NestedTypes)
                {
                    ProcessClass(nestedType);
                }
                ProcessClass(typeDefinition);
            }
            ModuleDefinition extensionModuleDefinition = ModuleDefinition.ReadModule(Assembly.GetEntryAssembly().Location);
            for (int i = 0; i < xnaTypes.Length; i++)
            {
                TypeDefinition extensionTypeDefinition = extensionModuleDefinition.GetType("FNA.Numerics", numericsTypes[i] + "Extension");
                TypeDefinition typeDefinition = moduleDefinition.GetType("Microsoft.Xna.Framework", xnaTypes[i]);
                typeDefinition.Namespace = extensionTypeDefinition.Namespace;
                typeDefinition.Name = extensionTypeDefinition.Name;
                typeDefinition.Interfaces.Clear();
                foreach(MethodDefinition methodDefinition in typeDefinition.Methods)
                {
                    if (!methodDefinition.IsConstructor && !methodDefinition.IsStatic)
                    {
                        methodDefinition.IsStatic = true;
                        methodDefinition.HasThis = false;
                        methodDefinition.IsNewSlot = false;
                        methodDefinition.IsVirtual = false;
                        methodDefinition.IsFinal = false;
                        methodDefinition.Parameters.Insert(0, new ParameterDefinition(new ByReferenceType(typeReferences[i])));
                    }
                }
                foreach (MethodDefinition extenstionMethodDefinition in extensionTypeDefinition.Methods)
                {
                    foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                    {
                        if (StringFromMethod(methodDefinition) == StringFromMethod(extenstionMethodDefinition))
                        {
                            CloneMethod(methodDefinition, extenstionMethodDefinition);
                            break;
                        }
                    }
                }
            }
            TypeDefinition ContentTypeReaderManager = moduleDefinition.GetType("Microsoft.Xna.Framework.Content", "ContentTypeReaderManager");
            foreach (MethodDefinition methodDefinition in ContentTypeReaderManager.Methods)
            {
                if (methodDefinition.Name == "PrepareType")
                {
                    MethodReference replace = moduleDefinition.ImportReference(typeof(string).GetMethod("Replace", new Type[] { typeof(string), typeof(string) }));
                    Instruction i0 = methodDefinition.Body.Instructions[0];
                    ILProcessor processor = methodDefinition.Body.GetILProcessor();

                    processor.InsertBefore(i0, processor.Create(OpCodes.Ldarg_0));
                    for (int i = 0; i < xnaTypes.Length; i++)
                    {
                        processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework." + xnaTypes[i] +", Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                        processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics." + numericsTypes[i] +", FNA, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
                        processor.InsertBefore(i0, processor.Create(OpCodes.Call, replace));
                    }
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
                            if (fd != null && !fd.IsStatic)
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
                        else if (instruction.OpCode.OperandType == OperandType.InlineMethod)
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
                                        if (md.IsConstructor || methodForward.Contains(StringFromMethod(md)))
                                        {
                                            MethodReference mdRef = new MethodReference(md.Name, md.ReturnType, typeReferences[index]);
                                            mdRef.HasThis = md.HasThis;
                                            foreach (ParameterDefinition parameterDefinition in md.Parameters)
                                            {
                                                mdRef.Parameters.Add(parameterDefinition);
                                            }
                                            instruction.Operand = moduleDefinition.ImportReference(mdRef);
                                        }
                                    }
                                }
                            }
                            ReplaceType(((MethodReference)instruction.Operand).DeclaringType);
                        }
                    }
                }
            }
        }

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

        static void CloneMethod(MethodDefinition target, MethodDefinition source)
        {
            ModuleDefinition moduleDefinition = target.Module;
            target.ImplAttributes = source.ImplAttributes;
            target.Body = source.Body;
            foreach (VariableDefinition variable in target.Body.Variables)
            {
                if (variable.VariableType is TypeDefinition)
                    variable.VariableType = GetType(moduleDefinition, variable.VariableType);
                else if (variable.VariableType is TypeReference)
                    variable.VariableType = moduleDefinition.ImportReference(variable.VariableType);
            }
            foreach (Instruction instruction in target.Body.Instructions)
            {
                if (instruction.Operand != null)
                {
                    if (instruction.Operand is MethodDefinition)
                    {
                        MethodDefinition operand = (MethodDefinition)instruction.Operand;
                        instruction.Operand = GetType(moduleDefinition, operand.DeclaringType).Methods.First(md => md.FullName == operand.FullName);
                    }
                    else if (instruction.Operand is MethodReference operand)
                    {
                        instruction.Operand = moduleDefinition.ImportReference(operand);
                    }
                    if (instruction.Operand is FieldDefinition)
                    {
                        FieldDefinition operand = (FieldDefinition)instruction.Operand;
                        instruction.Operand = GetType(moduleDefinition, operand.DeclaringType).Fields.First(fd => fd.FullName == operand.FullName);
                    }
                    else if (instruction.Operand is FieldReference operand)
                    {
                        instruction.Operand = moduleDefinition.ImportReference(operand);
                    }
                    if (instruction.Operand is GenericInstanceType)
                    {
                        instruction.Operand = moduleDefinition.ImportReference((GenericInstanceType)instruction.Operand);
                    }
                    else if (instruction.Operand is TypeReference)
                    {
                        instruction.Operand = moduleDefinition.ImportReference((TypeReference)instruction.Operand);
                    }
                }
            }
        }

        static TypeDefinition GetType(ModuleDefinition module, TypeReference type)
        {
            return module.GetType(type.Namespace, type.Name);
        }
    }
}