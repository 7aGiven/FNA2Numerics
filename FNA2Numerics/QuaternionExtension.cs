using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FNA.Numerics
{
    public static class QuaternionExtension
    {
        public static int GetHashCode(this ref Quaternion quaternion)
        {
            return quaternion.X.GetHashCode() + quaternion.Y.GetHashCode() + quaternion.Z.GetHashCode() + quaternion.W.GetHashCode();
        }

        public static string ToString(this ref Quaternion quaternion)
        {
            StringBuilder sb = new StringBuilder("{X:", 1 + 3 * 17);
            sb.Append(quaternion.X);
            sb.Append(" Y:");
            sb.Append(quaternion.Y);
            sb.Append(" Z:");
            sb.Append(quaternion.Z);
            sb.Append(" W:");
            sb.Append(quaternion.W);
            sb.Append('}');
            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Conjugate(this ref Quaternion quaternion)
        {
            quaternion = Quaternion.Conjugate(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Quaternion quaternion)
        {
            quaternion = Quaternion.Normalize(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(
            ref Quaternion quaternion1,
            ref Quaternion quaternion2,
            out Quaternion result
        )
        {
            result = Quaternion.Add(quaternion1, quaternion2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Concatenate(
            ref Quaternion value1,
            ref Quaternion value2,
            out Quaternion result
        )
        {
            result = Quaternion.Concatenate(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Conjugate(ref Quaternion value, out Quaternion result)
        {
            result = Quaternion.Conjugate(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromAxisAngle(
            ref Vector3 axis,
            float angle,
            out Quaternion result
        )
        {
            result = Quaternion.CreateFromAxisAngle(axis, angle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromRotationMatrix(ref Matrix4x4 matrix, out Quaternion result)
        {
            result = Quaternion.CreateFromRotationMatrix(matrix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateFromYawPitchRoll(
            float yaw,
            float pitch,
            float roll,
            out Quaternion result
        )
        {
            result = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(
            ref Quaternion quaternion1,
            ref Quaternion quaternion2,
            out Quaternion result
        )
        {
            result = Quaternion.Divide(quaternion1, quaternion2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dot(
            ref Quaternion quaternion1,
            ref Quaternion quaternion2,
            out float result
        )
        {
            result = Quaternion.Dot(quaternion1, quaternion2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Inverse(ref Quaternion quaternion, out Quaternion result)
        {
            result = Quaternion.Inverse(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Lerp(
            ref Quaternion quaternion1,
            ref Quaternion quaternion2,
            float amount,
            out Quaternion result
        )
        {
            result = Quaternion.Lerp(quaternion1, quaternion2, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(
            ref Quaternion quaternion1,
            ref Quaternion quaternion2,
            out Quaternion result
        )
        {
            result = Quaternion.Multiply(quaternion1, quaternion2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(
            ref Quaternion quaternion1,
            float scaleFactor,
            out Quaternion result
        )
        {
            result = Quaternion.Multiply(quaternion1, scaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(ref Quaternion quaternion, out Quaternion result)
        {
            result = Quaternion.Negate(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref Quaternion quaternion, out Quaternion result)
        {
            result = Quaternion.Normalize(quaternion);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Slerp(
            ref Quaternion quaternion1,
            ref Quaternion quaternion2,
            float amount,
            out Quaternion result
        )
        {
            result = Quaternion.Slerp(quaternion1, quaternion2, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(
            ref Quaternion quaternion1,
            ref Quaternion quaternion2,
            out Quaternion result
        )
        {
            result = Quaternion.Subtract(quaternion1, quaternion2);
        }
    }
}