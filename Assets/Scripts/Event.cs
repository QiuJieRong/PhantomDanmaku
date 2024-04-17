using Cysharp.Threading.Tasks;
using Fungus;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhantomDanmaku.Config
{
    public class Event
    {
        public virtual async UniTask StartEvent()
        {
            
        }
    }

    [LabelText("开启一段流程")]
    public class StartFlowchart : Event
    {
        [LabelText("流程")]
        [DrawWithUnity]
        public AssetReferenceGameObject Flowchart;
        public override async UniTask StartEvent()
        {
            var handle = Flowchart.InstantiateAsync();
            await handle;
            var flowchart = handle.Result.GetComponent<Flowchart>();
            while (!flowchart.IsDestroyed())
            {
                await UniTask.DelayFrame(0);
            }
            Debug.Log("结束事件");
        }
    }
}