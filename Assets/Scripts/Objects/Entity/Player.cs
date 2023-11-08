using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBase
{
    public static Player instance;
    public Player Instance => instance;
    private Rigidbody2D rig;
    private Animator animator;
    private Controls controls;
    private WeaponBase currentWeapon;
    public WeaponBase CurrentWeapon => currentWeapon;
    //是否在房间中
    public bool IsInRoom = true;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        EventCenter.Instance.EventTrigger<EntityBase>(CustomEvent.PlayerSpawn, this);
        camp = "Player";
        speed = 10;
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        controls = new Controls();
        controls.Enable();
        controls.Player.Attack.performed += (context) =>
        {
            Attack();
        };
    }
    void Update()
    {
        Vector2 dir = controls.Player.Move.ReadValue<Vector2>();
        rig.velocity = dir * speed;
        animator.SetFloat("Speed", rig.velocity.magnitude);

        Vector3 mousePosWS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (currentWeapon != null)
            currentWeapon.Ami(mousePosWS);
        CheckIsEnterRoom();
    }

    /// <summary>
    /// 判断是否进入了哪个房间
    /// </summary>
    void CheckIsEnterRoom()
    {
        bool isInRoom = false;
        List<Room> rooms = MapGenerator.Instance.Rooms;
        foreach (Room room in rooms)
        {
            if (transform.position.x > (room.CenterCoord.x - room.Info.Width / 2) &&
                transform.position.x < (room.CenterCoord.x + room.Info.Width / 2 + 1) &&
                transform.position.y > (room.CenterCoord.y - room.Info.Height / 2 + 0.5) &&
                transform.position.y < (room.CenterCoord.y + room.Info.Height / 2 + 1))
            {
                isInRoom = true;
                EventCenter.Instance.EventTrigger(CustomEvent.RoomEnter, room);
                break;
            }
        }
        IsInRoom = isInRoom;
        // if(!IsInRoom)
        // {
        //     EventCenter.Instance.EventTrigger(CustomEvent.RoomLeave);
        // }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            if (controls.Player.Interact.ReadValue<float>() == 1)
            {
                ItemBase item = other.GetComponent<ItemBase>();
                //使用道具，如果是枪则装备，如果是道具则使用，具体逻辑判断在该函数内部
                item.UseItem(this);
            }
        }
    }

    protected override void Attack()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Attack();
            SoundMgr.Instance.PlaySound("Fire", false);
        }
    }

    public override void SetCurrentWeapon(WeaponBase weapon)
    {
        currentWeapon = weapon;
        //设置父对象和位置
        weapon.transform.SetParent(transform.Find("Weapons").transform);
        weapon.transform.localPosition = Vector3.zero;
        //设置武器的拥有者
        weapon.SetOwner(this);
    }
    public void ChangeCurrentWeapon(WeaponBase weapon)
    {
        //销毁原本的武器
        Destroy(currentWeapon.gameObject);
        //设置新武器为当前武器
        SetCurrentWeapon(weapon);
    }

    public override void Wounded(WeaponBase damageSource)
    {
        base.Wounded(damageSource);
        EventCenter.Instance.EventTrigger<EntityBase>(CustomEvent.PlayerWounded, this);
    }

    public override void Dead()
    {
        base.Dead();
        controls.Dispose();
        UIMgr.Instance.HidePanel("GamePanel");
        UIMgr.Instance.ShowPanel<EndPanel>("EndPanel");
        SoundMgr.Instance.PlaySound("Dead", false);
    }

    void OnDestroy()
    {
        controls.Dispose();
    }
}
