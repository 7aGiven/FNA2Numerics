using System;
using System.Collections.Generic;
using System.Numerics;

namespace Microsoft.Xna.Framework
{
    public struct BoundingBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public bool Equals(BoundingBox other)
        {
            return other.Min == Min && other.Max == Max;
        }

        public override bool Equals(object obj)
        {
            return obj is BoundingBox other && other.Min == Min && other.Max == Max;
        }

        public Vector3[] GetCorners()
        {
            return new Vector3[8]
            {
                new Vector3(Min.X, Max.Y, Max.Z),
                Max,
                new Vector3(Max.X, Min.Y, Max.Z),
                new Vector3(Min.X, Min.Y, Max.Z),
                new Vector3(Min.X, Max.Y, Min.Z),
                new Vector3(Max.X, Max.Y, Min.Z),
                new Vector3(Max.X, Min.Y, Min.Z),
                Min
            };
        }

        public void GetCorners(Vector3[] corners)
        {
            if (corners == null)
            {
                throw new ArgumentNullException("corners");
            }
            if (corners.Length < 8)
            {
                throw new ArgumentOutOfRangeException("corners", "You have to have at least 8 elements to copy corners.");
            }
            corners[0].X = Min.X; corners[0].Y = Max.Y; corners[0].Z = Max.Z;
            corners[1] = Max;
            corners[2].X = Max.X; corners[2].Y = Min.Y; corners[2].Z = Max.Z;
            corners[3].X = Min.X; corners[3].Y = Min.Y; corners[3].Z = Max.Z;
            corners[4].X = Min.X; corners[4].Y = Max.Y; corners[4].Z = Min.Z;
            corners[5].X = Max.X; corners[5].Y = Max.Y; corners[5].Z = Min.Z;
            corners[6].X = Max.X; corners[6].Y = Min.Y; corners[6].Z = Min.Z;
            corners[7] = Min;
        }

        public override int GetHashCode()
        {
            return Min.X.GetHashCode() + Min.Y.GetHashCode() + Min.Z.GetHashCode() + Max.X.GetHashCode() + Max.Y.GetHashCode() + Max.Z.GetHashCode();
        }

        public bool Intersects(BoundingBox box)
        {
            return (
                Max.X >= box.Min.X && Min.X <= box.Max.X &&
                Max.Y >= box.Min.Y && Min.Y <= box.Max.Y &&
                Max.Z >= box.Min.Z && Min.Z <= box.Max.Z
            );
        }

        public void Intersects(ref BoundingBox box, out bool result)
        {
            result = (
                Max.X >= box.Min.X && Min.X <= box.Max.X &&
                Max.Y >= box.Min.Y && Min.Y <= box.Max.Y &&
                Max.Z >= box.Min.Z && Min.Z <= box.Max.Z
            );
        }

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

        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException();
            }
            bool flag = false;
            Vector3 minVec = new Vector3(float.MaxValue);
            Vector3 maxVec = new Vector3(float.MinValue);
            foreach (Vector3 point in points)
            {
                minVec = Vector3.Min(minVec, point);
                maxVec = Vector3.Max(maxVec, point);
                flag = true;
            }
            if (!flag)
            {
                throw new ArgumentException("You should have at least one point in points");
            }
            return new BoundingBox(minVec, maxVec);
        }

        public static BoundingBox CreateFromSphere(BoundingSphere sphere)
        {
            BoundingBox result;
            Vector3 radius = new Vector3(sphere.Radius);
            result.Min = sphere.Center - radius;
            result.Max = sphere.Center + radius;
            return result;
        }

        public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
        {
            Vector3 radius = new Vector3(sphere.Radius);
            result.Min = sphere.Center - radius;
            result.Max = sphere.Center + radius;
        }

        public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
        {
            original.Min = Vector3.Min(original.Min, additional.Min);
            original.Max = Vector3.Max(original.Max, additional.Max);
            return original;
        }

        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result.Min = Vector3.Min(original.Min, additional.Min);
            result.Max = Vector3.Max(original.Max, additional.Max);
        }
    }
}
