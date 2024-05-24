using System;
using System.Collections.Generic;
using System.Linq;
using PhantomDanmaku.Runtime;
using PhantomDanmaku.Runtime.UI;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public static UIGroupBase GetUIGroup<T>(this GameObject gameObject) where T : UIGroupBase
        {
            return Components.UI.GetUIGroup<T>(gameObject);
        }

        public static UIGroupBase AddUIGroup<T>(this GameObject gameObject) where T : UIGroupBase
        {
            var uiGroup = Activator.CreateInstance<T>();
            Components.UI.RegisterUIGroup(uiGroup, gameObject);
            return uiGroup;
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}