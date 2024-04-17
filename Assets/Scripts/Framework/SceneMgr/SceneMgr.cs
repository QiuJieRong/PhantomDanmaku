using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : SingletonBase<SceneMgr>
{
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="sceneName">要加载的场景的名字</param>
    /// <param name="action">加载完场景后要执行的方法</param>
    public void LoadScene(string sceneName,UnityAction action = null)
    {
        SceneManager.LoadScene(sceneName);
        action?.Invoke();
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">要加载的场景的名字</param>
    /// <param name="action">加载完场景后要执行的方法</param>
    public void LoadSceneAsync(string sceneName,UnityAction action)
    {
        MonoMgr.Instance.StartCoroutine(CoroutineLoadSceneAsync(sceneName,action));
    }

    private IEnumerator CoroutineLoadSceneAsync(string sceneName,UnityAction action)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        while(!ao.isDone)
        {
            //向事件中心分发进度更新事件
            EventCenter.Instance.EventTrigger("LoadSceneProgressUpdate",ao.progress);
            yield return ao.progress;
        }
        action.Invoke();
    }
}
