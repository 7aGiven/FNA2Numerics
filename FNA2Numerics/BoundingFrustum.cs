using FNA2Numerics;
using System;
using System.Numerics;

namespace Microsoft.Xna.Framework
{
    // https://learn.microsoft.com/en-us/previous-versions/windows/xna/bb195165(v=xnagamestudio.40)
    public class BoundingFrustum : IEquatable<BoundingFrustum>
    {
        public const int CornerCount = 8;

        internal readonly Vector3[] cornerArray = new Vector3[CornerCount];

        private readonly Plane[] planes = new Plane[6];
        private Matrix4x4 matrix;

        public ContainmentType Contains(BoundingBox box)
        {
            bool flag = false;
            Vector3 center = (box.Min + box.Max) * 0.5f;
            Vector3 extent = (box.Max - box.Min) * 0.5f;
            for (int i = 0; i < 6; i++)
            {
                // inline BoundingBox.Intersects(Plane)
                Plane plane = planes[i];
                float radius = Vector3.Dot(Vector3.Abs(plane.Normal), extent);
                float distance = Plane.DotCoordinate(plane, center);
                if (distance > radius)
                {
                    return ContainmentType.Disjoint;
                }
                if (!(distance < -radius))
                {
                    flag = true;
                }
            }
            return flag ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            bool flag = false;
            Vector3 center = (box.Min + box.Max) * 0.5f;
            Vector3 extent = (box.Max - box.Min) * 0.5f;
            for (int i = 0; i < 6; i++)
            {
                // inline BoundingBox.Intersects(Plane)
                Plane plane = planes[i];
                float radius = Vector3.Dot(Vector3.Abs(plane.Normal), extent);
                float distance = Plane.DotCoordinate(plane, center);
                if (distance > radius)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
                if (!(distance < -radius))
                {
                    flag = true;
                }
            }
            result = flag ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            bool flag = false;
            for (int i = 0; i < 6; i++)
            {
                // inline BoundingSphere.Intersects(Plane)
                float distance = Plane.DotCoordinate(planes[i], sphere.Center);
                if (distance > sphere.Radius)
                {
                    return ContainmentType.Disjoint;
                }
                if (!(distance < -sphere.Radius))
                {
                    flag = true;
                }
            }
            return flag ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            bool flag = false;
            for (int i = 0; i < 6; i++)
            {
                // inline BoundingSphere.Intersects(Plane)
                float distance = Plane.DotCoordinate(planes[i], sphere.Center);
                if (distance > sphere.Radius)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
                if (!(distance < -sphere.Radius))
                {
                    flag = true;
                }
            }
            result = flag ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        public ContainmentType Contains(Vector3 point)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Plane.DotCoordinate(planes[i], point) > 1e-5f)
                {
                    return ContainmentType.Disjoint;
                }
            }
            return ContainmentType.Contains;
        }

        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Plane.DotCoordinate(planes[i], point) > 1e-5f)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
            }
            result = ContainmentType.Contains;
        }

        public Vector3[] GetCorners()
        {
            return (Vector3[]) cornerArray.Clone();
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
            cornerArray.CopyTo(corners, 0);
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            bool flag = Plane.DotCoordinate(plane, cornerArray[0]) > 0f;
            for (int i = 1; i < CornerCount; i++)
            {
                if (Plane.DotCoordinate(plane, cornerArray[i]) > 0f != flag)
                {
                    return PlaneIntersectionType.Intersecting;
                }
            }
            return flag ? PlaneIntersectionType.Front : PlaneIntersectionType.Back;
        }

        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            bool flag = Plane.DotCoordinate(plane, cornerArray[0]) > 0f;
            for (int i = 1; i < CornerCount; i++)
            {
                if (Plane.DotCoordinate(plane, cornerArray[i]) > 0f != flag)
                {
                    result = PlaneIntersectionType.Intersecting;
                    return;
                }
            }
            result = flag ? PlaneIntersectionType.Front : PlaneIntersectionType.Back;
        }

        public float? Intersects(Ray ray)
        {
            float tEnter = float.MinValue;
            float tLeave = float.MaxValue;
            for (int i = 0; i < 6; i++)
            {
                float velocity = Plane.DotNormal(planes[i], ray.Direction);
                float distance = Plane.DotCoordinate(planes[i], ray.Position);
                if (Math.Abs(velocity) < 1e-5f)
                {
                    if (distance > 0f)
                    {
                        return null;
                    }
                    continue;
                }
                float t = -distance / velocity;
                if (velocity < 0f)
                {
                    if (t > tLeave)
                    {
                        return null;
                    }
                    if (t > tEnter)
                    {
                        tEnter = t;
                    }
                }
                else
                {
                    if (t < tEnter)
                    {
                        return null;
                    }
                    if (t < tLeave)
                    {
                        tLeave = t;
                    }
                }
            }
            if (tEnter >= 0f)
            {
                return tEnter;
            }
            if (tLeave >= 0f)
            {
                return tLeave;
            }
            return null;
        }

        public void Intersects(ref Ray ray, out float? result)
        {
            float tEnter = float.MinValue;
            float tLeave = float.MaxValue;
            result = null;
            for (int i = 0; i < 6; i++)
            {
                float velocity = Plane.DotNormal(planes[i], ray.Direction);
                float distance = Plane.DotCoordinate(planes[i], ray.Position);
                if (Math.Abs(velocity) < 1e-5f)
                {
                    if (distance > 0f)
                    {
                        return;
                    }
                    continue;
                }
                float t = -distance / velocity;
                if (velocity < 0f)
                {
                    if (t > tLeave)
                    {
                        return;
                    }
                    if (t > tEnter)
                    {
                        tEnter = t;
                    }
                }
                else
                {
                    if (t < tEnter)
                    {
                        return;
                    }
                    if (t < tLeave)
                    {
                        tLeave = t;
                    }
                }
            }
            if (tEnter >= 0f)
            {
                result = tEnter;
                return;
            }
            if (tLeave >= 0f)
            {
                result = tLeave;
                return;
            }
        }

        #region GetHashCode ToString Equals
        public override int GetHashCode()
        {
            return (
                matrix.M11.GetHashCode() + matrix.M12.GetHashCode() + matrix.M13.GetHashCode() + matrix.M14.GetHashCode() +
                matrix.M21.GetHashCode() + matrix.M22.GetHashCode() + matrix.M23.GetHashCode() + matrix.M24.GetHashCode() +
                matrix.M31.GetHashCode() + matrix.M32.GetHashCode() + matrix.M33.GetHashCode() + matrix.M34.GetHashCode() +
                matrix.M41.GetHashCode() + matrix.M42.GetHashCode() + matrix.M43.GetHashCode() + matrix.M44.GetHashCode()
            );
        }

        public bool Equals(BoundingFrustum other)
        {
            return !ReferenceEquals(other, null) && other.matrix == matrix;
        }

        public override bool Equals(object obj)
        {
            BoundingFrustum other = obj as BoundingFrustum;
            return !ReferenceEquals(other, null) && other.matrix == matrix;
        }

        public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            return a.matrix == b.matrix;
        }

        public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
        {
            if (ReferenceEquals(a, b))
            {
                return false;
            }
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return true;
            }
            return a.matrix != b.matrix;
        }

        #endregion
    }
}