using System;
using PhantomDanmaku.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PhantomDanmaku.Scripts.Objects.Interoperable
{
    public class InteroperableObjectBase : MonoBehaviour,Interoperable
    {
        private Transform m_Transform;
        public Transform Tip;
        public bool CanInteract { get; set; }

        private void Start()
        {
            m_Transform = transform;
        }

        private void InteractKeyListener(InputAction.CallbackContext context)
        {
            Interact();
        }

        /// <summary>
        /// 添加按键交互监听
        /// </summary>
        protected virtual void AddInteractKeyListener()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CanInteract = true;
            //监听交互按键
            Components.Input.Controls.Player.Interact.performed += InteractKeyListener;
            ShowTip();
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            CanInteract = false;
            //取消监听交互按键
            Components.Input.Controls.Player.Interact.performed -= InteractKeyListener;
            HideTip();
        }

        private void OnDestroy()
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