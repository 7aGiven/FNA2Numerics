using FNA2Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Microsoft.Xna.Framework
{
    // https://learn.microsoft.com/en-us/previous-versions/windows/xna/bb195161(v=xnagamestudio.40)
    public struct BoundingBox : IEquatable<BoundingBox>
    {
        public const int CornerCount = 8;

        public Vector3 Min;
        public Vector3 Max;

        #region Public Methods

        public ContainmentType Contains(BoundingBox box)
        {
            if (
                Max.X < box.Min.X || Min.X > box.Max.X ||
                Max.Y < box.Min.Y || Min.Y > box.Max.Y ||
                Max.Z < box.Min.Z || Min.Z > box.Max.Z
            )
            {
                return ContainmentType.Disjoint;
            }
            if (
                Min.X <= box.Min.X && box.Max.X <= Max.X &&
                Min.Y <= box.Min.Y && box.Max.Y <= Max.Y &&
                Min.Z <= box.Min.Z && box.Max.Z <= Max.Z
            )
            {
                return ContainmentType.Contains;
            }
            return ContainmentType.Intersects;
        }

        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            if (
                Max.X < box.Min.X || Min.X > box.Max.X ||
                Max.Y < box.Min.Y || Min.Y > box.Max.Y ||
                Max.Z < box.Min.Z || Min.Z > box.Max.Z
            )
            {
                result = ContainmentType.Disjoint;
                return;
            }
            if (
                Min.X <= box.Min.X && box.Max.X <= Max.X &&
                Min.Y <= box.Min.Y && box.Max.Y <= Max.Y &&
                Min.Z <= box.Min.Z && box.Max.Z <= Max.Z
            )
            {
                result = ContainmentType.Contains;
                return;
            }
            result = ContainmentType.Intersects;
        }

        public ContainmentType Contains(Vector3 point)
        {
            return (
                Min.X <= point.X && point.X <= Max.X &&
                Min.Y <= point.Y && point.Y <= Max.Y &&
                Min.Z <= point.Z && point.Z <= Max.Z
            ) ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            result = (
                Min.X <= point.X && point.X <= Max.X &&
                Min.Y <= point.Y && point.Y <= Max.Y &&
                Min.Z <= point.Z && point.Z <= Max.Z
            ) ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        public Vector3[] GetCorners()
        {
            return new Vector3[CornerCount]
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
            if (corners.Length < CornerCount)
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

        public bool Intersects(BoundingBox box)
        {
            return !(
                this.Max.X < box.Min.X || this.Min.X > box.Max.X ||
                this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y ||
                this.Max.Z < box.Min.Z || this.Min.Z > box.Max.Z
            );
        }

        public void Intersects(ref BoundingBox box, out bool result)
        {
            result = !(
                this.Max.X < box.Min.X || this.Min.X > box.Max.X ||
                this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y ||
                this.Max.Z < box.Min.Z || this.Min.Z > box.Max.Z
            );
        }

        public bool Intersects(BoundingSphere sphere)
        {
            float radius = sphere.Radius;
            return !(
                Vector3.DistanceSquared(sphere.Center, Vector3.Clamp(sphere.Center, Min, Max)) > radius * radius
            );
        }

        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            float radius = sphere.Radius;
            result = !(
                Vector3.DistanceSquared(sphere.Center, Vector3.Clamp(sphere.Center, Min, Max)) > radius * radius
            );
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            float radius = Vector3.Dot(Vector3.Abs(plane.Normal), (Max - Min) * 0.5f);
            float distance = Plane.DotCoordinate(plane, (Min + Max) * 0.5f);
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
            float radius = Vector3.Dot(Vector3.Abs(plane.Normal), (Max - Min) * 0.5f);
            float distance = Plane.DotCoordinate(plane, (Min + Max) * 0.5f);
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

        public float? Intersects(Ray ray)
        {
            float divide, t1, t2, swap;
            float tEnter = 0f;
            float tLeave = float.MaxValue;
            if (Math.Abs(ray.Direction.X) < 1e-6f)
            {
                if (ray.Position.X < this.Min.X || ray.Position.X > this.Max.X)
                {
                    return null;
                }
            }
            else
            {
                divide = 1f / ray.Direction.X;
                t1 = (this.Min.X - ray.Position.X) * divide;
                t2 = (this.Max.X - ray.Position.X) * divide;
                if (t1 > t2)
                {
                    swap = t1;
                    t1 = t2;
                    t2 = swap;
                }
                tEnter = Math.Max(t1, tEnter);
                tLeave = Math.Min(t2, tLeave);
                if (tEnter > tLeave)
                {
                    return null;
                }
            }
            if (Math.Abs(ray.Direction.Y) < 1e-6f)
            {
                if (ray.Position.Y < this.Min.Y || ray.Position.Y > this.Max.Y)
                {
                    return null;
                }
            }
            else
            {
                divide = 1f / ray.Direction.Y;
                t1 = (this.Min.Y - ray.Position.Y) * divide;
                t2 = (this.Max.Y - ray.Position.Y) * divide;
                if (t1 > t2)
                {
                    swap = t1;
                    t1 = t2;
                    t2 = swap;
                }
                tEnter = Math.Max(t1, tEnter);
                tLeave = Math.Min(t2, tLeave);
                if (tEnter > tLeave)
                {
                    return null;
                }
            }
            if (Math.Abs(ray.Direction.Z) < 1e-6f)
            {
                if (ray.Position.Z < this.Min.Z || ray.Position.Z > this.Max.Z)
                {
                    return null;
                }
            }
            else
            {
                divide = 1f / ray.Direction.Z;
                t1 = (this.Min.Z - ray.Position.Z) * divide;
                t2 = (this.Max.Z - ray.Position.Z) * divide;
                if (t1 > t2)
                {
                    swap = t1;
                    t1 = t2;
                    t2 = swap;
                }
                tEnter = Math.Max(t1, tEnter);
                tLeave = Math.Min(t2, tLeave);
                if (tEnter > tLeave)
                {
                    return null;
                }
            }
            return tEnter;
        }

        public void Intersects(ref Ray ray, out float? result)
        {
            float divide, t1, t2, swap;
            float tEnter = 0f;
            float tLeave = float.MaxValue;
            if (Math.Abs(ray.Direction.X) < 1e-6f)
            {
                if (ray.Position.X < this.Min.X || ray.Position.X > this.Max.X)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                divide = 1f / ray.Direction.X;
                t1 = (this.Min.X - ray.Position.X) * divide;
                t2 = (this.Max.X - ray.Position.X) * divide;
                if (t1 > t2)
                {
                    swap = t1;
                    t1 = t2;
                    t2 = swap;
                }
                tEnter = Math.Max(t1, tEnter);
                tLeave = Math.Min(t2, tLeave);
                if (tEnter > tLeave)
                {
                    result = null;
                    return;
                }
            }
            if (Math.Abs(ray.Direction.Y) < 1e-6f)
            {
                if (ray.Position.Y < this.Min.Y || ray.Position.Y > this.Max.Y)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                divide = 1f / ray.Direction.Y;
                t1 = (this.Min.Y - ray.Position.Y) * divide;
                t2 = (this.Max.Y - ray.Position.Y) * divide;
                if (t1 > t2)
                {
                    swap = t1;
                    t1 = t2;
                    t2 = swap;
                }
                tEnter = Math.Max(t1, tEnter);
                tLeave = Math.Min(t2, tLeave);
                if (tEnter > tLeave)
                {
                    result = null;
                    return;
                }
            }
            if (Math.Abs(ray.Direction.Z) < 1e-6f)
            {
                if (ray.Position.Z < this.Min.Z || ray.Position.Z > this.Max.Z)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                divide = 1f / ray.Direction.Z;
                t1 = (this.Min.Z - ray.Position.Z) * divide;
                t2 = (this.Max.Z - ray.Position.Z) * divide;
                if (t1 > t2)
                {
                    swap = t1;
                    t1 = t2;
                    t2 = swap;
                }
                tEnter = Math.Max(t1, tEnter);
                tLeave = Math.Min(t2, tLeave);
                if (tEnter > tLeave)
                {
                    result = null;
                    return;
                }
            }
            result = tEnter;
        }

        #endregion

        #region Public Static Methods

        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException();
            }
            IEnumerator<Vector3> enumerator = points.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                throw new ArgumentException("You should have at least one point in points");
            }
            BoundingBox result;
            result.Min = enumerator.Current;
            result.Max = result.Min;
            while (enumerator.MoveNext())
            {
                Vector3 point = enumerator.Current;
                result.Min = Vector3.Min(result.Min, point);
                result.Max = Vector3.Max(result.Max, point);
            }
            return result;
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

        #endregion

        #region GetHashCode ToString Equals

        public override int GetHashCode()
        {
            return Min.X.GetHashCode() + Min.Y.GetHashCode() + Min.Z.GetHashCode() + Max.X.GetHashCode() + Max.Y.GetHashCode() + Max.Z.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{Min:", 11 + 2 * 52);
            sb.Append("{X:");
            sb.Append(Min.X);
            sb.Append(" Y:");
            sb.Append(Min.Y);
            sb.Append(" Z:");
            sb.Append(Min.Z);
            sb.Append("} Max:{X:");
            sb.Append(Max.X);
            sb.Append(" Y:");
            sb.Append(Max.Y);
            sb.Append(" Z:");
            sb.Append(Max.Z);
            sb.Append("}}");
            return sb.ToString();
        }

        public bool Equals(BoundingBox other)
        {
            return other.Min == Min && other.Max == Max;
        }

        public override bool Equals(object obj)
        {
            return obj is BoundingBox other && other.Min == Min && other.Max == Max;
        }

        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Min == b.Min && a.Max == b.Max;
        }

        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return a.Min != b.Min || a.Max != b.Max;
        }

        #endregion
    }
}