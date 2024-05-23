using PhantomDanmaku.Runtime.UI;
using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    public class GameStart : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // UIMgr.Instance.ShowPanel<StartPanel>("StartPanel");
            // SoundMgr.Instance.PlayBkMusic("");
            GameEntry.UI.Open<StartUIForm>(null);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}