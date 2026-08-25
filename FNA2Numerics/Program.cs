using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FNA2Numerics
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Usage: Do both command\nFNA2Numerics.exe FNA.dll\nFNA2Numerics.exe game.exe");
                return;
            }
            string path = args[0];

            if (path.EndsWith("FNA.dll"))
            {
                ModuleDefinition moduleDefinition = ModuleDefinition.ReadModule(path, new ReaderParameters() { ReadWrite = true });
                ProcessInternal(moduleDefinition);
                moduleDefinition.Write();
            }
            else
            {
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

        static readonly string[] types = new string[] { "Vector2", "Vector3", "Vector4" };
        static readonly string[] methods = new string[] { "Barycentric", "CatmullRom", "Hermite", "SmoothStep", "get_Up", "get_Down", "get_Right", "get_Left", "get_Forward", "get_Backward" };
        static readonly TypeReference[] extensionTypeReferences = new TypeReference[types.Length];
        static readonly TypeReference[] typeReferences = new TypeReference[types.Length];

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
                    int index = Array.IndexOf(types, checkTypeReference.Name);
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
            for (int i = 0; i < types.Length; i++)
            {
                extensionTypeReferences[i] = moduleDefinition.ImportReference(new TypeReference("FNA.Numerics", types[i] + "Extension", moduleDefinition, Numerics));
            }
            foreach (MemberReference memberReference in moduleDefinition.GetMemberReferences())
            {
                MethodReference methodReference = memberReference as MethodReference;
                if (methodReference != null && methodReference.Name != ".ctor")
                {
                    TypeReference typeReference = methodReference.DeclaringType;
                    if (typeReference.Namespace == "Microsoft.Xna.Framework")
                    {
                        int index = Array.IndexOf(types, typeReference.Name);
                        if (index != -1 && (methodReference.ReturnType == moduleDefinition.TypeSystem.Void || Array.IndexOf(methods, methodReference.Name) != -1))
                        {
                            methodReference.DeclaringType = extensionTypeReferences[index];
                        }
                    }
                }
            }
            foreach (string type in types)
            {
                TypeReference typeReference2;
                moduleDefinition.TryGetTypeReference("Microsoft.Xna.Framework." + type, out typeReference2);
                if (typeReference2 != null)
                {
                    typeReference2.Scope = Numerics;
                    typeReference2.Namespace = "System.Numerics";
                }
            }
        }

        static void ProcessInternal(ModuleDefinition moduleDefinition)
        {
            AssemblyNameReference Numerics = new AssemblyNameReference("FNA.Numerics", null);
            moduleDefinition.AssemblyReferences.Add(Numerics);
            for (int i = 0; i < types.Length; i++)
            {
                typeReferences[i] = moduleDefinition.ImportReference(new TypeReference("FNA.Numerics", types[i], moduleDefinition, Numerics));
            }
            for (int i = 0; i < types.Length; i++)
            {
                extensionTypeReferences[i] = moduleDefinition.ImportReference(new TypeReference("FNA.Numerics", types[i] + "Extension", moduleDefinition, Numerics));
            }
            foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
            {
                foreach (TypeDefinition nestedType in typeDefinition.NestedTypes)
                {
                    ProcessClass(nestedType);
                }
                ProcessClass(typeDefinition);

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
                                    int index = Array.IndexOf(types, md.DeclaringType.Name);
                                    if (index != -1)
                                    {
                                        TypeReference declaringType;
                                        if (md.ReturnType == moduleDefinition.TypeSystem.Void || Array.IndexOf(methods, md.Name) != -1)
                                        {
                                            declaringType = extensionTypeReferences[index];
                                        }
                                        else
                                        {
                                            declaringType = typeReferences[index];
                                        }
                                        instruction.Operand = moduleDefinition.ImportReference(new MethodReference(md.Name, md.ReturnType, declaringType));
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
                                instruction.Operand = moduleDefinition.ImportReference(new MethodReference(md.Name, md.ReturnType, replace));
                            }
                        }
                    }
                }
            }
        }
    }
}
