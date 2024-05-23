using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    public class InputMgr : SingletonBase<InputMgr>
    {
        //是否开启输入检测
        private bool isCheckInput;

        public InputMgr()
        {
            //默认开启输入检测
            isCheckInput = true;
            MonoMgr.Instance.AddUpdateListener(Update);
        }

        /// <summary>
        /// 开启输入检测
        /// </summary>
        public void OpenInputCheck()
        {
            isCheckInput = true;
        }

        /// <summary>
        /// 关闭输入检测
        /// </summary>
        public void CloseInputCheck()
        {
            isCheckInput = false;
        }

        private void Update()
        {
            //如果没有开启输入检测，直接返回
            if (!isCheckInput)
                return;
            CheckKeyDownAndUp(KeyCode.W);
            CheckKeyDownAndUp(KeyCode.A);
            CheckKeyDownAndUp(KeyCode.S);
            CheckKeyDownAndUp(KeyCode.D);
            CheckAnyKeyPress();
        }

        public void CheckKeyDownAndUp(KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                Components.EventCenter.EventTrigger<KeyCode>("KeyDown", key);
            }

            if (Input.GetKeyUp(key))
            {
                Components.EventCenter.EventTrigger<KeyCode>("KeyUp", key);
            }
        }

        public void CheckAnyKeyPress()
        {
            if (Input.anyKey)
                Components.EventCenter.EventTrigger("AnyKeyDown");
        }
    }
}