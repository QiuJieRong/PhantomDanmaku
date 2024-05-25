using UnityEngine;

namespace PhantomDanmaku.Runtime
{

    public class BulletBase : MonoBehaviour
    {
        public float speed = 5;
        public GameObject bulletEffect; //子弹销毁特效
        protected WeaponBase owner; //所属武器
        private Camp m_Camp; //阵营
        private Camp Camp => m_Camp;
        protected Rigidbody2D rig2D;
        protected Vector3 dir;

        public void SetOwner(WeaponBase weapon)
        {
            owner = weapon;
            m_Camp = owner.Camp;
            dir = owner.transform.up;
        }

        protected virtual void Start()
        {
            dir = owner.transform.up;
            rig2D = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            rig2D.velocity = dir * speed;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //如果是不同阵营或不是子弹则销毁自己,执行对象的受伤函数,创建特效
            if (
                ((1 << other.gameObject.layer) & (1 << LayerMask.NameToLayer(m_Camp.ToString()))) == 0 &&
                ((1 << other.gameObject.layer) & LayerMask.GetMask("Bullet")) == 0 &&
                ((1 << other.gameObject.layer) & LayerMask.GetMask("Item")) == 0
            )
            {
                EntityBase otherEntity;
                other.TryGetComponent(out otherEntity);
                if (otherEntity != null)
                {
                    //执行受伤函数的同时传入伤害来源的武器
                    otherEntity.Wounded(owner);
                }

                // Destroy(gameObject);
                Components.ObjectPool.PushObj(gameObject);
                //创建特效对象
                GameObject obj = Components.ObjectPool.GetObj("BulletEffect", bulletEffect);
                obj.transform.position = transform.position;
                //创建音效
                Components.Sound.PlaySound("Explosion", false);
            }
        }
    }

}