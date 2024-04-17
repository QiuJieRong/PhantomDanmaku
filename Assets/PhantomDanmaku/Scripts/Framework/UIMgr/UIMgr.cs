using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Video;

/// <summary>
/// 要放入Canvas下的哪个层级
/// </summary>
public enum E_UI_Layer
{
    /// <summary>
    /// 底层
    /// </summary>
    bot,
    /// <summary>
    /// 中层
    /// </summary>
    mid,
    /// <summary>
    /// 上层
    /// </summary>
    top,
    /// <summary>
    /// 系统层（所有层之上）
    /// </summary>
    system
}

/// <summary>
/// 要使用该UI管理器，需要在Resources文件夹下创建UI/Canvas和UI/EventSystem两个预设体
/// 所有的面板预设体也是从这个文件夹下加载
/// </summary>
public class UIMgr : SingletonBase<UIMgr>
{
    public RectTransform canvas;
    //声明各个层的Transform
    Transform bot;
    Transform mid;
    Transform top;
    Transform system;
    private Dictionary<string, PanelBase> panelDic = new Dictionary<string, PanelBase>();

    public UIMgr()
    {
        //在构造函数里创建Canvas和EventSystem
        GameObject obj = ResMgr.Instance.Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        obj = ResMgr.Instance.Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// 得到Canvas下各层级的Transform
    /// </summary>
    /// <param name="layer">层级枚举</param>
    /// <returns></returns>
    public Transform GetLayer(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.bot:
                return bot;
            case E_UI_Layer.mid:
                return mid;
            case E_UI_Layer.top:
                return top;
            case E_UI_Layer.system:
                return system;
        }
        return null;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="panelName">面板名字</param>
    /// <param name="layer">面板在Canvas下的层级,不填写默认中层</param>
    /// <param name="callback">面板创建好之后的回调函数，参数时创建好的面板</param>
    /// <typeparam name="T">面板组件的类型</typeparam>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.mid, UnityAction<T> callback = null) where T : PanelBase
    {
        if (panelDic.ContainsKey(panelName))
        {
            //如果面板已经存在，又显示的话就直接调用回调函数
            callback?.Invoke(panelDic[panelName] as T);
            return;
        }
        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (panelObj) =>
        {
            //加载好后，设置panelObj的父对象（设置层级）
            switch (layer)
            {
                case E_UI_Layer.bot:
                    panelObj.transform.SetParent(bot, false);
                    break;
                case E_UI_Layer.mid:
                    panelObj.transform.SetParent(mid, false);
                    break;
                case E_UI_Layer.top:
                    panelObj.transform.SetParent(top, false);
                    break;
                case E_UI_Layer.system:
                    panelObj.transform.SetParent(system, false);
                    break;
            }
            T panel = panelObj.GetComponent<T>();
            //执行面板的Show函数
            panel.Show();
            //将面板组件存入字典
            panelDic.Add(panelName, panel);
            //通过回调函数返回这个面板组件,此时这个面板组件的Start函数还未执行,
            //此时加载好了面板，这个面板的Awake函数执行完毕，但是尚未执行这个面板的Start
            //有一些操作可能会被面板Start里的操作覆盖，比如对Text控件的text文本赋值
            //所以不希望操作覆盖回调函数里的操作，可以卸载Panelbase子类的重写的Awake函数里
            callback?.Invoke(panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">要隐藏的面板名字</param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            //执行面板的Hide函数
            panelDic[panelName].Hide();
            //销毁该面板
            if (panelDic[panelName] != null)
                Object.Destroy(panelDic[panelName].gameObject);
            //从字典里移除
            panelDic.Remove(panelName);
        }
    }

    public T GetPanel<T>(string panelName) where T : PanelBase
    {
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        return null;
    }

    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger eventTrigger = control.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
            eventTrigger = control.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
    }
}
