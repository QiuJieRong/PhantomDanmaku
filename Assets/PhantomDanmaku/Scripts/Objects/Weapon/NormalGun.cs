namespace PhantomDanmaku.Runtime
{
    
    public class NormalGun : GunBase
    {
        protected override void Start()
        {
            base.Start();
            atk = 1;
        }

        public override void Attack()
        {
            base.Attack();
        }
    }

}