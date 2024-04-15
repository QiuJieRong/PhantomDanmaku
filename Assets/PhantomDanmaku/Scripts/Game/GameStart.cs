using UnityEngine;

namespace PhantomDanmaku
{
    
    public class GameStart : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameEntry.UI.OpenUIForm("Assets/PhantomDanmaku/UI/StartPanel.prefab","Default");
            // SoundMgr.Instance.PlayBkMusic("");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}