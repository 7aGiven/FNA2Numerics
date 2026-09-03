using System;
using System.Numerics;
using System.Text;

namespace Microsoft.Xna.Framework
{
    // https://learn.microsoft.com/en-us/previous-versions/windows/xna/bb195165(v=xnagamestudio.40)
    public class BoundingFrustum : IEquatable<BoundingFrustum>
    {
        public const int CornerCount = 8;

        public Matrix4x4 Matrix
        {
            get
            {
                return matrix;
            }
            set
            {
                matrix = value;

                Vector4 col1 = new Vector4(value.M11, value.M21, value.M31, value.M41);
                Vector4 col2 = new Vector4(value.M12, value.M22, value.M32, value.M42);
                Vector4 col3 = new Vector4(value.M13, value.M23, value.M33, value.M43);
                Vector4 col4 = new Vector4(value.M14, value.M24, value.M34, value.M44);

                planes[0] = Plane.Normalize(new Plane(col3));
                planes[1] = Plane.Normalize(new Plane(col3 - col4));
                planes[2] = Plane.Normalize(new Plane(-col1 - col4));
                planes[3] = Plane.Normalize(new Plane(col1 - col4));
                planes[4] = Plane.Normalize(new Plane(col2 - col4));
                planes[5] = Plane.Normalize(new Plane(-col2 - col4));


                Ray ray = ComputeIntersectionLine(ref planes[0], ref planes[2]);
                corners[0] = ComputeIntersection(ref planes[4], ref ray);
                corners[3] = ComputeIntersection(ref planes[5], ref ray);
                ray = ComputeIntersectionLine(ref planes[3], ref planes[0]);
                corners[1] = ComputeIntersection(ref planes[4], ref ray);
                corners[2] = ComputeIntersection(ref planes[5], ref ray);
                ray = ComputeIntersectionLine(ref planes[2], ref planes[1]);
                corners[4] = ComputeIntersection(ref planes[4], ref ray);
                corners[7] = ComputeIntersection(ref planes[5], ref ray);
                ray = ComputeIntersectionLine(ref planes[1], ref planes[3]);
                corners[5] = ComputeIntersection(ref planes[4], ref ray);
                corners[6] = ComputeIntersection(ref planes[5], ref ray);
            }
        }

        internal readonly Vector3[] corners = new Vector3[CornerCount];

        private readonly Plane[] planes = new Plane[6];
        private Matrix4x4 matrix;

        public BoundingFrustum(Matrix4x4 value)
        {
            matrix = value;

            Vector4 col1 = new Vector4(value.M11, value.M21, value.M31, value.M41);
            Vector4 col2 = new Vector4(value.M12, value.M22, value.M32, value.M42);
            Vector4 col3 = new Vector4(value.M13, value.M23, value.M33, value.M43);
            Vector4 col4 = new Vector4(value.M14, value.M24, value.M34, value.M44);

            planes[0] = Plane.Normalize(new Plane(col3));
            planes[1] = Plane.Normalize(new Plane(col3 - col4));
            planes[2] = Plane.Normalize(new Plane(-col1 - col4));
            planes[3] = Plane.Normalize(new Plane(col1 - col4));
            planes[4] = Plane.Normalize(new Plane(col2 - col4));
            planes[5] = Plane.Normalize(new Plane(-col2 - col4));


            Ray ray = ComputeIntersectionLine(ref planes[0], ref planes[2]);
            corners[0] = ComputeIntersection(ref planes[4], ref ray);
            corners[3] = ComputeIntersection(ref planes[5], ref ray);
            ray = ComputeIntersectionLine(ref planes[3], ref planes[0]);
            corners[1] = ComputeIntersection(ref planes[4], ref ray);
            corners[2] = ComputeIntersection(ref planes[5], ref ray);
            ray = ComputeIntersectionLine(ref planes[2], ref planes[1]);
            corners[4] = ComputeIntersection(ref planes[4], ref ray);
            corners[7] = ComputeIntersection(ref planes[5], ref ray);
            ray = ComputeIntersectionLine(ref planes[1], ref planes[3]);
            corners[5] = ComputeIntersection(ref planes[4], ref ray);
            corners[6] = ComputeIntersection(ref planes[5], ref ray);
        }

        #region Public Methods

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
            bool intersects = false;
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
                    intersects = true;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            bool intersects = false;
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
                    intersects = true;
                }
            }
            return intersects ? ContainmentType.Intersects : ContainmentType.Contains;
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
            return (Vector3[]) corners.Clone();
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
            this.corners.CopyTo(corners, 0);
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            bool flag = Plane.DotCoordinate(plane, corners[0]) > 0f;
            for (int i = 1; i < CornerCount; i++)
            {
                if (Plane.DotCoordinate(plane, corners[i]) > 0f != flag)
                {
                    return PlaneIntersectionType.Intersecting;
                }
            }
            return flag ? PlaneIntersectionType.Front : PlaneIntersectionType.Back;
        }

        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            bool flag = Plane.DotCoordinate(plane, corners[0]) > 0f;
            for (int i = 1; i < CornerCount; i++)
            {
                if (Plane.DotCoordinate(plane, corners[i]) > 0f != flag)
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

        #endregion

        private static Ray ComputeIntersectionLine(ref Plane p1, ref Plane p2)
        {
            Ray result;
            result.Direction = Vector3.Cross(p1.Normal, p2.Normal);
            result.Position = Vector3.Cross(p2.D * p1.Normal - p1.D * p2.Normal, result.Direction) / result.Direction.LengthSquared();
            return result;
        }

        private static Vector3 ComputeIntersection(ref Plane plane, ref Ray ray)
        {
            return ray.Position + ray.Direction * (-Plane.DotCoordinate(plane, ray.Position) / Vector3.Dot(plane.Normal, ray.Direction));
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(78 * 6 + 38);
            sb.Append("{Near:");
            sb.Append("{Normal:{X:");
            sb.Append(planes[0].Normal.X);
            sb.Append(" Y:");
            sb.Append(planes[0].Normal.Y);
            sb.Append(" Z:");
            sb.Append(planes[0].Normal.Z);
            sb.Append("} D:");
            sb.Append(planes[0].D);
            sb.Append("} Far:");
            sb.Append("{Normal:{X:");
            sb.Append(planes[1].Normal.X);
            sb.Append(" Y:");
            sb.Append(planes[1].Normal.Y);
            sb.Append(" Z:");
            sb.Append(planes[1].Normal.Z);
            sb.Append("} D:");
            sb.Append(planes[1].D);
            sb.Append("} Left:");
            sb.Append("{Normal:{X:");
            sb.Append(planes[2].Normal.X);
            sb.Append(" Y:");
            sb.Append(planes[2].Normal.Y);
            sb.Append(" Z:");
            sb.Append(planes[2].Normal.Z);
            sb.Append("} D:");
            sb.Append(planes[2].D);
            sb.Append("} Right:");
            sb.Append("{Normal:{X:");
            sb.Append(planes[3].Normal.X);
            sb.Append(" Y:");
            sb.Append(planes[3].Normal.Y);
            sb.Append(" Z:");
            sb.Append(planes[3].Normal.Z);
            sb.Append("} D:");
            sb.Append(planes[3].D);
            sb.Append("} Top:");
            sb.Append("{Normal:{X:");
            sb.Append(planes[4].Normal.X);
            sb.Append(" Y:");
            sb.Append(planes[4].Normal.Y);
            sb.Append(" Z:");
            sb.Append(planes[4].Normal.Z);
            sb.Append("} D:");
            sb.Append(planes[4].D);
            sb.Append("} Bottom:");
            sb.Append("{Normal:{X:");
            sb.Append(planes[5].Normal.X);
            sb.Append(" Y:");
            sb.Append(planes[5].Normal.Y);
            sb.Append(" Z:");
            sb.Append(planes[5].Normal.Z);
            sb.Append("} D:");
            sb.Append(planes[5].D);
            sb.Append("}}");
            return sb.ToString();
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