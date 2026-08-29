using System.Numerics;

namespace Microsoft.Xna.Framework
{
    public struct BoundingSphere
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
    }
}
