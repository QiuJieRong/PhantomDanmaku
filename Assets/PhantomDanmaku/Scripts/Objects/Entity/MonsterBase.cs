using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace PhantomDanmaku.Runtime
{
    public class MonsterBase : EntityBase
    {
        protected Transform curAttackTarget;
        protected Rigidbody2D rig2D;
        
        protected override void Start()
        {
            base.Start();
            m_Camp = Camp.Monster;
            rig2D = GetComponent<Rigidbody2D>();
        }


        protected virtual void Update()
        {
            if (curAttackTarget != null)
            {
                Aim(curAttackTarget.position);
                var dir = Vector3.Normalize(curAttackTarget.position - transform.position);
                rig2D.velocity = dir * m_Speed;
            }
        }
        
        protected override void Attack()
        {
            throw new NotImplementedException();
        }

        protected override void Dead()
        {
            base.Dead();
            Components.EventCenter.EventTrigger(CustomEvent.MonsterDead, this);
        }
        
    }
}