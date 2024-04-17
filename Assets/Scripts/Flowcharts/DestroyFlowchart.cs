using Fungus;

namespace PhantomDanmaku.Scripts.Flowcharts
{
    [CommandInfo("Custom","DestroyFlowchart","销毁流程对象")]
    public class DestroyFlowchart : Command
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Destroy(gameObject);
            Continue();
        }
    }
}