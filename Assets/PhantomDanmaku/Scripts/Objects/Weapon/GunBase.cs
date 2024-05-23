using UnityEngine;
using UnityEngine.VFX;

namespace PhantomDanmaku.Runtime
{
    
    public class GunBase : WeaponBase
    {
        public GameObject bulletPrefab;
        public VisualEffect fireEffect;

        public override void Attack()
        {
            GameObject obj = GameEntry.ObjectPool.GetObj(bulletPrefab.name, bulletPrefab);
            BulletBase bullet = obj.GetComponent<BulletBase>();
            bullet.SetOwner(this);
            obj.transform.position = gameObject.transform.position;
            fireEffect.Play();
        }
    }

}