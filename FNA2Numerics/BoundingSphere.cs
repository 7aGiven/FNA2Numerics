using FNA2Numerics;
using System;
using System.Numerics;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public struct BoundingSphere : IEquatable<BoundingSphere>
    {
        public Vector3 Center;
        public float Radius;

        public ContainmentType Contains(BoundingBox box)
        {
            float radiusSquare = Radius * Radius;
            if (Vector3.DistanceSquared(Center, Vector3.Clamp(Center, box.Min, box.Max)) > radiusSquare)
            {
                return ContainmentType.Disjoint;
            }
            return Vector3.Max(
                Vector3.Abs(Center - box.Min),
                Vector3.Abs(Center - box.Max)
            ).LengthSquared() > radiusSquare ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            float radiusSquare = Radius * Radius;
            if (Vector3.DistanceSquared(Center, Vector3.Clamp(Center, box.Min, box.Max)) > radiusSquare)
            {
                result = ContainmentType.Disjoint;
                return;
            }
            result = Vector3.Max(
                Vector3.Abs(Center - box.Min),
                Vector3.Abs(Center - box.Max)
            ).LengthSquared() > radiusSquare ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            float distance = Vector3.Distance(Center, sphere.Center);
            if (Radius + sphere.Radius >= distance)
            {
                return Radius - sphere.Radius >= distance ? ContainmentType.Contains : ContainmentType.Intersects;
            }
            return ContainmentType.Disjoint;
        }

        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            float distance = Vector3.Distance(Center, sphere.Center);
            if (Radius + sphere.Radius >= distance)
            {
                result = Radius - sphere.Radius >= distance ? ContainmentType.Contains : ContainmentType.Intersects;
                return;
            }
            result = ContainmentType.Disjoint;
        }

        public ContainmentType Contains(Vector3 point)
        {
            return Vector3.DistanceSquared(point, Center) < Radius * Radius
                ? ContainmentType.Contains : ContainmentType.Disjoint;
        }
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            result = Vector3.DistanceSquared(point, Center) < Radius * Radius
                ? ContainmentType.Contains : ContainmentType.Disjoint;
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

        public float? Intersects(Ray ray)
        {
            Vector3 difference = this.Center - ray.Position;
            float differenceLengthSquared = difference.LengthSquared();
            float sphereRadiusSquared = this.Radius * this.Radius;
            if (differenceLengthSquared <= sphereRadiusSquared)
            {
                return 0f;
            }
            float distanceAlongRay = Vector3.Dot(difference, ray.Direction);
            if (distanceAlongRay < 0f)
            {
                return null;
            }
            float dist = sphereRadiusSquared - differenceLengthSquared + distanceAlongRay * distanceAlongRay;
            if (dist < 0)
            {
                return null;
            }
            return distanceAlongRay - (float) Math.Sqrt(dist);
        }

        public void Intersects(ref Ray ray, out float? result)
        {
            Vector3 difference = this.Center - ray.Position;
            float differenceLengthSquared = difference.LengthSquared();
            float sphereRadiusSquared = this.Radius * this.Radius;
            if (differenceLengthSquared <= sphereRadiusSquared)
            {
                result = 0f;
                return;
            }
            float distanceAlongRay = Vector3.Dot(difference, ray.Direction);
            if (distanceAlongRay < 0f)
            {
                result = null;
                return;
            }
            float dist = sphereRadiusSquared - differenceLengthSquared + distanceAlongRay * distanceAlongRay;
            if (dist < 0)
            {
                result = null;
                return;
            }
            result = distanceAlongRay - (float) Math.Sqrt(dist);
        }

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

        public static BoundingSphere CreateMerged(BoundingSphere original, BoundingSphere additional)
        {
            Vector3 diff = Vector3.Subtract(additional.Center, original.Center);
            float distance = diff.Length();
            float radius1 = original.Radius;
            float radius2 = additional.Radius;
            if (radius1 + radius2 >= distance)
            {
                if (radius1 - radius2 >= distance)
                {
                    return original;
                }
                if (radius2 - radius1 >= distance)
                {
                    return additional;
                }
            }
            BoundingSphere result;
            float leftBound = Math.Min(-radius1, distance - radius2);
            result.Radius = (Math.Max(radius1, distance + radius2) - leftBound) * 0.5f;
            result.Center = original.Center + (diff * (1f / distance)) * (result.Radius + leftBound);
            return result;
        }

        #region GetHashCode ToString Equals

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

        public bool Equals(BoundingSphere other)
        {
            return other.Center == Center && other.Radius == Radius;
        }

        public override bool Equals(object obj)
        {
            return obj is BoundingSphere other && other.Center == Center && other.Radius == Radius;
        }

        public static bool operator ==(BoundingSphere a, BoundingSphere b)
        {
            return a.Center == b.Center && a.Radius == b.Radius;
        }

        public static bool operator !=(BoundingSphere a, BoundingSphere b)
        {
            return a.Center != b.Center || a.Radius != b.Radius;
        }

        #endregion
    }
}