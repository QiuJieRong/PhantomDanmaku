using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    public abstract class ItemBase : MonoBehaviour
    {
        public abstract void UseItem(Player user);
    }
}