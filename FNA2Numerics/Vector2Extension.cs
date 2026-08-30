using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FNA.Numerics
{
    public static class Vector2Extension
    {
        public static int GetHashCode(this ref Vector2 vector)
        {
            return vector.X.GetHashCode() + vector.Y.GetHashCode();
        }

        public static string ToString(this ref Vector2 vector)
        {
            StringBuilder sb = new StringBuilder("{X:", 1 + 2 * 17);
            sb.Append(vector.X);
            sb.Append(" Y:");
            sb.Append(vector.Y);
            sb.Append('}');
            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Vector2 vector)
        {
            vector = Vector2.Normalize(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result = Vector2.Add(value1, value2);
        }

        public static Vector2 Barycentric(
            Vector2 value1,
            Vector2 value2,
            Vector2 value3,
            float amount1,
            float amount2
        )
        {
            return value1 * (1 - amount1 - amount2) + value2 * amount1 + value3 * amount2;
        }

        public static void Barycentric(
            ref Vector2 value1,
            ref Vector2 value2,
            ref Vector2 value3,
            float amount1,
            float amount2,
            out Vector2 result
        )
        {
            result = value1 * (1 - amount1 - amount2) + value2 * amount1 + value3 * amount2;
        }

        public static Vector2 CatmullRom(
            Vector2 value1,
            Vector2 value2,
            Vector2 value3,
            Vector2 value4,
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
            ref Vector2 value1,
            ref Vector2 value2,
            ref Vector2 value3,
            ref Vector2 value4,
            float amount,
            out Vector2 result
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
            ref Vector2 value1,
            ref Vector2 min,
            ref Vector2 max,
            out Vector2 result
        )
        {
            result = Vector2.Clamp(value1, min, max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            result = Vector2.Distance(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DistanceSquared(
            ref Vector2 value1,
            ref Vector2 value2,
            out float result
        )
        {
            result = Vector2.DistanceSquared(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
        {
            result = Vector2.Divide(value1, divider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result = Vector2.Divide(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dot(ref Vector2 vector1, ref Vector2 vector2, out float result)
        {
            result = Vector2.Dot(vector1, vector2);
        }

        public static Vector2 Hermite(
            Vector2 value1,
            Vector2 tangent1,
            Vector2 value2,
            Vector2 tangent2,
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
            ref Vector2 value1,
            ref Vector2 tangent1,
            ref Vector2 value2,
            ref Vector2 tangent2,
            float amount,
            out Vector2 result
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
            ref Vector2 value1,
            ref Vector2 value2,
            float amount,
            out Vector2 result
        )
        {
            result = Vector2.Lerp(value1, value2, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result = Vector2.Max(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result = Vector2.Min(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
        {
            result = Vector2.Multiply(value1, scaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result = Vector2.Multiply(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(ref Vector2 value, out Vector2 result)
        {
            result = Vector2.Negate(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref Vector2 vector, out Vector2 result)
        {
            result = Vector2.Normalize(vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
        {
            result = Vector2.Reflect(vector, normal);
        }

        public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
        {
            if (amount < 0) amount = 0f; else if (amount > 1f) amount = 1f;
            amount = amount * amount * (3f - 2f * amount);
            return value1 + (value2 - value1) * amount;
        }

        public static void SmoothStep(
            ref Vector2 value1,
            ref Vector2 value2,
            float amount,
            out Vector2 result
        )
        {
            if (amount < 0) amount = 0f; else if (amount > 1f) amount = 1f;
            amount = amount * amount * (3f - 2f * amount);
            result = value1 + (value2 - value1) * amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result = Vector2.Subtract(value1, value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Vector2 position, ref Matrix4x4 matrix, out Vector2 result)
        {
            result = Vector2.Transform(position, matrix);
        }

        public static void Transform(
            Vector2[] sourceArray,
            ref Matrix4x4 matrix,
            Vector2[] destinationArray
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
                destinationArray[i] = Vector2.Transform(sourceArray[i], matrix);
                i++;
            } while (i < sourceArray.Length);
        }

        public static void Transform(
            Vector2[] sourceArray,
            int sourceIndex,
            ref Matrix4x4 matrix,
            Vector2[] destinationArray,
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
                destinationArray[destinationIndex + i] = Vector2.Transform(sourceArray[sourceIndex + i], matrix);
                i++;
            } while (i < length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Vector2 value,
            ref Quaternion rotation,
            out Vector2 result
        )
        {
            result = Vector2.Transform(value, rotation);
        }

        public static void Transform(
            Vector2[] sourceArray,
            ref Quaternion rotation,
            Vector2[] destinationArray
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
                destinationArray[i] = Vector2.Transform(sourceArray[i], rotation);
                i++;
            } while (i < sourceArray.Length);
        }

        public static void Transform(
            Vector2[] sourceArray,
            int sourceIndex,
            ref Quaternion rotation,
            Vector2[] destinationArray,
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
                destinationArray[destinationIndex + i] = Vector2.Transform(sourceArray[sourceIndex + i], rotation);
                i++;
            } while (i < length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormal(
            ref Vector2 normal,
            ref Matrix4x4 matrix,
            out Vector2 result
        )
        {
            result = Vector2.TransformNormal(normal, matrix);
        }

        public static void TransformNormal(
            Vector2[] sourceArray,
            ref Matrix4x4 matrix,
            Vector2[] destinationArray
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
                destinationArray[i] = Vector2.TransformNormal(sourceArray[i], matrix);
                i++;
            } while (i < sourceArray.Length);
        }

        public static void TransformNormal(
            Vector2[] sourceArray,
            int sourceIndex,
            ref Matrix4x4 matrix,
            Vector2[] destinationArray,
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
                destinationArray[destinationIndex + i] = Vector2.TransformNormal(sourceArray[sourceIndex + i], matrix);
                i++;
            } while (i < length);
        }
    }
}