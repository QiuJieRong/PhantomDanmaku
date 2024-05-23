using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}