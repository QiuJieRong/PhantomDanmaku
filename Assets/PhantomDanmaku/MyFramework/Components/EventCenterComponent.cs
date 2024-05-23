using System.Collections.Generic;
using UnityEngine.Events;

namespace MyFramework.Runtime
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
    public class EventCenterComponent : GameFrameworkComponent
    {
        private Dictionary<string ,IEventAction> eventDic = new Dictionary<string, IEventAction>();

    /// <summary>
    /// 添加事件监听，事件触发时执行，用泛型委托接收参数
    /// </summary>
    /// <param name="name">要监听的事件的名字</param>
    /// <param name="action">事件触发后要执行的委托函数，委托默认传参object</param>
    public void AddEventListener<T>(string name,UnityAction<T> action)
    {
        //如果字典不包含该事件
        if(!eventDic.ContainsKey(name))
            eventDic.Add(name,new EventAction<T>(action));
        else
            (eventDic[name] as EventAction<T>).actions += action;
    }
    public void AddEventListener(string name,UnityAction action)
    {
        //如果字典不包含该事件
        if(!eventDic.ContainsKey(name))
            eventDic.Add(name,new EventAction(action));
        else
            (eventDic[name] as EventAction).actions += action;
    }

    /// <summary>
    /// 移除事件监听，用泛型委托接收参数
    /// </summary>
    /// <param name="name">要移除监听的事件名字</param>
    /// <param name="action">要移除的监听委托函数</param>
    public void RemoveEventListener<T>(string name,UnityAction<T> action)
    {
        if(eventDic.ContainsKey(name))
            (eventDic[name] as EventAction<T>).actions -= action;
    }
    /// <summary>
    /// 移除事件监听，用委托没有参数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name,UnityAction action)
    {
        if(eventDic.ContainsKey(name))
            (eventDic[name] as EventAction).actions -= action;
    }

    /// <summary>
    /// 触发事件,传递泛型参数
    /// </summary>
    /// <param name="name">要触发的事件名字</param>
    /// <param name="info">为触发事件时，执行的委托函数传参</param>
    public void EventTrigger<T>(string name,T info)
    {
        if(eventDic.ContainsKey(name))
            (eventDic[name] as EventAction<T>).actions?.Invoke(info);
    }
    /// <summary>
    /// 触发事件，不传递参数
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        if(eventDic.ContainsKey(name))
            (eventDic[name] as EventAction).actions?.Invoke();
    }

    /// <summary>
    /// 清空事件中心，一般在场景切换时调用
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
    }
}