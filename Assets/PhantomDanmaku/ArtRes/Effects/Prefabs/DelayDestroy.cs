using UnityEngine;

namespace PhantomDanmaku.Runtime
{
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
            GameEntry.ObjectPool.PushObj(gameObject);
        }
    }
}