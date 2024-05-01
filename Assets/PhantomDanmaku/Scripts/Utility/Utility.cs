using System.Collections.Generic;
using UnityEngine;

namespace PhantomDanmaku
{
    public static partial class Utility
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static int GetRandom(this Vector2Int range)
        {
            return Random.Range(range.x, range.y + 1);
        }
    }
}