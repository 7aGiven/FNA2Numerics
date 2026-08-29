using System.Numerics;

namespace Microsoft.Xna.Framework
{
    public struct BoundingBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public PlaneIntersectionType Intersects(Plane plane)
        {
            Vector3 positiveVertex;
            Vector3 negativeVertex;

            if (plane.Normal.X >= 0)
            {
                positiveVertex.X = Max.X;
                negativeVertex.X = Min.X;
            }
            else
            {
                positiveVertex.X = Min.X;
                negativeVertex.X = Max.X;
            }

            if (plane.Normal.Y >= 0)
            {
                positiveVertex.Y = Max.Y;
                negativeVertex.Y = Min.Y;
            }
            else
            {
                positiveVertex.Y = Min.Y;
                negativeVertex.Y = Max.Y;
            }

            if (plane.Normal.Z >= 0)
            {
                positiveVertex.Z = Max.Z;
                negativeVertex.Z = Min.Z;
            }
            else
            {
                positiveVertex.Z = Min.Z;
                negativeVertex.Z = Max.Z;
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

        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            Vector3 positiveVertex;
            Vector3 negativeVertex;

            if (plane.Normal.X >= 0)
            {
                positiveVertex.X = Max.X;
                negativeVertex.X = Min.X;
            }
            else
            {
                positiveVertex.X = Min.X;
                negativeVertex.X = Max.X;
            }

            if (plane.Normal.Y >= 0)
            {
                positiveVertex.Y = Max.Y;
                negativeVertex.Y = Min.Y;
            }
            else
            {
                positiveVertex.Y = Min.Y;
                negativeVertex.Y = Max.Y;
            }

            if (plane.Normal.Z >= 0)
            {
                positiveVertex.Z = Max.Z;
                negativeVertex.Z = Min.Z;
            }
            else
            {
                positiveVertex.Z = Min.Z;
                negativeVertex.Z = Max.Z;
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

        public bool Intersects(BoundingSphere sphere)
        {
            return !(
                Vector3.DistanceSquared(sphere.Center, Vector3.Clamp(sphere.Center, Min, Max))
                > sphere.Radius * sphere.Radius
            );
        }

        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            result = !(
                Vector3.DistanceSquared(sphere.Center, Vector3.Clamp(sphere.Center, Min, Max))
                > sphere.Radius * sphere.Radius
            );
        }
    }
}
