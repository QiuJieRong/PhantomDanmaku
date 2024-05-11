using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhantomDanmaku.Runtime.UI
{
    public class UIFormBase
    {
        public GameObject gameObject;

        public Transform transform => gameObject.transform;
        
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
            GameEntry.UI.RegisterUIMessage(key,action);
        }

        protected void UnRegisterUIMessage(string key, Action<object> action)
        {
            GameEntry.UI.UnRegisterUIMessage(key,action);
        }

        protected void SendUIMessage(string key,object userData)
        {
            GameEntry.UI.SendUIMessage(key,userData);
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