using System.Collections.Generic;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityEngine.Events;

namespace PhantomDanmaku
{
    public interface IEventAction{}

    public class EventAction<T> : IEventAction
    {
        public UnityAction<T> actions;
        public EventAction(UnityAction<T> action)
        {
            actions += action;
        }
    }

    public class EventAction : IEventAction
    {
        public UnityAction actions;
        public EventAction(UnityAction action)
        {
            actions += action;
        }
    }


    public class GameSystem : Singleton<GameSystem>
    {
        #region 事件中心
        
        private readonly Dictionary<string, IEventAction> m_EventDic = new Dictionary<string, IEventAction>();

        /// <summary>
        /// 添加事件监听，事件触发时执行，用泛型委托接收参数
        /// </summary>
        /// <param name="key">要监听的事件的名字</param>
        /// <param name="action">事件触发后要执行的委托函数，委托默认传参object</param>
        public void AddEventListener<T>(string key, UnityAction<T> action)
        {
            //如果字典不包含该事件
            if (!m_EventDic.ContainsKey(key))
                m_EventDic.Add(key, new EventAction<T>(action));
            else if (m_EventDic[key] is EventAction<T> eventAction)
            {
                eventAction.actions += action;
            }
            else
            {
                Debug.LogError($"已添加{key}有参监听，但尝试添加无参监听");
            }
        }

        public void AddEventListener(string key, UnityAction action)
        {
            //如果字典不包含该事件
            if (!m_EventDic.ContainsKey(key))
                m_EventDic.Add(key, new EventAction(action));
            else if (m_EventDic[key] is EventAction eventAction)
            {
                eventAction.actions += action;
            }
            else
            {
                Debug.LogError($"已添加{key}无参监听，但尝试添加有参监听");
            }
        }

        /// <summary>
        /// 移除事件监听，用泛型委托接收参数
        /// </summary>
        /// <param name="key">要移除监听的事件名字</param>
        /// <param name="action">要移除的监听委托函数</param>
        public void RemoveEventListener<T>(string key, UnityAction<T> action)
        {
            if (m_EventDic.TryGetValue(key, out var iEventAction) && iEventAction is EventAction<T> eventAction)
            {
                eventAction.actions -= action;
            }
        }

        /// <summary>
        /// 移除事件监听，用委托没有参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void RemoveEventListener(string key, UnityAction action)
        {
            if (m_EventDic.TryGetValue(key, out var iEventAction) && iEventAction is EventAction eventAction)
            {
                eventAction.actions -= action;
            }
        }

        /// <summary>
        /// 触发事件,传递泛型参数
        /// </summary>
        /// <param name="key">要触发的事件名字</param>
        /// <param name="info">为触发事件时，执行的委托函数传参</param>
        public void EventTrigger<T>(string key, T info)
        {
            if (m_EventDic.TryGetValue(key, out var iEventAction) && iEventAction is EventAction<T> eventAction)
            {
                eventAction.actions?.Invoke(info);
            }
        }

        /// <summary>
        /// 触发事件，不传递参数
        /// </summary>
        /// <param name="key"></param>
        public void EventTrigger(string key)
        {
            if (m_EventDic.TryGetValue(key, out var iEventAction) && iEventAction is EventAction eventAction)
            {
                eventAction.actions?.Invoke();
            }
        }

        /// <summary>
        /// 清空事件中心，一般在场景切换时调用
        /// </summary>
        public void Clear()
        {
            m_EventDic.Clear();
        }

        #endregion
    }
}