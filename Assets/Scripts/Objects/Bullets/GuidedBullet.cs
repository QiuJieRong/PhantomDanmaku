using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhantomDanmaku
{
    
    public class GuidedBullet : BulletBase
    {
        protected override void Update()
        {
            dir = owner.transform.right;
            base.Update();
        }
    }

}