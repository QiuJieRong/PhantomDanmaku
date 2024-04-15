using UnityEngine;

namespace PhantomDanmaku
{
    public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_Instance;

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    var go = new GameObject
                    {
                        name = typeof(T).Name
                    };
                    s_Instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
                return s_Instance;
            }
        }
    }
}