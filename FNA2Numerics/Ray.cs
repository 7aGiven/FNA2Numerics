using Microsoft.Xna.Framework;
using System;
using System.Numerics;
using System.Text;

namespace FNA2Numerics
{
    public struct Ray
    {
        public Vector3 Position;
        public Vector3 Direction;

        public bool Equals(Ray other)
        {
            return other.Position == Position && other.Direction == Direction;
        }

        public override bool Equals(object obj)
        {
            return obj is Ray other && other.Position == Position && other.Direction == Direction;
        }

        public override int GetHashCode()
        {
            return Position.X.GetHashCode() + Position.Y.GetHashCode() + Position.Z.GetHashCode() + Direction.X.GetHashCode() + Direction.Y.GetHashCode() + Direction.Z.GetHashCode();
        }

        public float? Intersects(BoundingBox box)
        {
            float divide, t1, t2, swap;
            float tEnter = 0f;
            float tLeave = float.MaxValue;
            if (Math.Abs(this.Direction.X) < 1e-6f)
            {
                if (this.Position.X < box.Min.X || this.Position.X > box.Max.X)
                {
                    return null;
                }
            }
            else
            {
                divide = 1f / this.Direction.X;
                t1 = (box.Min.X - this.Position.X) * divide;
                t2 = (box.Max.X - this.Position.X) * divide;
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
            if (Math.Abs(this.Direction.Y) < 1e-6f)
            {
                if (this.Position.Y < box.Min.Y || this.Position.Y > box.Max.Y)
                {
                    return null;
                }
            }
            else
            {
                divide = 1f / this.Direction.Y;
                t1 = (box.Min.Y - this.Position.Y) * divide;
                t2 = (box.Max.Y - this.Position.Y) * divide;
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
            if (Math.Abs(this.Direction.Z) < 1e-6f)
            {
                if (this.Position.Z < box.Min.Z || this.Position.Z > box.Max.Z)
                {
                    return null;
                }
            }
            else
            {
                divide = 1f / this.Direction.Z;
                t1 = (box.Min.Z - this.Position.Z) * divide;
                t2 = (box.Max.Z - this.Position.Z) * divide;
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

        public void Intersects(ref BoundingBox box, out float? result)
        {
            float divide, t1, t2, swap;
            float tEnter = 0f;
            float tLeave = float.MaxValue;
            if (Math.Abs(this.Direction.X) < 1e-6f)
            {
                if (this.Position.X < box.Min.X || this.Position.X > box.Max.X)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                divide = 1f / this.Direction.X;
                t1 = (box.Min.X - this.Position.X) * divide;
                t2 = (box.Max.X - this.Position.X) * divide;
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
            if (Math.Abs(this.Direction.Y) < 1e-6f)
            {
                if (this.Position.Y < box.Min.Y || this.Position.Y > box.Max.Y)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                divide = 1f / this.Direction.Y;
                t1 = (box.Min.Y - this.Position.Y) * divide;
                t2 = (box.Max.Y - this.Position.Y) * divide;
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
            if (Math.Abs(this.Direction.Z) < 1e-6f)
            {
                if (this.Position.Z < box.Min.Z || this.Position.Z > box.Max.Z)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                divide = 1f / this.Direction.Z;
                t1 = (box.Min.Z - this.Position.Z) * divide;
                t2 = (box.Max.Z - this.Position.Z) * divide;
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

        public float? Intersects(BoundingFrustum frustum)
        {
            if (ReferenceEquals(frustum, null))
            {
                throw new ArgumentNullException("frustum");
            }
            float? result;
            frustum.Intersects(ref this, out result);
            return result;
        }

        public float? Intersects(BoundingSphere sphere)
        {
            Vector3 difference = sphere.Center - Position;
            float differenceLengthSquared = difference.LengthSquared();
            float sphereRadiusSquared = sphere.Radius * sphere.Radius;
            if (differenceLengthSquared <= sphereRadiusSquared)
            {
                return 0f;
            }
            float distanceAlongRay = Vector3.Dot(difference, Direction);
            if (distanceAlongRay < 0f)
            {
                return null;
            }
            float dist = sphereRadiusSquared - differenceLengthSquared + distanceAlongRay * distanceAlongRay;
            if (dist < 0)
            {
                return null;
            }
            return distanceAlongRay - (float)Math.Sqrt(dist);
        }

        public void Intersects(ref BoundingSphere sphere, out float? result)
        {
            Vector3 difference = sphere.Center - Position;
            float differenceLengthSquared = difference.LengthSquared();
            float sphereRadiusSquared = sphere.Radius * sphere.Radius;
            if (differenceLengthSquared <= sphereRadiusSquared)
            {
                result = 0f;
                return;
            }
            float distanceAlongRay = Vector3.Dot(difference, Direction);
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
            result = distanceAlongRay - (float)Math.Sqrt(dist);
        }

        public float? Intersects(Plane plane)
        {
            float den = Vector3.Dot(plane.Normal, Direction);
            if (Math.Abs(den) < 1e-5f)
            {
                return null;
            }
            float distance = -Plane.DotCoordinate(plane, Position) / den;
            if (distance < 0f)
            {
                if (distance < -1e-5f)
                {
                    return null;
                }
                return 0f;
            }
            return distance;
        }

        public void Intersects(ref Plane plane, out float? result)
        {
            float den = Vector3.Dot(plane.Normal, Direction);
            if (Math.Abs(den) < 1e-5f)
            {
                result = null;
                return;
            }
            float distance = -Plane.DotCoordinate(plane, Position) / den;
            if (distance < 0f)
            {
                if (distance < -1e-5f)
                {
                    result = null;
                    return;
                }
                result = 0f;
                return;
            }
            result = distance;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{Position:", 24 + 6 * 17);
            sb.Append("{X:");
            sb.Append(Position.X);
            sb.Append(" Y:");
            sb.Append(Position.Y);
            sb.Append(" Z:");
            sb.Append(Position.Z);
            sb.Append("} Direction:");
            sb.Append("{X:");
            sb.Append(Direction.X);
            sb.Append(" Y:");
            sb.Append(Direction.Y);
            sb.Append(" Z:");
            sb.Append(Direction.Z);
            sb.Append("}}");
            return sb.ToString();
        }

        public static bool operator ==(Ray a, Ray b)
        {
            return a.Position == b.Position && a.Direction == b.Direction;
        }

        public static bool operator !=(Ray a, Ray b)
        {
            return a.Position != b.Position || a.Direction != b.Direction;
        }
    }
}
