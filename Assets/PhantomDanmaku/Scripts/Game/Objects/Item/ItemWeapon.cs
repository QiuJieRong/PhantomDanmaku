using UnityEngine;

namespace PhantomDanmaku
{
    
    public class ItemWeapon : ItemBase
    {
        public GameObject weaponPrefabs;
        public override void UseItem(Player owner)
        {
            //实例化武器
            GameObject weaponObj = Instantiate(weaponPrefabs);
            WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();
            //先判断是否已经拥有武器
            if (owner.CurrentWeapon != null)//如果已经拥有武器
            {
                //调用玩家的切换武器函数
                owner.ChangeCurrentWeapon(weapon);
            }
            else
            {
                //如果道具类型是武器，则装备武器。
                if (type == E_Item_Type.weapon)
                {
                    //设置Owner的当前武器
                    owner.SetCurrentWeapon(weapon);
                }
            }

            //销毁自己
            Destroy(gameObject);
        }
    }

}