using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace FNA.Numerics
{
    public static class Vector4Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Vector4 vector)
        {
            vector = Vector4.Normalize(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result = Vector4.Add(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Barycentric(
            Vector4 value1,
            Vector4 value2,
            Vector4 value3,
            float amount1,
            float amount2
        )
        {
            return value1 * (1 - amount1 - amount2) + value2 * amount1 + value3 * amount2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Barycentric(
            ref Vector4 value1,
            ref Vector4 value2,
            ref Vector4 value3,
            float amount1,
            float amount2,
            out Vector4 result
        )
        {
            result = value1 * (1 - amount1 - amount2) + value2 * amount1 + value3 * amount2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 CatmullRom(
            Vector4 value1,
            Vector4 value2,
            Vector4 value3,
            Vector4 value4,
            float amount
        )
        {
            float amountSquared = amount * amount;
            float amountCubed = amount * amountSquared;
            return 0.5f * (
                value1 * (2 * amountSquared - amount - amountCubed) +
                value2 * (2 - 5 * amountSquared + 3 * amountCubed) +
                value3 * (amount + 4 * amountSquared - 3 * amountCubed) +
                value4 * (amountCubed - amountSquared)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CatmullRom(
            ref Vector4 value1,
            ref Vector4 value2,
            ref Vector4 value3,
            ref Vector4 value4,
            float amount,
            out Vector4 result
        )
        {
            float amountSquared = amount * amount;
            float amountCubed = amount * amountSquared;
            result = 0.5f * (
                value1 * (2 * amountSquared - amount - amountCubed) +
                value2 * (2 - 5 * amountSquared + 3 * amountCubed) +
                value3 * (amount + 4 * amountSquared - 3 * amountCubed) +
                value4 * (amountCubed - amountSquared)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(
            ref Vector4 value1,
            ref Vector4 min,
            ref Vector4 max,
            out Vector4 result
        )
        {
            result = Vector4.Clamp(value1, min, max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Distance(ref Vector4 value1, ref Vector4 value2, out float result)
        {
            result = Vector4.Distance(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceSquared(
            ref Vector4 value1,
            ref Vector4 value2,
            out float result
        )
        {
            result = Vector4.DistanceSquared(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(ref Vector4 value1, float divider, out Vector4 result)
        {
            result = Vector4.Divide(value1, divider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result = Vector4.Divide(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out float result)
        {
            result = Vector4.Dot(vector1, vector2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Hermite(
            Vector4 value1,
            Vector4 tangent1,
            Vector4 value2,
            Vector4 tangent2,
            float amount
        )
        {
            float amountSquared = amount * amount;
            float amountCubed = amount * amountSquared;
            float num = 3f * amountSquared - 2f * amountCubed;
            return value1 * (1f - num) + value2 * num + tangent1 * (amountCubed - 2f * amountSquared + amount) + tangent2 * (amountCubed - amountSquared);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Hermite(
            ref Vector4 value1,
            ref Vector4 tangent1,
            ref Vector4 value2,
            ref Vector4 tangent2,
            float amount,
            out Vector4 result
        )
        {
            float amountSquared = amount * amount;
            float amountCubed = amount * amountSquared;
            float num = 3f * amountSquared - 2f * amountCubed;
            result = value1 * (1f - num) + value2 * num + tangent1 * (amountCubed - 2f * amountSquared + amount) + tangent2 * (amountCubed - amountSquared);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Lerp(
            ref Vector4 value1,
            ref Vector4 value2,
            float amount,
            out Vector4 result
        )
        {
            result = Vector4.Lerp(value1, value2, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result = Vector4.Max(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result = Vector4.Min(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector4 value1, float scaleFactor, out Vector4 result)
        {
            result = Vector4.Multiply(value1, scaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result = Vector4.Multiply(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(ref Vector4 value, out Vector4 result)
        {
            result = Vector4.Negate(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref Vector4 vector, out Vector4 result)
        {
            result = Vector4.Normalize(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SmoothStep(Vector4 value1, Vector4 value2, float amount)
        {
            if (amount > 1f) amount = 1f;
            if (amount < 0f) amount = 0f;
            amount = amount * amount * (3f - 2f * amount);
            return value1 + (value2 - value1) * amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SmoothStep(
            ref Vector4 value1,
            ref Vector4 value2,
            float amount,
            out Vector4 result
        )
        {
            if (amount > 1f) amount = 1f;
            if (amount < 0f) amount = 0f;
            amount = amount * amount * (3f - 2f * amount);
            result = value1 + (value2 - value1) * amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result = Vector4.Subtract(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Vector2 position, ref Matrix4x4 matrix, out Vector4 result)
        {
            result = Vector4.Transform(position, matrix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Vector3 position, ref Matrix4x4 matrix, out Vector4 result)
        {
            result = Vector4.Transform(position, matrix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Vector4 vector, ref Matrix4x4 matrix, out Vector4 result)
        {
            result = Vector4.Transform(vector, matrix);
        }

        public static void Transform(
            Vector4[] sourceArray,
            ref Matrix4x4 matrix,
            Vector4[] destinationArray
        )
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length > destinationArray.Length)
            {
                throw new ArgumentException("Target array size must be equal or bigger than source array size.");
            }
            for (int i = sourceArray.Length - 1; i >= 0; i--)
            {
                destinationArray[i] = Vector4.Transform(sourceArray[i], matrix);
            }
        }

        public static void Transform(
            Vector4[] sourceArray,
            int sourceIndex,
            ref Matrix4x4 matrix,
            Vector4[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceIndex + length > sourceArray.Length)
            {
                throw new ArgumentException("Source array must be equal or bigger than requested length.");
            }
            if (destinationIndex + length > destinationArray.Length)
            {
                throw new ArgumentException("Target array size must be equal or bigger than source array size.");
            }
            for (int i = length - 1; i >= 0; i--)
            {
                destinationArray[destinationIndex + i] = Vector4.Transform(sourceArray[sourceIndex + i], matrix);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Vector2 value,
            ref Quaternion rotation,
            out Vector4 result
        )
        {
            result = Vector4.Transform(value, rotation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Vector3 value,
            ref Quaternion rotation,
            out Vector4 result
        )
        {
            result = Vector4.Transform(value, rotation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Vector4 value,
            ref Quaternion rotation,
            out Vector4 result
        )
        {
            result = Vector4.Transform(value, rotation);
        }

        public static void Transform(
            Vector4[] sourceArray,
            ref Quaternion rotation,
            Vector4[] destinationArray
        )
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length > destinationArray.Length)
            {
                throw new ArgumentException("Target array size must be equal or bigger than source array size.");
            }
            for (int i = sourceArray.Length - 1; i >= 0; i--)
            {
                destinationArray[i] = Vector4.Transform(sourceArray[i], rotation);
            }
        }

        public static void Transform(
            Vector4[] sourceArray,
            int sourceIndex,
            ref Quaternion rotation,
            Vector4[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceIndex + length > sourceArray.Length)
            {
                throw new ArgumentException("Source array must be equal or bigger than requested length.");
            }
            if (destinationIndex + length > destinationArray.Length)
            {
                throw new ArgumentException("Target array size must be equal or bigger than source array size.");
            }
            for (int i = length - 1; i >= 0; i--)
            {
                destinationArray[destinationIndex + i] = Vector4.Transform(sourceArray[sourceIndex + i], rotation);
            }
        }
    }
}