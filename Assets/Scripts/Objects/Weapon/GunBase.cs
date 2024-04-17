using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunBase : WeaponBase
{
    public GameObject bulletPrefab;
    public VisualEffect fireEffect;

    public override void Attack()
    {
        GameObject obj = PoolMgr.Instance.GetObj(bulletPrefab.name, bulletPrefab);
        BulletBase bullet = obj.GetComponent<BulletBase>();
        bullet.SetOwner(this);
        obj.transform.position = gameObject.transform.position;
        fireEffect.Play();
    }
}
