using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime.System;
using PhantomDanmaku.Runtime.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace PhantomDanmaku.Runtime
{

    public class Player : EntityBase
    {
        private static Player instance;
        public static Player Instance => instance;
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
            speed = 10;
            rig2D = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();

            Components.Input.Controls.Player.Attack.performed += AttackListener;
            Components.Input.Controls.Player.SwitchWeapon.performed += SwitchWeaponListener;
            Components.Input.Controls.Player.ShowLine.performed += ShowLineListener;
            Components.Input.Controls.Player.Exit.performed += ExitListener;
            GetKnife();
        }

        /// <summary>
        /// 重写初始化属性函数
        /// </summary>
        protected override void InitStateValue()
        {
            var playerData = PhantomSystem.Instance.PlayerData;
            maxHp = playerData.GetStateValue(State.MaxHp).Value;
            curHp = maxHp;
            maxShield = playerData.GetStateValue(State.MaxShield).Value;
            curShield = maxShield;
            maxEnergy = playerData.GetStateValue(State.MaxEnergy).Value;
            curEnergy = maxEnergy;
            speed = playerData.GetStateValue(State.Speed).Value;
        }

        private void GetKnife()
        {
            //获得一把小刀
            Addressables.InstantiateAsync("Assets/PhantomDanmaku/Prefabs/Weapons/CloseInWeapon/Dagger.prefab")
                    .Completed +=
                handle =>
                {
                    if (CurWeapon != null)
                        ChangeCurrentWeapon(handle.Result.GetComponent<WeaponBase>());
                    else
                        SetCurrentWeapon(handle.Result.GetComponent<WeaponBase>());
                };
        }

        private void Update()
        {
            Vector2 dir = Components.Input.Controls.Player.Move.ReadValue<Vector2>();
            rig2D.velocity = dir * speed;
            animator.SetFloat("Speed", rig2D.velocity.magnitude);
            
            var mousePosWs = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Aim(mousePosWs);
            CheckIsEnterRoom();
        }

        private void AttackListener(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Attack();
        }

        private void SwitchWeaponListener(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            GetKnife();
        }
        
        
        private async void ExitListener(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            //返回大厅
            Components.ObjectPool.Clear();
            Components.UI.Close<HUDUIForm>();
            await SceneManager.UnloadSceneAsync("BattleScene");
                
            //进入大厅
            SceneManager.LoadScene("HallScene", LoadSceneMode.Additive);
        }
        
        private void ShowLineListener(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            MonsterBase.ShowLine = !MonsterBase.ShowLine;
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
                    CurRoom = room;
                    Components.EventCenter.EventTrigger(CustomEvent.RoomEnter, room);
                    break;
                }
            }

            IsInRoom = isInRoom;

            if (!isInRoom)
            {
                CurRoom = null;
            }
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
            if (CurWeapon != null && CurEnergy >= CurWeapon.EnergyConsume)
            {
                CurWeapon.Attack();
                Components.UI.SendUIMessage("RefreshHUDUIForm",this);
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
            Components.Input.Controls.Player.Attack.performed -= AttackListener;
            Components.Input.Controls.Player.SwitchWeapon.performed -= SwitchWeaponListener;
            Components.Input.Controls.Player.ShowLine.performed -= ShowLineListener;
            Components.Input.Controls.Player.Exit.performed -= ExitListener;
            Components.UI.Close<HUDUIForm>();
            Components.UI.Open<LevelEndUIForm>((Components.Battle.CurChapterIdx,Components.Battle.CurLevelIdx,false)).Forget();
            Components.Sound.PlaySound("Dead", false);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Components.Input.Controls.Player.Attack.performed -= AttackListener;
            Components.Input.Controls.Player.SwitchWeapon.performed -= SwitchWeaponListener;
            Components.Input.Controls.Player.ShowLine.performed -= ShowLineListener;
            Components.Input.Controls.Player.Exit.performed -= ExitListener;
        }
    }

}