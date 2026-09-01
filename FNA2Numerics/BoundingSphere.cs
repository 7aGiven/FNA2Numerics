using System;
using System.Numerics;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public struct BoundingSphere : IEquatable<BoundingSphere>
    {
        public Vector3 Center;
        public float Radius;

        public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
        {
            BoundingSphere result;
            result.Center = (box.Min + box.Max) * 0.5f;
            result.Radius = Vector3.Distance(result.Center, box.Max);
            return result;
        }

        public static void CreateFromBoundingBox(ref BoundingBox box, out BoundingSphere result)
        {
            result.Center = (box.Min + box.Max) * 0.5f;
            result.Radius = Vector3.Distance(result.Center, box.Max);
        }

        public bool Intersects(BoundingSphere sphere)
        {
            float combineRadius = Radius + sphere.Radius;
            return combineRadius * combineRadius > Vector3.DistanceSquared(Center, sphere.Center);
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            float radius = this.Radius;
            float distance = Plane.DotCoordinate(plane, this.Center);
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

        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            float radius = this.Radius;
            float distance = Plane.DotCoordinate(plane, this.Center);
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

        public bool Equals(BoundingSphere other)
        {
            return other.Center == Center && other.Radius == Radius;
        }

        public override bool Equals(object obj)
        {
            return obj is BoundingSphere other && other.Center == Center && other.Radius == Radius;
        }

        public override int GetHashCode()
        {
            return Center.X.GetHashCode() + Center.Y.GetHashCode() + Center.Z.GetHashCode() + Radius.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{Center:", 8 + 1 + 3 * 17 + 8 + 14 + 1);
            sb.Append("{X:");
            sb.Append(Center.X);
            sb.Append(" Y:");
            sb.Append(Center.Y);
            sb.Append(" Z:");
            sb.Append(Center.Z);
            sb.Append("} Radius:");
            sb.Append(Radius);
            sb.Append('}');
            return sb.ToString();
        }

        public static bool operator ==(BoundingSphere a, BoundingSphere b)
        {
            return a.Center == b.Center && a.Radius == b.Radius;
        }

        public static bool operator !=(BoundingSphere a, BoundingSphere b)
        {
            return a.Center != b.Center || a.Radius != b.Radius;
        }
    }
}