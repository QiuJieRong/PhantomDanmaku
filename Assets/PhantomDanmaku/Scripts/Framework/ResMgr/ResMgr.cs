using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 从Resources文件夹下加载资源
/// </summary>
public class ResMgr : SingletonBase<ResMgr>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="path">Resources文件夹下的资源路径</param>
    /// <typeparam name="T">资源类型</typeparam>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        //如果加载的资源类型时GameObject，直接实例化并返回实例化对象
        if(res is GameObject)
            res = GameObject.Instantiate(res);
        //返回资源
        return res;
    }

    public void LoadAsync<T>(string path,UnityAction<T> callback) where T : Object
    {
        MonoMgr.Instance.StartCoroutine(CoroutineLoadAsync<T>(path,callback));
    }
    private IEnumerator CoroutineLoadAsync<T>(string path,UnityAction<T> callback) where T : Object
    {
        ResourceRequest rq = Resources.LoadAsync<T>(path);
        yield return rq;
        if(rq.asset is GameObject)
            callback(GameObject.Instantiate(rq.asset as T));
        else
            callback(rq.asset as T);
    }
}
