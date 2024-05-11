using System.Collections.Generic;
using System.Linq;
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
        
        public static string GetRelativePath(this Component component, Transform root, Transform cur, string path = "")
        {
            if (!root.GetComponentsInChildren(typeof(Component)).Contains(component))
            {
                Debug.LogError($"该组件不在{root.name}对象下");
                return null;
            }

            if (root == cur)
            {
                return path;
            }
            
            path = $"{cur.name}/{path}";
            path = component.GetRelativePath(root, cur.parent, path);
            return path;
        }
    }
}