using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBase
{
    private Rigidbody2D rig;
    private Animator animator;
    private Controls controls;
    private WeaponBase currentWeapon;
    public WeaponBase CurrentWeapon => currentWeapon;
    void Start()
    {
        EventCenter.Instance.EventTrigger<EntityBase>(CustomEvent.PlayerSpawn, this);
        camp = "Player";
        speed = 10;
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        controls = new Controls();
        controls.Enable();
        controls.Player.Move.performed += (context) =>
        {
            Vector2 dir = context.ReadValue<Vector2>();
            rig.velocity = dir * speed;
            animator.SetFloat("Speed", rig.velocity.magnitude);
        };
        controls.Player.Move.canceled += (context) =>
        {
            rig.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        };
        controls.Player.Attack.performed += (context) =>
        {
            Attack();
        };
    }
    void Update()
    {
        Vector3 mousePosWS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (currentWeapon != null)
            currentWeapon.Ami(mousePosWS);
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
            currentWeapon.Attack();
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
    }
}
