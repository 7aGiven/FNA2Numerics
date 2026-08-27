using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace FNA.Numerics
{
    public static class Vector3Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Vector3 vector)
        {
            vector = Vector3.Normalize(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = Vector3.Add(value1, value2);
        }

        public static Vector3 Barycentric(
            Vector3 value1,
            Vector3 value2,
            Vector3 value3,
            float amount1,
            float amount2
        )
        {
            return value1 * (1 - amount1 - amount2) + value2 * amount1 + value3 * amount2;
        }

        public static void Barycentric(
            ref Vector3 value1,
            ref Vector3 value2,
            ref Vector3 value3,
            float amount1,
            float amount2,
            out Vector3 result
        )
        {
            result = value1 * (1 - amount1 - amount2) + value2 * amount1 + value3 * amount2;
        }

        public static Vector3 CatmullRom(
            Vector3 value1,
            Vector3 value2,
            Vector3 value3,
            Vector3 value4,
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

        public static void CatmullRom(
            ref Vector3 value1,
            ref Vector3 value2,
            ref Vector3 value3,
            ref Vector3 value4,
            float amount,
            out Vector3 result
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
            ref Vector3 value1,
            ref Vector3 min,
            ref Vector3 max,
            out Vector3 result
        )
        {
            result = Vector3.Clamp(value1, min, max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            result = Vector3.Cross(vector1, vector2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            result = Vector3.Distance(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceSquared(
            ref Vector3 value1,
            ref Vector3 value2,
            out float result
        )
        {
            result = Vector3.DistanceSquared(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(ref Vector3 value1, float divider, out Vector3 result)
        {
            result = Vector3.Divide(value1, divider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = Vector3.Divide(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
        {
            result = Vector3.Dot(vector1, vector2);
        }

        public static Vector3 Hermite(
            Vector3 value1,
            Vector3 tangent1,
            Vector3 value2,
            Vector3 tangent2,
            float amount
        )
        {
            float amountSquared = amount * amount;
            float amountCubed = amount * amountSquared;
            float num = 3f * amountSquared - 2f * amountCubed;
            return (
                value1 * (1f - num) +
                tangent1 * (amountCubed - 2f * amountSquared + amount) +
                value2 * num +
                tangent2 * (amountCubed - amountSquared)
            );
        }

        public static void Hermite(
            ref Vector3 value1,
            ref Vector3 tangent1,
            ref Vector3 value2,
            ref Vector3 tangent2,
            float amount,
            out Vector3 result
        )
        {
            float amountSquared = amount * amount;
            float amountCubed = amount * amountSquared;
            float num = 3f * amountSquared - 2f * amountCubed;
            result = (
                value1 * (1f - num) +
                tangent1 * (amountCubed - 2f * amountSquared + amount) +
                value2 * num +
                tangent2 * (amountCubed - amountSquared)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Lerp(
            ref Vector3 value1,
            ref Vector3 value2,
            float amount,
            out Vector3 result
        )
        {
            result = Vector3.Lerp(value1, value2, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = Vector3.Max(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = Vector3.Min(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
        {
            result = Vector3.Multiply(value1, scaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = Vector3.Multiply(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(ref Vector3 value, out Vector3 result)
        {
            result = Vector3.Negate(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref Vector3 vector, out Vector3 result)
        {
            result = Vector3.Normalize(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            result = Vector3.Reflect(vector, normal);
        }

        public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
        {
            if (amount < 0) amount = 0f; else if (amount > 1f) amount = 1f;
            amount = amount * amount * (3f - 2f * amount);
            return value1 + (value2 - value1) * amount;
        }

        public static void SmoothStep(
            ref Vector3 value1,
            ref Vector3 value2,
            float amount,
            out Vector3 result
        )
        {
            if (amount < 0) amount = 0f; else if (amount > 1f) amount = 1f;
            amount = amount * amount * (3f - 2f * amount);
            result = value1 + (value2 - value1) * amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result = Vector3.Subtract(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Vector3 position, ref Matrix4x4 matrix, out Vector3 result)
        {
            result = Vector3.Transform(position, matrix);
        }

        public static void Transform(
            Vector3[] sourceArray,
            ref Matrix4x4 matrix,
            Vector3[] destinationArray
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
            int i = 0;
            do
            {
                destinationArray[i] = Vector3.Transform(sourceArray[i], matrix);
                i++;
            } while (i < sourceArray.Length);
        }

        public static void Transform(
            Vector3[] sourceArray,
            int sourceIndex,
            ref Matrix4x4 matrix,
            Vector3[] destinationArray,
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
            int i = 0;
            do
            {
                destinationArray[destinationIndex + i] = Vector3.Transform(sourceArray[sourceIndex + i], matrix);
                i++;
            } while (i < length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Vector3 value,
            ref Quaternion rotation,
            out Vector3 result
        )
        {
            result = Vector3.Transform(value, rotation);
        }

        public static void Transform(
            Vector3[] sourceArray,
            ref Quaternion rotation,
            Vector3[] destinationArray
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
            int i = 0;
            do
            {
                destinationArray[i] = Vector3.Transform(sourceArray[i], rotation);
                i++;
            } while (i < sourceArray.Length);
        }

        public static void Transform(
            Vector3[] sourceArray,
            int sourceIndex,
            ref Quaternion rotation,
            Vector3[] destinationArray,
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
            int i = 0;
            do
            {
                destinationArray[destinationIndex + i] = Vector3.Transform(sourceArray[sourceIndex + i], rotation);
                i++;
            } while (i < length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormal(
            ref Vector3 normal,
            ref Matrix4x4 matrix,
            out Vector3 result
        )
        {
            result = Vector3.TransformNormal(normal, matrix);
        }

        public static void TransformNormal(
            Vector3[] sourceArray,
            ref Matrix4x4 matrix,
            Vector3[] destinationArray
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
            int i = 0;
            do
            {
                destinationArray[i] = Vector3.TransformNormal(sourceArray[i], matrix);
                i++;
            } while (i < sourceArray.Length);
        }

        public static void TransformNormal(
            Vector3[] sourceArray,
            int sourceIndex,
            ref Matrix4x4 matrix,
            Vector3[] destinationArray,
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
            int i = 0;
            do
            {
                destinationArray[destinationIndex + i] = Vector3.TransformNormal(sourceArray[sourceIndex + i], matrix);
                i++;
            } while (i < length);
        }
    }
}