using System;
using MyFramework.Runtime;

namespace PhantomDanmaku.Runtime
{
    public class InputComponent : GameFrameworkComponent
    {
        public Controls Controls { get; private set; }

        private void Start()
        {
            Controls = new Controls();
            Controls.Enable();
        }
    }
}