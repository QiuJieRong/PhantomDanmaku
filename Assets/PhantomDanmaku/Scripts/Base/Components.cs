using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    public partial class Components : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}