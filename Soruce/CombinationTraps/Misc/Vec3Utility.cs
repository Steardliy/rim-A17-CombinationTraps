using UnityEngine;
using Verse;

namespace CombinationTraps
{
    public static class Vec3Utility
    {
        public static Vector3 Round(this Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
        public static IntVec3 AsIntVec3(this float angle)
        {
            return Vector3.forward.RotatedBy(angle).Round().ToIntVec3();
        }
    }
}
