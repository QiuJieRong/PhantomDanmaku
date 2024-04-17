using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace PhantomDanmaku.Editor
{
    public class DatabaseWindow : OdinMenuEditorWindow
    {
        [MenuItem("配置/配置表")]
        private static void OpenWindow()
        {
            GetWindow<DatabaseWindow>().Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true);
            
            CreateAssetsMenu(tree, "配置", "Assets/PhantomDanmaku/Config");
            return tree;
        }

        private void CreateAssetsMenu(OdinMenuTree tree, string root,string path)
        {
            tree.AddAllAssetsAtPath(root, path, typeof(ScriptableObject));
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var regularPath = Utility.Path.GetRegularPath(dir);
                var dirName = regularPath.Split('/').Last();
                
                CreateAssetsMenu(tree, $"{root}/{dirName}", regularPath);
            }
        }
    }
}