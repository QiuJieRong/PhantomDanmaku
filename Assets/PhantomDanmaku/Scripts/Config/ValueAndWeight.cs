using Sirenix.OdinInspector;

namespace PhantomDanmaku.Config
{
    public class ValueAndWeight<T>
    {
        [LabelText("值")]
        public T Value;

        [LabelText("权重")]
        public int Weight;
    }
}