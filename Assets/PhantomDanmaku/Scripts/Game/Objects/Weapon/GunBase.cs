using UnityEngine;
using UnityEngine.VFX;

namespace PhantomDanmaku
{
    
    public class GunBase : WeaponBase
    {
        public GameObject bulletPrefab;
        public VisualEffect fireEffect;

        public override void Attack()
        {
            GameEntry.Entity.ShowEntity<GuidedBullet>(GameEntry.Entity.GenerateSerialId(),
                "Assets/PhantomDanmaku/Prefabs/Bullets/GuidedBullet.prefab","Bullet",this);
            fireEffect.Play();
        }
    }

}