using System;
using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    
    public abstract class WeaponBase : MonoBehaviour
    {
        [HideInInspector]
        public EntityBase owner;
        private Camp m_Camp;
        public Camp Camp => m_Camp;
        
        protected int atk;//攻击力
        public int Atk => atk;
        protected int interval;//攻击间隔
        protected int consume;//攻击能量消耗

        protected Transform m_Transform;

        private static Vector3 _reverseXY = new(-1, -1, 1);

        protected virtual void Start()
        {
            m_Transform = transform;
        }

        public void SetOwner(EntityBase entity)
        {
            owner = entity;
            m_Camp = entity.Camp;
        }

        /// <summary>
        /// 武器的朝向也决定了角色的朝向
        /// </summary>
        public void Aim(Vector3 dir)
        {
            m_Transform.localScale = dir.x < 0 ? _reverseXY : Vector3.one;
            
            dir = Quaternion.AngleAxis(90, Vector3.forward) * dir;
            
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }
        public abstract void Attack();

    }

}