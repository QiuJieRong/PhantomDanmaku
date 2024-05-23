using UnityEngine;
namespace PhantomDanmaku.Runtime
{
    public abstract class EntityBase : MonoBehaviour
    {
        /// <summary>
        /// 最大生命值
        /// </summary>
        private int m_MaxHp = 5;
        public int MaxHp => m_MaxHp;
        /// <summary>
        /// 当前生命值
        /// </summary>
        private int m_Hp = 5;
        public int Hp => m_Hp;
        /// <summary>
        /// 最大护盾值
        /// </summary>
        private int m_MaxShield = 3;
        public int MaxShield => m_MaxShield;
        /// <summary>
        /// 当前护盾值
        /// </summary>
        private int m_Shield = 3;
        public int Shield => m_Shield;
        
        /// <summary>
        /// 最大能量值
        /// </summary>
        private int m_MaxEnergy = 100;
        public int MaxEnergy => m_MaxEnergy;
        /// <summary>
        /// 当前能量值
        /// </summary>
        private int m_CurEnergy = 100;
        public int CurEnergy => m_CurEnergy;

        private WeaponBase m_CurWeapon;

        public WeaponBase CurWeapon => m_CurWeapon;

        /// <summary>
        /// 移动速度
        /// </summary>
        protected int m_Speed = 3; //移动速度
        
        /// <summary>
        /// 阵营
        /// </summary>
        protected Camp m_Camp;
        public Camp Camp => m_Camp;

        private Transform m_Transform;

        protected virtual void Start()
        {
            m_Transform = transform;
        }
        
        protected abstract void Attack();

        public virtual void Wounded(WeaponBase damageSource)
        {
            //如果已经死了，就不要执行剩下的代码。防止异常
            if (m_Hp <= 0)
                return;
            var damage = damageSource.Atk;
            if (damage >= m_Shield)
            {
                damage = damage - m_Shield;
                m_Shield = 0;
            }
            else if (damage < m_Shield)
            {
                m_Shield -= damage;
                damage = 0;
            }

            m_Hp = m_Hp - damage < 0 ? 0 : m_Hp - damage;
            if (m_Hp == 0)
                Dead();
        }

        protected virtual void Dead()
        {
            GameEntry.ObjectPool.PushObj(gameObject);
            // Destroy(this.gameObject);
        }

        public void SetCurrentWeapon(WeaponBase weapon)
        {
            m_CurWeapon = weapon;
            //设置父对象和位置
            var weaponTransform= weapon.transform;
            weaponTransform.SetParent(transform.Find("Weapons").transform);
            weaponTransform.localPosition = Vector3.zero;
            //设置武器的拥有者
            weapon.SetOwner(this);
        }

        /// <summary>
        /// 实体朝向
        /// </summary>
        protected virtual void Aim(Vector3 targetPosWs)
        {
            var dir = targetPosWs - transform.position;
            
            if (dir.x < 0)
            {
                m_Transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (m_CurWeapon != null)
            {
                m_CurWeapon.Aim(dir);
            }
        }
    }

    public enum Camp
    {
        Player,
        Monster
    }
}