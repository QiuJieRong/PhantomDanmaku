using UnityEngine.Events;

/// <summary>
/// 可以提供给外部添加想要每帧执行的函数的方法
/// 可以提供给外部开启协程的方法
/// </summary>
public class MonoMgr : SingletonAutoMono<MonoMgr>
{
    private event UnityAction updateEvent;

    void Update()
    {
        if(updateEvent != null)
            updateEvent.Invoke();
    }
    /// <summary>
    /// 添加每帧执行的方法
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }
    /// <summary>
    /// 移除每帧执行的方法
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }
}
