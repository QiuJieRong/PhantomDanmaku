using Sirenix.OdinInspector;
using UnityEngine;

namespace PhantomDanmaku.Config
{
    public class SerializedConfig : SerializedScriptableObject
    {
        [ReadOnly]
        public string Guid = System.Guid.NewGuid().ToString();
    }
}