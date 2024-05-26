using UnityEngine;
namespace PhantomDanmaku.Runtime
{
    public abstract class EntityBase : MonoBehaviour
    {
        /// <summary>
        /// 最大生命值
        /// </summary>
        protected float maxHp = 5;
        public float MaxHp => maxHp;
        /// <summary>
        /// 当前生命值
        /// </summary>
        protected float curHp;
        public float CurHp => curHp;
        /// <summary>
        /// 最大护盾值
        /// </summary>
        protected float maxShield = 3;
        public float MaxShield => maxShield;
        /// <summary>
        /// 当前护盾值
        /// </summary>
        protected float curShield;
        public float CurShield => curShield;
        
        /// <summary>
        /// 最大能量值
        /// </summary>
        protected float maxEnergy = 500;
        public float MaxEnergy => maxEnergy;
        /// <summary>
        /// 当前能量值
        /// </summary>
        protected float curEnergy;

        public float CurEnergy
        {
            get => curEnergy;
            set => curEnergy = Mathf.Max(value, 0);
        }

        private WeaponBase m_CurWeapon;

        public WeaponBase CurWeapon => m_CurWeapon;

        /// <summary>
        /// 移动速度
        /// </summary>
        protected float speed = 3; //移动速度
        
        /// <summary>
        /// 阵营
        /// </summary>
        protected Camp m_Camp;
        public Camp Camp => m_Camp;

        private Transform m_Transform;
        
        protected Rigidbody2D rig2D;

        protected virtual void Start()
        {
            m_Transform = transform;
            rig2D = GetComponent<Rigidbody2D>();
            InitStateValue();
        }

        /// <summary>
        /// 初始化数值
        /// </summary>
        protected virtual void InitStateValue()
        {
            curHp = maxHp;
            curShield = maxShield;
            curEnergy = maxEnergy;
        }
        
        protected abstract void Attack();

        public virtual void Wounded(WeaponBase damageSource)
        {
            //如果已经死了，就不要执行剩下的代码。防止异常
            if (curHp <= 0 || damageSource.owner.CurHp <= 0 || damageSource.Transform == null)
                return;
            var damage = damageSource.Atk;
            if (damage >= curShield)
            {
                damage = damage - curShield;
                curShield = 0;
            }
            else if (damage < curShield)
            {
                curShield -= damage;
                damage = 0;
            }

            curHp = curHp - damage < 0 ? 0 : curHp - damage;
            
            //进行击退效果结算
            var repel = damageSource.Repel;
            var dir = m_Transform.position - damageSource.transform.position;
            dir = dir.normalized;
            rig2D.AddForce(dir * repel);
            
            if (curHp == 0)
                Dead();
        }

        protected virtual void Dead()
        {
            // Components.ObjectPool.PushObj(gameObject);
            Destroy(gameObject);
        }

        public void SetCurrentWeapon(WeaponBase weapon)
        {
            m_CurWeapon = weapon;
            //设置父对象和位置
            var weaponTransform= weapon.transform;
            weaponTransform.SetParent(transform.Find("Weapon").transform);
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