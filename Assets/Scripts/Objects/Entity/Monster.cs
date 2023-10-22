using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : EntityBase
{
    private Transform attackTarget;
    private Rigidbody2D rig;
    public WeaponBase weapon;
    private Coroutine attackCoroutine;
    public override void SetCurrentWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
        weapon.SetOwner(this);
    }

    protected override void Attack()
    {
        weapon.Attack();
    }

    public override void Wounded(WeaponBase damageSource)
    {
        base.Wounded(damageSource);
    }

    void Start()
    {
        camp = "Monster";
        rig = GetComponent<Rigidbody2D>();
        SetCurrentWeapon(weapon);
        // attackCoroutine = StartCoroutine(AttackIenum());
    }
    void Update()
    {
        //检测范围内是否有敌人，如果有的话设置攻击目标,如果没有的话攻击目标设置为空
        Collider2D target = Physics2D.OverlapCircle(transform.position, 15, 1 << LayerMask.NameToLayer("Player"));
        if (target == null)
            attackTarget = null;
        else
        {
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackIenum());
            attackTarget = target.transform;
        }
        if (attackTarget != null)
        {
            weapon.Ami(attackTarget.position);
            Vector3 dir = Vector3.Normalize(attackTarget.position - transform.position);
            rig.velocity = dir * speed;
        }
        else
        {
            rig.velocity = Vector3.zero;
            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    IEnumerator AttackIenum()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(1);
        }
    }

    public override void Dead()
    {
        base.Dead();
        EventCenter.Instance.EventTrigger(CustomEvent.MonsterDead, gameObject);
    }
}
