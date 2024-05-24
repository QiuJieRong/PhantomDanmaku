using UnityEngine;
using UnityEngine.InputSystem;

namespace PhantomDanmaku.Runtime
{
    public class InteroperableObjectBase : MonoBehaviour,Interoperable
    {
        private Transform m_Transform;
        public Transform Tip;
        public virtual bool CanInteract { get; }

        protected virtual void Start()
        {
            m_Transform = transform;
        }

        private void InteractKeyListener(InputAction.CallbackContext context)
        {
            if (CanInteract)
            {
                Interact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //监听交互按键
                Components.Input.Controls.Player.Interact.performed += InteractKeyListener;
                ShowTip();
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //取消监听交互按键
                Components.Input.Controls.Player.Interact.performed -= InteractKeyListener;
                HideTip(); 
            }
        }

        protected virtual void OnDestroy()
        {
            //取消监听交互按键
            Components.Input.Controls.Player.Interact.performed -= InteractKeyListener;
        }

        public virtual void ShowTip()
        {
            Tip.gameObject.SetActive(true);
        }

        public virtual void HideTip()
        {
            Tip.gameObject.SetActive(false);
        }

        public virtual void Interact()
        {
        }
    }
}