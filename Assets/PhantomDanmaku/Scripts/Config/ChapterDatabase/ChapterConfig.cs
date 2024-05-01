using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;

namespace PhantomDanmaku.Config
{
    public class ChapterConfig : SerializedConfig
    {
        [LabelText("章节ID")]
        public int Id;

        [LabelText("章节名字")]
        public string Name;

        [LabelText("章节描述")]
        public string Desc;

        [LabelText("关卡列表")]
        [ListDrawerSettings(CustomAddFunction = "CustomAddFunction",CustomRemoveIndexFunction = "CustomRemoveFunction")]
        public List<LevelConfig> LevelConfigs;
#if UNITY_EDITOR
        public void CustomRemoveFunction(int index)
        {
            var obj = LevelConfigs[index];
            LevelConfigs.RemoveAt(index);
            var path = AssetDatabase.GetAssetPath(obj);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
        }
        public void CustomAddFunction()
        {
            var path = AssetDatabase.GetAssetPath(this);
            path = Path.GetDirectoryName(path);

            if (Directory.Exists(path + $"/{GetType().Name.Split(".").Last()}"))
            {
                path += $"/{GetType().Name.Split(".").Last()}";
            }
            
            ScriptableObjectCreator.ShowDialog<LevelConfig>(path, obj =>
            {
                LevelConfigs.Add(obj);
            });
        }
#endif
    }

    public enum ColorEnum
    {
        red,green
    }
}