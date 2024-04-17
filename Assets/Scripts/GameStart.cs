using UnityEngine;

namespace PhantomDanmaku
{
    public class GameStart : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            UIMgr.Instance.ShowPanel<StartPanel>("StartPanel");
            SoundMgr.Instance.PlayBkMusic("");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}