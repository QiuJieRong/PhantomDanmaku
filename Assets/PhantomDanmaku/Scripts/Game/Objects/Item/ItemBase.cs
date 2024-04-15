using UnityGameFramework.Runtime;

namespace PhantomDanmaku
{
    
    public enum E_Item_Type
    {
        weapon,
        itme
    }

    public abstract class ItemBase : EntityLogic
    {
        public E_Item_Type type;
        public abstract void UseItem(Player owner);
    }

}