namespace PhantomDanmaku.Runtime
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