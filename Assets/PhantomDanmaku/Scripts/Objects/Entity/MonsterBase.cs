using Cysharp.Threading.Tasks;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace PhantomDanmaku.Runtime
{
    public class MonsterBase : EntityBase
    {
        protected Transform curAttackTarget;

        /// <summary>
        /// 掉落的物品
        /// </summary>
        public GameObject ItemWeapon;
        
        protected override void Start()
        {
            base.Start();
            m_Camp = Camp.Monster;
        }

        // protected virtual void Update()
        // {
        //     if (curAttackTarget != null)
        //     {
        //         Aim(curAttackTarget.position);
        //         var dir = Vector3.Normalize(curAttackTarget.position - transform.position);
        //         rig2D.velocity = dir * speed;
        //     }
        // }
        
        protected override void Attack()
        {
            throw new NotImplementedException();
        }

        protected override void Dead()
        {
            Components.EventCenter.EventTrigger(CustomEvent.MonsterDead, this);
            if (Random.Range(0f, 1f) < 0.2f)
            {
                //50%掉落武器
                ItemWeapon.gameObject.SetActive(true);
                ItemWeapon.transform.SetParent(transform.parent);
            }
            base.Dead();
        }
        public async UniTask AttackForBehaviorTree()
        {
            Attack();
            await UniTask.Delay(1000);
        }
    }
}