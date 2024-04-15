namespace PhantomDanmaku
{
    
    public class GuidedBullet : BulletBase
    {
        protected override void Update()
        {
            if (owner != null)
            {
                dir = owner.transform.right;
            }
            base.Update();
        }
    }

}