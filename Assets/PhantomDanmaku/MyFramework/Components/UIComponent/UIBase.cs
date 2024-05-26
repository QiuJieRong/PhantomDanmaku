using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PhantomDanmaku.Runtime.UI
{
    public class UIBase
    {
        
        public GameObject gameObject;

        public Transform transform => gameObject.transform;

        private readonly List<AsyncOperationHandle> m_Handles = new();
        public virtual void OnInit(object userData)
        {
            InstallField();
        }
        
        public virtual void OnOpen(object userData)
        {
            
        }

        public virtual void OnClose()
        {
        
        }

        protected virtual void InstallField()
        {
            
        }

        protected void RegisterUIMessage(string key, Action<object> action)
        {
            Components.UI.RegisterUIMessage(key,action);
        }

        protected void UnRegisterUIMessage(string key, Action<object> action)
        {
            Components.UI.UnRegisterUIMessage(key,action);
        }

        protected void SendUIMessage(string key,object userData)
        {
            Components.UI.SendUIMessage(key,userData);
        }

        protected void AddDependence(AsyncOperationHandle handle)
        {
            m_Handles.Add(handle);
        }

        /// <summary>
        /// 释放依赖的资源
        /// </summary>
        protected void Release()
        {
            foreach (var handle in m_Handles)
            {
                Addressables.Release(handle);
            }
        }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class UIPrefabName : Attribute
    {
        public string Name { get; }

        public UIPrefabName(string prefabName)
        {
            Name = prefabName;
        }
    }
}