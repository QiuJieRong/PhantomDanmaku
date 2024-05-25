using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhantomDanmaku.Runtime
{

    public class Player : EntityBase
    {
        private static Player instance;
        public static Player Instance => instance;
        private Rigidbody2D rig;
        private Animator animator;

        //是否在房间中
        public bool IsInRoom = true;
        private Camera m_MainCamera;

        private void Awake()
        {
            instance = this;
        }

        protected override void Start()
        {
            base.Start();
            m_MainCamera = Camera.main;
            Components.EventCenter.EventTrigger<EntityBase>(CustomEvent.PlayerSpawn, this);
            Components.UI.SendUIMessage("RefreshHUDUIForm",this);
            m_Camp = Camp.Player;
            m_Speed = 10;
            rig = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();

            Components.Input.Controls.Player.Attack.performed += AttackListener;
            
            //初始获得一把小刀
            Addressables.InstantiateAsync("Assets/PhantomDanmaku/Prefabs/Weapons/CloseInWeapon/Dagger.prefab")
                    .Completed +=
                handle =>
                {
                    SetCurrentWeapon(handle.Result.GetComponent<WeaponBase>());
                };
        }

        private void Update()
        {
            Vector2 dir = Components.Input.Controls.Player.Move.ReadValue<Vector2>();
            rig.velocity = dir * m_Speed;
            animator.SetFloat("Speed", rig.velocity.magnitude);
            
            var mousePosWs = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Aim(mousePosWs);
            CheckIsEnterRoom();
        }

        private void AttackListener(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Attack();
        }

        /// <summary>
        /// 判断是否进入了哪个房间
        /// </summary>
        private void CheckIsEnterRoom()
        {
            bool isInRoom = false;
            List<Room> rooms = MapGenerator.Instance.Rooms;
            if (rooms == null)
                return;
            foreach (var room in rooms)
            {
                if (transform.position.x > (room.CenterCoord.x - room.Info.Width / 2) &&
                    transform.position.x < (room.CenterCoord.x + room.Info.Width / 2 + 1) &&
                    transform.position.y > (room.CenterCoord.y - room.Info.Height / 2 + 0.5) &&
                    transform.position.y < (room.CenterCoord.y + room.Info.Height / 2 + 1))
                {
                    isInRoom = true;
                    Components.EventCenter.EventTrigger(CustomEvent.RoomEnter, room);
                    break;
                }
            }

            IsInRoom = isInRoom;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                if (Components.Input.Controls.Player.Interact.ReadValue<float>() == 1)
                {
                    var item = other.GetComponent<ItemBase>();
                    //使用道具，如果是枪则装备，如果是道具则使用，具体逻辑判断在该函数内部
                    item.UseItem(this);
                }
            }
        }

        protected override void Attack()
        {
            if (CurWeapon != null)
            {
                CurWeapon.Attack();
            }
        }
        public void ChangeCurrentWeapon(WeaponBase weapon)
        {
            //销毁原本的武器
            Destroy(CurWeapon.gameObject);
            //设置新武器为当前武器
            SetCurrentWeapon(weapon);
        }

        public override void Wounded(WeaponBase damageSource)
        {
            base.Wounded(damageSource);
            Components.EventCenter.EventTrigger<EntityBase>(CustomEvent.PlayerWounded, this);
            Components.UI.SendUIMessage("RefreshHUDUIForm",this);
        }

        protected override void Dead()
        {
            base.Dead();
            Components.Input.Controls.Player.Attack.performed -= AttackListener;
            Components.UI.Close<HUDUIForm>();
            Components.UI.Open<LevelEndUIForm>((Components.Battle.CurChapterIdx,Components.Battle.CurLevelIdx,false)).Forget();
            Components.Sound.PlaySound("Dead", false);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Components.Input.Controls.Player.Attack.performed -= AttackListener;
        }
    }

}