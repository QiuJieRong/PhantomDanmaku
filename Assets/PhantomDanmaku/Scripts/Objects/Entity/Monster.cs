using System.Collections;
using UnityEngine;

namespace PhantomDanmaku.Runtime
{

    public class Monster : MonsterBase
    {
        public WeaponBase weapon;
        private Coroutine m_AttackCoroutine;

        protected override void Attack()
        {
            weapon.Attack();
        }

        protected override void Start()
        {
            base.Start();
            SetCurrentWeapon(weapon);
        }

        protected override void Update()
        {
            base.Update();
            if (Player.Instance.IsInRoom)
            {
                //检测范围内是否有敌人，如果有的话设置攻击目标,如果没有的话攻击目标设置为空
                Collider2D target =
                    Physics2D.OverlapCircle(transform.position, 15, 1 << LayerMask.NameToLayer("Player"));
                if (target == null)
                    curAttackTarget = null;
                else
                {
                    if (m_AttackCoroutine == null)
                        m_AttackCoroutine = StartCoroutine(AttackIenum());
                    curAttackTarget = target.transform;
                }

                if (curAttackTarget== null)
                {
                    rig2D.velocity = Vector3.zero;
                    if (m_AttackCoroutine != null)
                        StopCoroutine(m_AttackCoroutine);
                    m_AttackCoroutine = null;
                }
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
    }
}