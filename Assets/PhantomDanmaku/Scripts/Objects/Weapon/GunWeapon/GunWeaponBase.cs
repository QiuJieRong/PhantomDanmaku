using UnityEngine;
using UnityEngine.VFX;

namespace PhantomDanmaku.Runtime
{
    
    public class GunWeaponBase : WeaponBase
    {
        public GameObject bulletPrefab;
        public VisualEffect fireEffect;

        public override void Attack()
        {
            base.Attack();
            Components.Sound.PlaySound("Fire", false);
            GameObject obj = Components.ObjectPool.GetObj(bulletPrefab.name, bulletPrefab);
            BulletBase bullet = obj.GetComponent<BulletBase>();
            bullet.SetOwner(this);
            obj.transform.position = gameObject.transform.position;
            fireEffect.Play();
        }
    }

}