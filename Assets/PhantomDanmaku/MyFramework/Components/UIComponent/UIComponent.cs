using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MyFramework.Runtime
{
    public class UIComponent : GameFrameworkComponent
    {
        private readonly List<UIFormBase> m_OpenedUIFormList = new();

        private readonly Dictionary<Type,AsyncOperationHandle<GameObject>> m_UIFormHandleDic = new();

        private readonly Dictionary<Type, UIFormLoadState> m_UIFormLoadStateDic = new();

        private readonly List<UIFormBase> m_StandbyUIFormList = new();

        private readonly List<UIGroupBase> m_UIGroupBases = new();
        
        private UIFormBase Current => m_OpenedUIFormList.Last();
        
        private Transform Root => transform.Find("Root");

        private Transform Standby => Root.Find("Standby");

        private readonly Dictionary<string, List<Action<object>>> m_UIMessageDic = new();
        
        private void Start()
        {
            var standby = new GameObject("Standby");
            standby.transform.SetParent(Root);
        }

        public void RegisterUIMessage(string key,Action<object> action)
        {
            if (m_UIMessageDic.TryGetValue(key,out var actions))
            {
                actions.Add(action);
            }
            else
            {
                m_UIMessageDic.Add(key, new List<Action<object>> { action });
            }
        }

        public void SendUIMessage(string key,object userData)
        {
            if (m_UIMessageDic.TryGetValue(key, out var actions))
            {
                foreach (var action in actions)
                {
                    action.Invoke(userData);
                }
            }
        }

        public void UnRegisterUIMessage(string key,Action<object> action)
        {
            if (m_UIMessageDic.TryGetValue(key,out var actions))
            {
                actions.Remove(action);
            }
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="userData"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>是否打开成功</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async UniTask<bool> Open<T>(object userData) where T : UIFormBase
        {
            //判断页面是否已经存在
            if (m_OpenedUIFormList.Exists(uiForm => uiForm.GetType() == typeof(T)))
            {
                Debug.LogError("页面已存在");
                return false;
            }
            
            //判断页面是否加载
            if (m_UIFormLoadStateDic.TryGetValue(typeof(T), out var uiFormLoadState))
            {
                switch (uiFormLoadState)
                {
                    case UIFormLoadState.Loading:
                        Debug.LogError("页面正在加载");
                        return false;
                    case UIFormLoadState.Loaded:
                        //如果已经加载就在Standby中找
                        var standbyUIForm = GetStandbyUIForm<T>();
                        if (standbyUIForm != null)
                        {
                            standbyUIForm.gameObject.SetActive(true);
                            standbyUIForm.transform.SetParent(Root);
                            standbyUIForm.OnOpen(userData);
                            m_OpenedUIFormList.Add(standbyUIForm);
                            m_StandbyUIFormList.Remove(standbyUIForm);
                            return true;
                        }
                        //如果Standby中没有则重新实例化
                        else if (m_UIFormHandleDic.TryGetValue(typeof(T),out var handle) && handle.IsDone)
                        {
                            //实例化UI预制体
                            var go = Instantiate(handle.Result, transform);
                    
                            //实例并初始化UI对象
                            var uiForm = Activator.CreateInstance<T>();
                            uiForm.gameObject = go;
                            uiForm.OnInit(userData);
                            uiForm.OnOpen(userData);

                            //加入列表
                            m_OpenedUIFormList.Add(uiForm);
                            return true;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            //否则进行加载
            else
            {
                m_UIFormLoadStateDic.Add(typeof(T),UIFormLoadState.Loading);
                //获得UI类型上的UIPrefabName特性
                var type = typeof(T);
                var attributes = type.GetCustomAttributes(typeof(UIPrefabName), false);

                string path;
            
                //如果存在，则用该特性当作路径加载
                if (attributes.Length >= 1 && attributes[0] is UIPrefabName uiPrefabName)
                {
                    path = $"Assets/PhantomDanmaku/Prefabs/UI/UIForm/{uiPrefabName.Name}.prefab";
                }
                //否则用默认路径
                else
                {
                    path = $"Assets/PhantomDanmaku/Prefabs/UI/UIForm/{type.Name}.prefab";
                }

                //根据路径加载UI
                var uiHandle = Addressables.LoadAssetAsync<GameObject>(path);
                if (uiHandle.IsValid())
                {
                    m_UIFormHandleDic.Add(typeof(T),uiHandle);
                
                    uiHandle.Completed += handle =>
                    {
                        //实例化UI预制体
                        var go = Instantiate(handle.Result, Root);
                    
                        //实例并初始化UI对象
                        var uiForm = Activator.CreateInstance<T>();
                        uiForm.gameObject = go;
                        uiForm.OnInit(userData);
                        uiForm.OnOpen(userData);

                        //加入列表
                        m_OpenedUIFormList.Add(uiForm);
                        
                        //设置加载状态为已完成
                        m_UIFormLoadStateDic[typeof(T)] = UIFormLoadState.Loaded;
                    };
                    await uiHandle;
                    return true;
                }
                else
                {
                    Debug.LogError("预制体路径错误");
                    return false;
                }
            }

            return false;
        }
        
        public void Close<T>() where T : UIFormBase
        {
            var uiForm = GetOpenedUIForm<T>();
            if (uiForm != null)
            {
                Close(uiForm);
            }
        }

        public void Close(UIFormBase uiForm)
        {
            var uiFormGo = uiForm.gameObject;
            uiFormGo.SetActive(false);
            uiFormGo.transform.SetParent(Standby);
            uiForm.gameObject.SetActive(false);

            m_OpenedUIFormList.Remove(uiForm);
            m_StandbyUIFormList.Add(uiForm);
        }

        /// <summary>
        /// 获得当前打开的UIForm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetOpenedUIForm<T>() where T : UIFormBase
        {
            foreach (var uiFormBase in m_OpenedUIFormList)
            {
                if (uiFormBase is T uiForm)
                {
                    return uiForm;
                }
            }
            Debug.LogWarning("要获取的UIForm未打开");
            return null;
        }

        /// <summary>
        /// 获得待机的UIForm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetStandbyUIForm<T>() where T : UIFormBase
        {
            foreach (var uiFormBase in m_StandbyUIFormList)
            {
                if (uiFormBase is T uiForm)
                {
                    return uiForm;
                }
            }
            Debug.LogError("UIForm未待机");
            return null;
        }

        public void RegisterUIGroup(UIGroupBase uiGroup,GameObject gameObject)
        {
            m_UIGroupBases.Add(uiGroup);
            uiGroup.gameObject = gameObject;
        }

        public UIGroupBase GetUIGroup<T>(GameObject gameObject) where T : UIGroupBase
        {
            foreach (var uiGroupBase in m_UIGroupBases)
            {
                if (uiGroupBase is T && gameObject == uiGroupBase.gameObject)
                {
                    return uiGroupBase;
                }
            }

            return null;
        }
    }

    public enum UIFormLoadState
    {
        /// <summary>
        /// 加载中
        /// </summary>
        Loading,
        /// <summary>
        /// 加载完
        /// </summary>
        Loaded
    }
}