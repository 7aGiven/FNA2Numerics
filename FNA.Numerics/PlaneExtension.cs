using System.Numerics;
using System.Runtime.CompilerServices;

namespace FNA.Numerics
{
    public static class PlaneExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(this ref Plane plane, Vector4 value)
        {
            return Plane.Dot(plane, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dot(this ref Plane plane, ref Vector4 value, out float result)
        {
            result = Plane.Dot(plane, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DotCoordinate(this ref Plane plane, Vector3 value)
        {
            return Plane.DotCoordinate(plane, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DotCoordinate(this ref Plane plane, ref Vector3 value, out float result)
        {
            result = Plane.DotCoordinate(plane, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DotNormal(this ref Plane plane, Vector3 value)
        {
            return Plane.DotNormal(plane, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DotNormal(this ref Plane plane, ref Vector3 value, out float result)
        {
            result = Plane.DotNormal(plane, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Plane plane)
        {
            plane = Plane.Normalize(plane);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref Plane value, out Plane result)
        {
            result = Plane.Normalize(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Plane plane,
            ref Matrix4x4 matrix,
            out Plane result
        )
        {
            result = Plane.Transform(plane, matrix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(
            ref Plane plane,
            ref Quaternion rotation,
            out Plane result
        )
        {
            result = Plane.Transform(plane, rotation);
        }
    }
}
