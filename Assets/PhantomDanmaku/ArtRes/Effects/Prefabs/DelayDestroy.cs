using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        Invoke("DestroySelf", 2.0f);
    }
    void DestroySelf()
    {
        // PoolMgr.Instance.PushObj(gameObject);
    }
}
