using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhantomDanmaku
{
    
    public class NormalGun : GunBase
    {
        void Start()
        {
            atk = 1;
        }


        public override void Attack()
        {
            base.Attack();
        }
    }

}