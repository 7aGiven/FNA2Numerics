using Microsoft.Xna.Framework;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FNA.Numerics
{
    public static class PlaneExtension
    {
        public static int GetHashCode(this ref Plane plane)
        {
            return plane.Normal.X.GetHashCode() + plane.Normal.Y.GetHashCode() + plane.Normal.Z.GetHashCode() + plane.D.GetHashCode();
        }

        public static string ToString(this ref Plane plane)
        {
            StringBuilder sb = new StringBuilder("{Normal:{X:", 9 + 17 + 1 + 3 * 17);
            sb.Append(plane.Normal.X);
            sb.Append(" Y:");
            sb.Append(plane.Normal.Y);
            sb.Append(" Z:");
            sb.Append(plane.Normal.Z);
            sb.Append("} D:");
            sb.Append(plane.D);
            sb.Append('}');
            return sb.ToString();
        }

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

        public static PlaneIntersectionType Intersects(this ref Plane plane, BoundingBox box)
        {
            float radius = Vector3.Dot(Vector3.Abs(plane.Normal), (box.Max - box.Min) * 0.5f);
            float distance = Plane.DotCoordinate(plane, (box.Min + box.Max) * 0.5f);
            if (distance > radius)
            {
                return PlaneIntersectionType.Front;
            }
            if (distance < -radius)
            {
                return PlaneIntersectionType.Back;
            }
            return PlaneIntersectionType.Intersecting;
        }

        public static void Intersects(this ref Plane plane, ref BoundingBox box, out PlaneIntersectionType result)
        {
            float radius = Vector3.Dot(Vector3.Abs(plane.Normal), (box.Max - box.Min) * 0.5f);
            float distance = Plane.DotCoordinate(plane, (box.Min + box.Max) * 0.5f);
            if (distance > radius)
            {
                result = PlaneIntersectionType.Front;
                return;
            }
            if (distance < -radius)
            {
                result = PlaneIntersectionType.Back;
                return;
            }
            result = PlaneIntersectionType.Intersecting;
        }

        public static PlaneIntersectionType Intersects(this ref Plane plane, BoundingFrustum frustum)
        {
            if (ReferenceEquals(frustum, null))
            {
                throw new ArgumentNullException("frustum", "This method does not accept null for this parameter.");
            }
            PlaneIntersectionType result;
            frustum.Intersects(ref plane, out result);
            return result;
        }

        public static PlaneIntersectionType Intersects(this ref Plane plane, BoundingSphere sphere)
        {
            float radius = sphere.Radius;
            float distance = Plane.DotCoordinate(plane, sphere.Center);
            if (distance > radius)
            {
                return PlaneIntersectionType.Front;
            }
            if (distance < -radius)
            {
                return PlaneIntersectionType.Back;
            }
            return PlaneIntersectionType.Intersecting;
        }

        public static void Intersects(this ref Plane plane, ref BoundingSphere sphere, out PlaneIntersectionType result)
        {
            float radius = sphere.Radius;
            float distance = Plane.DotCoordinate(plane, sphere.Center);
            if (distance > radius)
            {
                result = PlaneIntersectionType.Front;
            }
            else if (distance < -radius)
            {
                result = PlaneIntersectionType.Back;
            }
            else
            {
                result = PlaneIntersectionType.Intersecting;
            }
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
