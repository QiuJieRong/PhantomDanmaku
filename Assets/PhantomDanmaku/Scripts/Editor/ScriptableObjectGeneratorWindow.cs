using System;
using System.IO;
using PhantomDanmaku.Config;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace PhantomDanmaku.Editor
{
    public class ScriptableObjectGeneratorWindow : OdinEditorWindow
    {
        private int selectedTypeIndex;
        [MenuItem("配置/创建配置")]
        private static void OpenWindow()
        {
            GetWindow<ScriptableObjectGeneratorWindow>().Show();
        }
        protected override void OnGUI()
        {
            var types = ConfigUtility.FindAllTypes<SerializedScriptableObject>().ToArray();

            var typeNames = new string[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                typeNames[i] = types[i].Name;
            }
            
            selectedTypeIndex = EditorGUILayout.Popup("选择资产类型", selectedTypeIndex, typeNames);

            if (GUILayout.Button("生成资产"))
            {
                Type selectedType = types[selectedTypeIndex];
                if (selectedType != null && selectedType.IsSubclassOf(typeof(ScriptableObject)))
                {
                    ScriptableObject obj = CreateInstance(selectedType);
                    string path = EditorUtility.SaveFilePanel("保存资产", Application.dataPath, selectedType.Name, "asset");

                    if (!string.IsNullOrEmpty(path))
                    {
                        path = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                        AssetDatabase.CreateAsset(obj, ConfigUtility.GetRelativePath(path + ".asset"));
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "未选择继承自ScriptableObject的类", "OK");
                }
            }
        }
    }
}