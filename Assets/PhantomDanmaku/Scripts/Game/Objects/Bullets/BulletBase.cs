using UnityEngine;
using UnityGameFramework.Runtime;

namespace PhantomDanmaku
{
    public class BulletBase : EntityLogic
    {
        public float speed = 3;
        public GameObject bulletEffect; //子弹销毁特效
        protected WeaponBase owner; //所属武器
        private string camp; //阵营
        private string Camp => camp;
        private Rigidbody2D rig;
        protected Vector3 dir;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData is WeaponBase weaponBase)
            {
                SetOwner(weaponBase);
            }
        }

        private void SetOwner(WeaponBase weapon)
        {
            owner = weapon;
            camp = owner.Camp;
            dir = owner.transform.right;
            gameObject.transform.position = weapon.gameObject.transform.position;
        }

        protected virtual void Start()
        {
            dir = owner.transform.right;
            rig = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            rig.velocity = dir * speed;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //如果是不同阵营或不是子弹则销毁自己,执行对象的受伤函数,创建特效
            if (
                ((1 << other.gameObject.layer) & (1 << LayerMask.NameToLayer(camp))) == 0 &&
                ((1 << other.gameObject.layer) & LayerMask.GetMask("Bullet")) == 0
            )
            {
                EntityBase otherEntity;
                other.TryGetComponent(out otherEntity);
                if (otherEntity != null)
                {
                    //执行受伤函数的同时传入伤害来源的武器
                    otherEntity.Wounded(owner);
                }

                GameEntry.Entity.HideEntity(Entity);
                
                //创建特效对象
                // GameObject obj = PoolMgr.Instance.GetObj("BulletEffect", bulletEffect);
                // obj.transform.position = transform.position;
                //创建音效
                // SoundMgr.Instance.PlaySound("Explosion", false);
            }
        }
    }
}