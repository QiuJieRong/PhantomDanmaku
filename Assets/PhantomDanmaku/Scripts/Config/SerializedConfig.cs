using Sirenix.OdinInspector;
using UnityEngine;

namespace PhantomDanmaku.Config
{
    public class SerializedConfig : SerializedScriptableObject
    {
        [InlineButton("Refresh","刷新")]
        public string Guid = System.Guid.NewGuid().ToString();

        private void Refresh()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}