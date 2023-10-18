using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // Debug.Log("SingletonBase中的Instance属性get执行" + typeof(T));
            if(instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}
