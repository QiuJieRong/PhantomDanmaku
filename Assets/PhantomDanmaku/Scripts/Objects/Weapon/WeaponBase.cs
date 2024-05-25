using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    
    public abstract class WeaponBase : MonoBehaviour
    {
        [HideInInspector]
        public EntityBase owner;
        /// <summary>
        /// 阵营数据需要额外存储一份，一份拥有者死亡后，获取不了阵营
        /// </summary>
        private Camp m_Camp;
        public Camp Camp => m_Camp;
        
        [LabelText("攻击力")]
        public int atk;//攻击力
        public int Atk => atk;
        protected float interval;//攻击间隔

        /// <summary>
        /// 距离上次攻击过去的时间
        /// </summary>
        protected float lastAttackTime;
        
        public int EnergyConsume;//攻击能量消耗

        protected Transform m_Transform;

        public Transform Transform => m_Transform;

        // private static readonly Vector3 _reverseX = new(-1, 1, 1);
        
        /// <summary>
        /// 击退力
        /// </summary>
        public float Repel;


        protected virtual void Start()
        {
            m_Transform = transform;
        }

        public void SetOwner(EntityBase entity)
        {
            owner = entity;
            m_Camp = entity.Camp;
        }

        public void Aim(Vector3 dir)
        {
            // m_Transform.localScale = dir.x < 0 ? _reverseX : Vector3.one;
            
            // dir = Quaternion.AngleAxis(90, Vector3.forward) * dir;
            
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }

        public virtual void Attack()
        {
            owner.CurEnergy -= EnergyConsume;
        }
    }
}