using System.Numerics;

namespace FNA.Numerics
{
    public static class Matrix4x4Extension
    {
        public static bool Decompose(
            this ref Matrix4x4 matrix,
            out Vector3 scale,
            out Quaternion rotation,
            out Vector3 translation
        )
        {
            return Matrix4x4.Decompose(matrix, out scale, out rotation, out translation);
        }
        public static float Determinant(this ref Matrix4x4 matrix)
        {
            float m = matrix.M11;
            float m2 = matrix.M12;
            float m3 = matrix.M13;
            float m4 = matrix.M14;
            float m5 = matrix.M21;
            float m6 = matrix.M22;
            float m7 = matrix.M23;
            float m8 = matrix.M24;
            float m9 = matrix.M31;
            float m10 = matrix.M32;
            float m11 = matrix.M33;
            float m12 = matrix.M34;
            float m13 = matrix.M41;
            float m14 = matrix.M42;
            float m15 = matrix.M43;
            float m16 = matrix.M44;
            float num = m11 * m16 - m12 * m15;
            float num2 = m10 * m16 - m12 * m14;
            float num3 = m10 * m15 - m11 * m14;
            float num4 = m9 * m16 - m12 * m13;
            float num5 = m9 * m15 - m11 * m13;
            float num6 = m9 * m14 - m10 * m13;
            return m * (m6 * num - m7 * num2 + m8 * num3) - m2 * (m5 * num - m7 * num4 + m8 * num5) + m3 * (m5 * num2 - m6 * num4 + m8 * num6) - m4 * (m5 * num3 - m6 * num5 + m7 * num6);
        }
        public static void Add(ref Matrix4x4 matrix1, ref Matrix4x4 matrix2, out Matrix4x4 result)
        {
            result = Matrix4x4.Add(matrix1, matrix2);
        }
        public static Matrix4x4 CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector)
        {
            return Matrix4x4.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, cameraForwardVector.GetValueOrDefault(Vector3.UnitZ));
        }
        public static void CreateBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, Vector3? cameraForwardVector, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, cameraForwardVector.GetValueOrDefault(Vector3.UnitZ));
        }
        //public static Matrix4x4 CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector)
        //{
        //    return Matrix4x4.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, cameraForwardVector.GetValueOrDefault(Vector3.UnitZ), );
        //}
        //public static void CreateConstrainedBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector, out Matrix4x4 result)
        //{
        //}
        public static void CreateFromAxisAngle(
            ref Vector3 axis,
            float angle,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateFromAxisAngle(axis, angle);
        }
        public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateFromQuaternion(quaternion);
        }
        public static void CreateFromYawPitchRoll(
            float yaw,
            float pitch,
            float roll,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateFromYawPitchRoll(yaw, pitch, roll);
        }
        public static void CreateLookAt(
            ref Vector3 cameraPosition,
            ref Vector3 cameraTarget,
            ref Vector3 cameraUpVector,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
        }
        public static void CreateOrthographic(
            float width,
            float height,
            float zNearPlane,
            float zFarPlane,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateOrthographic(width, height, zNearPlane, zFarPlane);
        }
        public static void CreateOrthographicOffCenter(
            float left,
            float right,
            float bottom,
            float top,
            float zNearPlane,
            float zFarPlane,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane);
        }
        public static void CreatePerspective(
            float width,
            float height,
            float nearPlaneDistance,
            float farPlaneDistance,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreatePerspective(width, height, nearPlaneDistance, farPlaneDistance);
        }
        public static void CreatePerspectiveFieldOfView(
            float fieldOfView,
            float aspectRatio,
            float nearPlaneDistance,
            float farPlaneDistance,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
        }
        public static void CreatePerspectiveOffCenter(
            float left,
            float right,
            float bottom,
            float top,
            float nearPlaneDistance,
            float farPlaneDistance,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreatePerspectiveOffCenter(left, right, bottom, top, nearPlaneDistance, farPlaneDistance);
        }
        public static void CreateRotationX(float radians, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateRotationX(radians);
        }
        public static void CreateRotationY(float radians, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateRotationY(radians);
        }
        public static void CreateRotationZ(float radians, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateRotationZ(radians);
        }
        public static void CreateScale(float scale, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateScale(scale);
        }
        public static void CreateScale(
            float xScale,
            float yScale,
            float zScale,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateScale(xScale, yScale, zScale);
        }
        public static void CreateScale(ref Vector3 scales, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateScale(scales);
        }
        public static void CreateShadow(
            ref Vector3 lightDirection,
            ref Plane plane,
            out Matrix4x4 result)
        {
            result = Matrix4x4.CreateShadow(lightDirection, plane);
        }
        public static void CreateTranslation(ref Vector3 position, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateTranslation(position);
        }
        public static void CreateTranslation(
            float xPosition,
            float yPosition,
            float zPosition,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateTranslation(xPosition, yPosition, zPosition);
        }
        public static void CreateReflection(ref Plane value, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateReflection(value);
        }
        public static void CreateWorld(
            ref Vector3 position,
            ref Vector3 forward,
            ref Vector3 up,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateWorld(position, forward, up);
        }
        public static Matrix4x4 Divide(Matrix4x4 matrix1, Matrix4x4 matrix2)
        {
            matrix1.M11 = matrix1.M11 / matrix2.M11;
            matrix1.M12 = matrix1.M12 / matrix2.M12;
            matrix1.M13 = matrix1.M13 / matrix2.M13;
            matrix1.M14 = matrix1.M14 / matrix2.M14;
            matrix1.M21 = matrix1.M21 / matrix2.M21;
            matrix1.M22 = matrix1.M22 / matrix2.M22;
            matrix1.M23 = matrix1.M23 / matrix2.M23;
            matrix1.M24 = matrix1.M24 / matrix2.M24;
            matrix1.M31 = matrix1.M31 / matrix2.M31;
            matrix1.M32 = matrix1.M32 / matrix2.M32;
            matrix1.M33 = matrix1.M33 / matrix2.M33;
            matrix1.M34 = matrix1.M34 / matrix2.M34;
            matrix1.M41 = matrix1.M41 / matrix2.M41;
            matrix1.M42 = matrix1.M42 / matrix2.M42;
            matrix1.M43 = matrix1.M43 / matrix2.M43;
            matrix1.M44 = matrix1.M44 / matrix2.M44;
            return matrix1;
        }
        public static void Divide(ref Matrix4x4 matrix1, ref Matrix4x4 matrix2, out Matrix4x4 result)
        {
            result.M11 = matrix1.M11 / matrix2.M11;
            result.M12 = matrix1.M12 / matrix2.M12;
            result.M13 = matrix1.M13 / matrix2.M13;
            result.M14 = matrix1.M14 / matrix2.M14;
            result.M21 = matrix1.M21 / matrix2.M21;
            result.M22 = matrix1.M22 / matrix2.M22;
            result.M23 = matrix1.M23 / matrix2.M23;
            result.M24 = matrix1.M24 / matrix2.M24;
            result.M31 = matrix1.M31 / matrix2.M31;
            result.M32 = matrix1.M32 / matrix2.M32;
            result.M33 = matrix1.M33 / matrix2.M33;
            result.M34 = matrix1.M34 / matrix2.M34;
            result.M41 = matrix1.M41 / matrix2.M41;
            result.M42 = matrix1.M42 / matrix2.M42;
            result.M43 = matrix1.M43 / matrix2.M43;
            result.M44 = matrix1.M44 / matrix2.M44;
        }
        public static Matrix4x4 Divide(Matrix4x4 matrix1, float divider)
        {
            return Matrix4x4.Multiply(matrix1, 1f / divider);
        }
        public static void Divide(ref Matrix4x4 matrix1, float divider, out Matrix4x4 result)
        {
            result = Matrix4x4.Multiply(matrix1, 1f / divider);
        }
        public static Matrix4x4 Invert(Matrix4x4 matrix)
        {
            Matrix4x4.Invert(matrix, out matrix);
            return matrix;
        }
        public static void Invert(ref Matrix4x4 matrix, out Matrix4x4 result)
        {
            Matrix4x4.Invert(matrix, out result);
        }
        public static void Lerp(
            ref Matrix4x4 matrix1,
            ref Matrix4x4 matrix2,
            float amount,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.Lerp(matrix1, matrix2, amount);
        }
        public static void Multiply(ref Matrix4x4 matrix1, ref Matrix4x4 matrix2, out Matrix4x4 result)
        {
            result = Matrix4x4.Multiply(matrix1, matrix2);
        }
        public static void Multiply(ref Matrix4x4 matrix1, float scaleFactor, out Matrix4x4 result)
        {
            result = Matrix4x4.Multiply(matrix1, scaleFactor);
        }
        public static void Negate(ref Matrix4x4 matrix, out Matrix4x4 result)
        {
            result = Matrix4x4.Negate(matrix);
        }
        public static void Subtract(ref Matrix4x4 matrix1, ref Matrix4x4 matrix2, out Matrix4x4 result)
        {
            result = Matrix4x4.Subtract(matrix1, matrix2);
        }
        public static void Transpose(ref Matrix4x4 matrix, out Matrix4x4 result)
        {
            result = Matrix4x4.Transpose(matrix);
        }
        public static void Transform(
            ref Matrix4x4 value,
            ref Quaternion rotation,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.Transform(value, rotation);
        }
        public static Matrix4x4 op_Division(Matrix4x4 matrix1, Matrix4x4 matrix2)
        {
            matrix1.M11 = matrix1.M11 / matrix2.M11;
            matrix1.M12 = matrix1.M12 / matrix2.M12;
            matrix1.M13 = matrix1.M13 / matrix2.M13;
            matrix1.M14 = matrix1.M14 / matrix2.M14;
            matrix1.M21 = matrix1.M21 / matrix2.M21;
            matrix1.M22 = matrix1.M22 / matrix2.M22;
            matrix1.M23 = matrix1.M23 / matrix2.M23;
            matrix1.M24 = matrix1.M24 / matrix2.M24;
            matrix1.M31 = matrix1.M31 / matrix2.M31;
            matrix1.M32 = matrix1.M32 / matrix2.M32;
            matrix1.M33 = matrix1.M33 / matrix2.M33;
            matrix1.M34 = matrix1.M34 / matrix2.M34;
            matrix1.M41 = matrix1.M41 / matrix2.M41;
            matrix1.M42 = matrix1.M42 / matrix2.M42;
            matrix1.M43 = matrix1.M43 / matrix2.M43;
            matrix1.M44 = matrix1.M44 / matrix2.M44;
            return matrix1;
        }
        public static Matrix4x4 op_Division(Matrix4x4 matrix, float divider)
        {
            return Matrix4x4.Multiply(matrix, 1f / divider);
        }
    }
}
