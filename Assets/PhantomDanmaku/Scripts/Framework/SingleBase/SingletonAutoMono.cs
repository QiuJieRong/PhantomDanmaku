using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // Debug.Log("SingletonAutoBase中的Instance属性get执行" + typeof(T));
            if (instance == null)
            {
                //如果还实例化则创建一个对象，名字和类名一样，将类实例化对象挂载上去
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
}
