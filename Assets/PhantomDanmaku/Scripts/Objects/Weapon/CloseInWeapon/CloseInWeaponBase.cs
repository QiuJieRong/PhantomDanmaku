using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    public class CloseInWeaponBase : WeaponBase
    {
        [LabelText("攻击范围")]
        public Collider2D AttackRange;

        [LabelText("检测时间")]
        public Vector2 EffectivePeriod;

        [LabelText("武器动画机")]
        public Animator Animator;

        private UniTask m_AttackUniTask;

        private bool m_CanAttack;

        protected override void Start()
        {
            base.Start();
            atk = 2;
            interval = 0.3f;
            //一开始没有碰撞
            AttackRange.gameObject.SetActive(false);
            m_CanAttack = true;
        }

        private void Update()
        {
            lastAttackTime += Time.deltaTime;
        }

        private void OnDestroy()
        {
            m_CanAttack = false;
        }

        public override async void Attack()
        {
            if (lastAttackTime < interval)
                return;
            
            lastAttackTime = 0;
            //播放攻击动画
            Animator.Play("Attack");
            Animator.Update(0);

            var attackedEntities = new List<EntityBase>();
            
            float time = 0;
            
            Components.Sound.PlaySound("Fire", false);
            
            while (Animator != null && time < interval)
            {
                if (!m_CanAttack)
                    return;
                //如果在有效时段内，则进行碰撞检测
                if (EffectivePeriod.InRange(time))
                {
                    if (!AttackRange.gameObject.activeSelf)
                    {
                        AttackRange.gameObject.SetActive(true);
                    }
                    var hits = new List<RaycastHit2D>();
                    var filter2D = new ContactFilter2D();
                    AttackRange.Cast(Vector2.zero, filter2D, hits, 0);
                    foreach (var hit2D in hits)
                    {
                        //如果碰撞的物体是实体，并且和自己的拥有者不是一个阵营，则对其造成伤害
                        if (hit2D.transform.TryGetComponent<EntityBase>(out var entity) 
                            && entity.Camp != owner.Camp
                            && !attackedEntities.Contains(entity))
                        {
                            entity.Wounded(this);
                            attackedEntities.Add(entity);
                        }
                    }
                }
                
                time += Time.deltaTime;
                await UniTask.DelayFrame(1);
            }

            if (AttackRange != null)
            {
                AttackRange.gameObject.SetActive(false);
            }
        }
    }
}