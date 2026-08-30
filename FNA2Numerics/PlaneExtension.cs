using Microsoft.Xna.Framework;
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
            Vector3 positiveVertex;
            Vector3 negativeVertex;

            if (plane.Normal.X >= 0)
            {
                positiveVertex.X = box.Max.X;
                negativeVertex.X = box.Min.X;
            }
            else
            {
                positiveVertex.X = box.Min.X;
                negativeVertex.X = box.Max.X;
            }

            if (plane.Normal.Y >= 0)
            {
                positiveVertex.Y = box.Max.Y;
                negativeVertex.Y = box.Min.Y;
            }
            else
            {
                positiveVertex.Y = box.Min.Y;
                negativeVertex.Y = box.Max.Y;
            }

            if (plane.Normal.Z >= 0)
            {
                positiveVertex.Z = box.Max.Z;
                negativeVertex.Z = box.Min.Z;
            }
            else
            {
                positiveVertex.Z = box.Min.Z;
                negativeVertex.Z = box.Max.Z;
            }

            if (Plane.DotCoordinate(plane, negativeVertex) > 0f)
            {
                return PlaneIntersectionType.Front;
            }
            if (Plane.DotCoordinate(plane, positiveVertex) < 0f)
            {
                return PlaneIntersectionType.Back;
            }
            return PlaneIntersectionType.Intersecting;
        }

        public static void Intersects(this ref Plane plane, ref BoundingBox box, out PlaneIntersectionType result)
        {
            Vector3 positiveVertex;
            Vector3 negativeVertex;

            if (plane.Normal.X >= 0)
            {
                positiveVertex.X = box.Max.X;
                negativeVertex.X = box.Min.X;
            }
            else
            {
                positiveVertex.X = box.Min.X;
                negativeVertex.X = box.Max.X;
            }

            if (plane.Normal.Y >= 0)
            {
                positiveVertex.Y = box.Max.Y;
                negativeVertex.Y = box.Min.Y;
            }
            else
            {
                positiveVertex.Y = box.Min.Y;
                negativeVertex.Y = box.Max.Y;
            }

            if (plane.Normal.Z >= 0)
            {
                positiveVertex.Z = box.Max.Z;
                negativeVertex.Z = box.Min.Z;
            }
            else
            {
                positiveVertex.Z = box.Min.Z;
                negativeVertex.Z = box.Max.Z;
            }

            if (Plane.DotCoordinate(plane, negativeVertex) > 0f)
            {
                result = PlaneIntersectionType.Front;
                return;
            }
            if (Plane.DotCoordinate(plane, positiveVertex) < 0f)
            {
                result = PlaneIntersectionType.Back;
                return;
            }
            result = PlaneIntersectionType.Intersecting;
        }

        public static PlaneIntersectionType Intersects(this ref Plane plane, BoundingSphere sphere)
        {
            float distance = Plane.DotCoordinate(plane, sphere.Center);
            if (distance > sphere.Radius)
            {
                return PlaneIntersectionType.Front;
            }
            if (distance < -sphere.Radius)
            {
                return PlaneIntersectionType.Back;
            }
            return PlaneIntersectionType.Intersecting;
        }

        public static void Intersects(this ref Plane plane, ref BoundingSphere sphere, out PlaneIntersectionType result)
        {
            float distance = Plane.DotCoordinate(plane, sphere.Center);
            if (distance > sphere.Radius)
            {
                result = PlaneIntersectionType.Front;
            }
            else if (distance < -sphere.Radius)
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
