using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FNA.Numerics
{
    public static class Matrix4x4Extension
    {
        public static int GetHashCode(this ref Matrix4x4 matrix)
        {
            return (
                matrix.M11.GetHashCode() + matrix.M12.GetHashCode() + matrix.M13.GetHashCode() + matrix.M14.GetHashCode() +
                matrix.M21.GetHashCode() + matrix.M22.GetHashCode() + matrix.M23.GetHashCode() + matrix.M24.GetHashCode() +
                matrix.M31.GetHashCode() + matrix.M32.GetHashCode() + matrix.M33.GetHashCode() + matrix.M34.GetHashCode() +
                matrix.M41.GetHashCode() + matrix.M42.GetHashCode() + matrix.M43.GetHashCode() + matrix.M44.GetHashCode()
            );
        }

        public static string ToString(this ref Matrix4x4 matrix)
        {
            StringBuilder sb = new StringBuilder("{ ", 5 + 2 * 3 + 16 * 19);
            sb.Append("{M11:"); sb.Append(matrix.M11);
            sb.Append(" M12:"); sb.Append(matrix.M12);
            sb.Append(" M13:"); sb.Append(matrix.M13);
            sb.Append(" M14:"); sb.Append(matrix.M14);
            sb.Append("} ");
            sb.Append("{M21:"); sb.Append(matrix.M21);
            sb.Append(" M22:"); sb.Append(matrix.M22);
            sb.Append(" M23:"); sb.Append(matrix.M23);
            sb.Append(" M24:"); sb.Append(matrix.M24);
            sb.Append("} ");
            sb.Append("{M31:"); sb.Append(matrix.M31);
            sb.Append(" M32:"); sb.Append(matrix.M32);
            sb.Append(" M33:"); sb.Append(matrix.M33);
            sb.Append(" M34:"); sb.Append(matrix.M34);
            sb.Append("} ");
            sb.Append("{M41:"); sb.Append(matrix.M41);
            sb.Append(" M42:"); sb.Append(matrix.M42);
            sb.Append(" M43:"); sb.Append(matrix.M43);
            sb.Append(" M44:"); sb.Append(matrix.M44);
            sb.Append("} }");
            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Decompose(
            this ref Matrix4x4 matrix,
            out Vector3 scale,
            out Quaternion rotation,
            out Vector3 translation
        )
        {
            return Matrix4x4.Decompose(matrix, out scale, out rotation, out translation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref Matrix4x4 matrix1, ref Matrix4x4 matrix2, out Matrix4x4 result)
        {
            result = Matrix4x4.Add(matrix1, matrix2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector)
        {
            return Matrix4x4.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, cameraForwardVector.GetValueOrDefault(Vector3.UnitZ));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromAxisAngle(
            ref Vector3 axis,
            float angle,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateFromAxisAngle(axis, angle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateFromQuaternion(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromYawPitchRoll(
            float yaw,
            float pitch,
            float roll,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateFromYawPitchRoll(yaw, pitch, roll);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateLookAt(
            ref Vector3 cameraPosition,
            ref Vector3 cameraTarget,
            ref Vector3 cameraUpVector,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateReflection(ref Plane value, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateReflection(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateRotationX(float radians, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateRotationX(radians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateRotationY(float radians, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateRotationY(radians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateRotationZ(float radians, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateRotationZ(radians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateScale(float scale, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateScale(scale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateScale(
            float xScale,
            float yScale,
            float zScale,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateScale(xScale, yScale, zScale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateScale(ref Vector3 scales, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateScale(scales);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateShadow(
            ref Vector3 lightDirection,
            ref Plane plane,
            out Matrix4x4 result)
        {
            result = Matrix4x4.CreateShadow(lightDirection, plane);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateTranslation(ref Vector3 position, out Matrix4x4 result)
        {
            result = Matrix4x4.CreateTranslation(position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateTranslation(
            float xPosition,
            float yPosition,
            float zPosition,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateTranslation(xPosition, yPosition, zPosition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateWorld(
            ref Vector3 position,
            ref Vector3 forward,
            ref Vector3 up,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.CreateWorld(position, forward, up);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Divide(Matrix4x4 matrix1, float divider)
        {
            return Matrix4x4.Multiply(matrix1, 1f / divider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(ref Matrix4x4 matrix1, float divider, out Matrix4x4 result)
        {
            result = Matrix4x4.Multiply(matrix1, 1f / divider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Invert(Matrix4x4 matrix)
        {
            Matrix4x4.Invert(matrix, out matrix);
            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(ref Matrix4x4 matrix, out Matrix4x4 result)
        {
            Matrix4x4.Invert(matrix, out result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Lerp(
            ref Matrix4x4 matrix1,
            ref Matrix4x4 matrix2,
            float amount,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.Lerp(matrix1, matrix2, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Matrix4x4 matrix1, ref Matrix4x4 matrix2, out Matrix4x4 result)
        {
            result = Matrix4x4.Multiply(matrix1, matrix2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Matrix4x4 matrix1, float scaleFactor, out Matrix4x4 result)
        {
            result = Matrix4x4.Multiply(matrix1, scaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(ref Matrix4x4 matrix, out Matrix4x4 result)
        {
            result = Matrix4x4.Negate(matrix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(ref Matrix4x4 matrix1, ref Matrix4x4 matrix2, out Matrix4x4 result)
        {
            result = Matrix4x4.Subtract(matrix1, matrix2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Matrix4x4 value,
            ref Quaternion rotation,
            out Matrix4x4 result
        )
        {
            result = Matrix4x4.Transform(value, rotation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transpose(ref Matrix4x4 matrix, out Matrix4x4 result)
        {
            result = Matrix4x4.Transpose(matrix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 op_Division(Matrix4x4 matrix, float divider)
        {
            return Matrix4x4.Multiply(matrix, 1f / divider);
        }
    }
}
