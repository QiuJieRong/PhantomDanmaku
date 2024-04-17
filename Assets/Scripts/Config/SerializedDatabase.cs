using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;

namespace PhantomDanmaku.Config
{
    public class SerializedDatabase<T> : SerializedScriptableObject where T : SerializedConfig
    {
        [ReadOnly]
        public string Guid = System.Guid.NewGuid().ToString();
        [ListDrawerSettings(CustomAddFunction = "CustomAddFunction",CustomRemoveIndexFunction = "CustomRemoveFunction")]
        public List<T> Values = new();

        public SerializedConfig Find(string guid)
        {
            return Values.Find(config => config.Guid == guid);
        }
        
#if UNITY_EDITOR
        public void CustomAddFunction()
        {
            var path = AssetDatabase.GetAssetPath(this);
            path = Path.GetDirectoryName(path);

            if (Directory.Exists(path + $"/{GetType().Name.Split(".").Last()}"))
            {
                path += $"/{GetType().Name.Split(".").Last()}";
            }
            
            ScriptableObjectCreator.ShowDialog<T>(path, obj =>
            {
                Values.Add(obj);
            });
        }
        public void CustomRemoveFunction(int index)
        {
            var obj = Values[index];
            Values.RemoveAt(index);
            var path = AssetDatabase.GetAssetPath(obj);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
        }
#endif
    }
}