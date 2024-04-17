using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhantomDanmaku
{
    
    public enum E_Item_Type
    {
        weapon,
        itme
    }

    public abstract class ItemBase : MonoBehaviour
    {
        public E_Item_Type type;
        public abstract void UseItem(Player owner);
    }

}