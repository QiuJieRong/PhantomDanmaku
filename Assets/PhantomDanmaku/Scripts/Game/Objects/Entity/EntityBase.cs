using UnityGameFramework.Runtime;

namespace PhantomDanmaku
{
    public abstract class EntityBase : EntityLogic
    {
        protected int hp = 5; //生命值
        public int Hp => hp;
        protected int curHp = 5; //当前生命值
        public int CurHp => curHp; //当前生命值
        protected int shield = 3; //护盾值
        public int Shield => shield; //
        protected int curShield = 3; //当前护盾值
        public int CurShield => curShield;
        protected int energy = 100; //能量值
        public int Energy => energy; //
        protected int curEnergy = 100; //当前能量值
        public int CurEnergy => curEnergy;

        protected int speed = 3; //移动速度
        protected string camp;
        public string Camp => camp;

        protected abstract void Attack();

        public virtual void Wounded(WeaponBase damageSource)
        {
            int damage = damageSource.Atk;
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
            if (curHp == 0)
                Dead();
        }

        public virtual void Dead()
        {
            Destroy(gameObject);
            // GameEntry.Entity.HideEntity(Entity);
        }

        public abstract void SetCurrentWeapon(WeaponBase weapon);
    }

}