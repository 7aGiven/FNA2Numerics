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
    public static class FNA2Numerics
    {
        static readonly string methodForwards = @"
Vector2.get_Zero()
Vector2.get_One()
Vector2.get_UnitX()
Vector2.get_UnitY()
Vector2.Equals(Object)
Vector2.Length()
Vector2.LengthSquared()
Vector2.Distance(Vector2,Vector2)
Vector2.DistanceSquared(Vector2,Vector2)
Vector2.Normalize(Vector2)
Vector2.Reflect(Vector2,Vector2)
Vector2.Clamp(Vector2,Vector2,Vector2)
Vector2.Lerp(Vector2,Vector2,Single)
Vector2.Transform(Vector2,Matrix3x2)
Vector2.Transform(Vector2,Matrix)
Vector2.TransformNormal(Vector2,Matrix3x2)
Vector2.TransformNormal(Vector2,Matrix)
Vector2.Transform(Vector2,Quaternion)
Vector2.Add(Vector2,Vector2)
Vector2.Subtract(Vector2,Vector2)
Vector2.Multiply(Vector2,Vector2)
Vector2.Multiply(Vector2,Single)
Vector2.Multiply(Single,Vector2)
Vector2.Divide(Vector2,Vector2)
Vector2.Divide(Vector2,Single)
Vector2.Negate(Vector2)
Vector2.Equals(Vector2)
Vector2.Dot(Vector2,Vector2)
Vector2.Min(Vector2,Vector2)
Vector2.Max(Vector2,Vector2)
Vector2.Abs(Vector2)
Vector2.SquareRoot(Vector2)
Vector2.op_Addition(Vector2,Vector2)
Vector2.op_Subtraction(Vector2,Vector2)
Vector2.op_Multiply(Vector2,Vector2)
Vector2.op_Multiply(Single,Vector2)
Vector2.op_Multiply(Vector2,Single)
Vector2.op_Division(Vector2,Vector2)
Vector2.op_Division(Vector2,Single)
Vector2.op_UnaryNegation(Vector2)
Vector2.op_Equality(Vector2,Vector2)
Vector2.op_Inequality(Vector2,Vector2)
Vector3.get_Zero()
Vector3.get_One()
Vector3.get_UnitX()
Vector3.get_UnitY()
Vector3.get_UnitZ()
Vector3.Equals(Object)
Vector3.Length()
Vector3.LengthSquared()
Vector3.Distance(Vector3,Vector3)
Vector3.DistanceSquared(Vector3,Vector3)
Vector3.Normalize(Vector3)
Vector3.Cross(Vector3,Vector3)
Vector3.Reflect(Vector3,Vector3)
Vector3.Clamp(Vector3,Vector3,Vector3)
Vector3.Lerp(Vector3,Vector3,Single)
Vector3.Transform(Vector3,Matrix)
Vector3.TransformNormal(Vector3,Matrix)
Vector3.Transform(Vector3,Quaternion)
Vector3.Add(Vector3,Vector3)
Vector3.Subtract(Vector3,Vector3)
Vector3.Multiply(Vector3,Vector3)
Vector3.Multiply(Vector3,Single)
Vector3.Multiply(Single,Vector3)
Vector3.Divide(Vector3,Vector3)
Vector3.Divide(Vector3,Single)
Vector3.Negate(Vector3)
Vector3.Equals(Vector3)
Vector3.Dot(Vector3,Vector3)
Vector3.Min(Vector3,Vector3)
Vector3.Max(Vector3,Vector3)
Vector3.Abs(Vector3)
Vector3.SquareRoot(Vector3)
Vector3.op_Addition(Vector3,Vector3)
Vector3.op_Subtraction(Vector3,Vector3)
Vector3.op_Multiply(Vector3,Vector3)
Vector3.op_Multiply(Vector3,Single)
Vector3.op_Multiply(Single,Vector3)
Vector3.op_Division(Vector3,Vector3)
Vector3.op_Division(Vector3,Single)
Vector3.op_UnaryNegation(Vector3)
Vector3.op_Equality(Vector3,Vector3)
Vector3.op_Inequality(Vector3,Vector3)
Vector4.get_Zero()
Vector4.get_One()
Vector4.get_UnitX()
Vector4.get_UnitY()
Vector4.get_UnitZ()
Vector4.get_UnitW()
Vector4.Equals(Object)
Vector4.Length()
Vector4.LengthSquared()
Vector4.Distance(Vector4,Vector4)
Vector4.DistanceSquared(Vector4,Vector4)
Vector4.Normalize(Vector4)
Vector4.Clamp(Vector4,Vector4,Vector4)
Vector4.Lerp(Vector4,Vector4,Single)
Vector4.Transform(Vector2,Matrix)
Vector4.Transform(Vector3,Matrix)
Vector4.Transform(Vector4,Matrix)
Vector4.Transform(Vector2,Quaternion)
Vector4.Transform(Vector3,Quaternion)
Vector4.Transform(Vector4,Quaternion)
Vector4.Add(Vector4,Vector4)
Vector4.Subtract(Vector4,Vector4)
Vector4.Multiply(Vector4,Vector4)
Vector4.Multiply(Vector4,Single)
Vector4.Multiply(Single,Vector4)
Vector4.Divide(Vector4,Vector4)
Vector4.Divide(Vector4,Single)
Vector4.Negate(Vector4)
Vector4.Equals(Vector4)
Vector4.Dot(Vector4,Vector4)
Vector4.Min(Vector4,Vector4)
Vector4.Max(Vector4,Vector4)
Vector4.Abs(Vector4)
Vector4.SquareRoot(Vector4)
Vector4.op_Addition(Vector4,Vector4)
Vector4.op_Subtraction(Vector4,Vector4)
Vector4.op_Multiply(Vector4,Vector4)
Vector4.op_Multiply(Vector4,Single)
Vector4.op_Multiply(Single,Vector4)
Vector4.op_Division(Vector4,Vector4)
Vector4.op_Division(Vector4,Single)
Vector4.op_UnaryNegation(Vector4)
Vector4.op_Equality(Vector4,Vector4)
Vector4.op_Inequality(Vector4,Vector4)
Plane.CreateFromVertices(Vector3,Vector3,Vector3)
Plane.Normalize(Plane)
Plane.Transform(Plane,Matrix)
Plane.Transform(Plane,Quaternion)
Plane.Dot(Plane,Vector4)
Plane.DotCoordinate(Plane,Vector3)
Plane.DotNormal(Plane,Vector3)
Plane.op_Equality(Plane,Plane)
Plane.op_Inequality(Plane,Plane)
Plane.Equals(Plane)
Plane.Equals(Object)
Quaternion.get_Identity()
Quaternion.get_IsIdentity()
Quaternion.Length()
Quaternion.LengthSquared()
Quaternion.Normalize(Quaternion)
Quaternion.Conjugate(Quaternion)
Quaternion.Inverse(Quaternion)
Quaternion.CreateFromAxisAngle(Vector3,Single)
Quaternion.CreateFromYawPitchRoll(Single,Single,Single)
Quaternion.CreateFromRotationMatrix(Matrix)
Quaternion.Dot(Quaternion,Quaternion)
Quaternion.Slerp(Quaternion,Quaternion,Single)
Quaternion.Lerp(Quaternion,Quaternion,Single)
Quaternion.Concatenate(Quaternion,Quaternion)
Quaternion.Negate(Quaternion)
Quaternion.Add(Quaternion,Quaternion)
Quaternion.Subtract(Quaternion,Quaternion)
Quaternion.Multiply(Quaternion,Quaternion)
Quaternion.Multiply(Quaternion,Single)
Quaternion.Divide(Quaternion,Quaternion)
Quaternion.op_UnaryNegation(Quaternion)
Quaternion.op_Addition(Quaternion,Quaternion)
Quaternion.op_Subtraction(Quaternion,Quaternion)
Quaternion.op_Multiply(Quaternion,Quaternion)
Quaternion.op_Multiply(Quaternion,Single)
Quaternion.op_Division(Quaternion,Quaternion)
Quaternion.op_Equality(Quaternion,Quaternion)
Quaternion.op_Inequality(Quaternion,Quaternion)
Quaternion.Equals(Quaternion)
Quaternion.Equals(Object)
Matrix.get_Identity()
Matrix.get_IsIdentity()
Matrix.get_Translation()
Matrix.set_Translation(Vector3)
Matrix.CreateBillboard(Vector3,Vector3,Vector3,Vector3)
Matrix.CreateConstrainedBillboard(Vector3,Vector3,Vector3,Vector3,Vector3)
Matrix.CreateTranslation(Vector3)
Matrix.CreateTranslation(Single,Single,Single)
Matrix.CreateScale(Single,Single,Single)
Matrix.CreateScale(Single,Single,Single,Vector3)
Matrix.CreateScale(Vector3)
Matrix.CreateScale(Vector3,Vector3)
Matrix.CreateScale(Single)
Matrix.CreateScale(Single,Vector3)
Matrix.CreateRotationX(Single)
Matrix.CreateRotationX(Single,Vector3)
Matrix.CreateRotationY(Single)
Matrix.CreateRotationY(Single,Vector3)
Matrix.CreateRotationZ(Single)
Matrix.CreateRotationZ(Single,Vector3)
Matrix.CreateFromAxisAngle(Vector3,Single)
Matrix.CreatePerspectiveFieldOfView(Single,Single,Single,Single)
Matrix.CreatePerspective(Single,Single,Single,Single)
Matrix.CreatePerspectiveOffCenter(Single,Single,Single,Single,Single,Single)
Matrix.CreateOrthographic(Single,Single,Single,Single)
Matrix.CreateOrthographicOffCenter(Single,Single,Single,Single,Single,Single)
Matrix.CreateLookAt(Vector3,Vector3,Vector3)
Matrix.CreateWorld(Vector3,Vector3,Vector3)
Matrix.CreateFromQuaternion(Quaternion)
Matrix.CreateFromYawPitchRoll(Single,Single,Single)
Matrix.CreateShadow(Vector3,Plane)
Matrix.CreateReflection(Plane)
Matrix.GetDeterminant()
Matrix.Invert(Matrix,Matrix4x4&)
Matrix.Decompose(Matrix,Vector3&,Quaternion&,Vector3&)
Matrix.Transform(Matrix,Quaternion)
Matrix.Transpose(Matrix)
Matrix.Lerp(Matrix,Matrix,Single)
Matrix.Negate(Matrix)
Matrix.Add(Matrix,Matrix)
Matrix.Subtract(Matrix,Matrix)
Matrix.Multiply(Matrix,Matrix)
Matrix.Multiply(Matrix,Single)
Matrix.op_UnaryNegation(Matrix)
Matrix.op_Addition(Matrix,Matrix)
Matrix.op_Subtraction(Matrix,Matrix)
Matrix.op_Multiply(Matrix,Matrix)
Matrix.op_Multiply(Matrix,Single)
Matrix.op_Equality(Matrix,Matrix)
Matrix.op_Inequality(Matrix,Matrix)
Matrix.Equals(Matrix)
Matrix.Equals(Object)
";
        static readonly HashSet<string> methodForward = new HashSet<string>();

        static FNA2Numerics() {
            using (StringReader reader = new StringReader(methodForwards))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length != 0)
                    {
                        methodForward.Add(line);
                    }
                }
            }
        }

        public static void Process(string path)
        {
            ModuleDefinition moduleDefinition;
            if (path.EndsWith("FNA.dll"))
            {
                using (moduleDefinition = ModuleDefinition.ReadModule(path, new ReaderParameters() { ReadWrite = true }))
                {
                    ProcessInternal(moduleDefinition);
                    moduleDefinition.Write();
                }
            }
            else
            {
                using (moduleDefinition = ModuleDefinition.ReadModule(path, new ReaderParameters() { ReadWrite = true }))
                {
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
            AssemblyNameReference Numerics = AssemblyNameReference.Parse("System.Numerics.Vectors, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
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
            ModuleDefinition extensionModuleDefinition = ModuleDefinition.ReadModule(Assembly.GetExecutingAssembly().Location);
            for (int i = 0; i < xnaTypes.Length; i++)
            {
                TypeDefinition extensionTypeDefinition = extensionModuleDefinition.GetType("FNA.Numerics", numericsTypes[i] + "Extension");
                TypeDefinition typeDefinition = moduleDefinition.GetType("Microsoft.Xna.Framework", xnaTypes[i]);
                typeDefinition.Namespace = extensionTypeDefinition.Namespace;
                typeDefinition.Name = extensionTypeDefinition.Name;
                typeDefinition.Interfaces.Clear();
                foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
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
            foreach (string typename in new string[] { "BoundingBox", "BoundingFrustum", "BoundingSphere", "Ray" })
            {
                TypeDefinition extensionTypeDefinition = extensionModuleDefinition.GetType("Microsoft.Xna.Framework", typename);
                TypeDefinition typeDefinition = moduleDefinition.GetType("Microsoft.Xna.Framework", typename);
                foreach (MethodDefinition extenstionMethodDefinition in extensionTypeDefinition.Methods)
                {
                    bool flag = false;
                    foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                    {
                        if (StringFromMethod(methodDefinition) == StringFromMethod(extenstionMethodDefinition))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        MethodDefinition methodDefinition = new MethodDefinition(extenstionMethodDefinition.Name, extenstionMethodDefinition.Attributes, moduleDefinition.ImportReference(extenstionMethodDefinition.ReturnType)) { Body = extenstionMethodDefinition.Body };
                        foreach (ParameterDefinition parameter in extenstionMethodDefinition.Parameters)
                        {
                            parameter.ParameterType = moduleDefinition.ImportReference(parameter.ParameterType);
                            methodDefinition.Parameters.Add(parameter);
                        }
                        typeDefinition.Methods.Add(methodDefinition);
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
                        processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[Microsoft.Xna.Framework." + xnaTypes[i] + ", Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]"));
                        processor.InsertBefore(i0, processor.Create(OpCodes.Ldstr, "[System.Numerics." + numericsTypes[i] + ", FNA, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]"));
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