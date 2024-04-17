using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhantomDanmaku.Scripts.System
{
    public class PhantomSystem : SingletonBase<PhantomSystem>
    {
        private static Dictionary<Type, SerializedScriptableObject> s_Configs;

        /// <summary>
        /// 加载配置
        /// </summary>
        public async UniTask LoadConfig()
        {
            s_Configs = new Dictionary<Type, SerializedScriptableObject>();
            var handle = Addressables.LoadAssetsAsync<ScriptableObject>("Config", obj=>
            {
                if (!s_Configs.ContainsKey(obj.GetType()))
                {
                    s_Configs.Add(obj.GetType(), obj as SerializedScriptableObject);
                }
            });
            await handle;
        }
        
        /// <summary>
        /// 获得配置表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : SerializedScriptableObject
        {
            if (s_Configs.TryGetValue(typeof(T), out var database))
            {
                return database as T;
            }
            Debug.LogError("配置表不存在！");
            return null;
        }
    }
}