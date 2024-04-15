using UnityEngine;

namespace PhantomDanmaku
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [HideInInspector]
        public EntityBase owner;
        protected string camp;
        public string Camp => camp;
        protected int atk;//攻击力
        public int Atk => atk;
        protected int interval;//攻击间隔
        protected int consume;//攻击能量消耗

        public void SetOwner(EntityBase entity)
        {
            owner = entity;
            camp = entity.Camp;
        }

        /// <summary>
        /// 武器的朝向也决定了角色的朝向
        /// </summary>
        public void Ami(Vector3 targetPosWS)
        {
            Vector3 dir = targetPosWS - transform.position;
            if (dir.x < 0)
            {
                transform.localScale = new Vector3(1, -1, 1);
                owner.transform.localScale = new Vector3(-1, 1, 1);
                transform.parent.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                owner.transform.localScale = new Vector3(1, 1, 1);
                transform.parent.transform.localScale = new Vector3(1, 1, 1);
            }

            dir = Quaternion.AngleAxis(90, Vector3.forward) * dir;

            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }
        public abstract void Attack();

    }
}