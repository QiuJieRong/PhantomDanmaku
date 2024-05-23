using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    /// <summary>
    /// 给予武器的交互物品
    /// </summary>
    public class ItemWeapon : ItemBase
    {
        public GameObject weaponPrefabs;
        public override void UseItem(Player user)
        {
            //实例化武器
            var weaponObj = Instantiate(weaponPrefabs);
            var weapon = weaponObj.GetComponent<WeaponBase>();
            //先判断是否已经拥有武器
            if (user.CurWeapon != null)//如果已经拥有武器
            {
                //调用玩家的切换武器函数
                user.ChangeCurrentWeapon(weapon);
            }
            else
            {
                //设置Owner的当前武器
                user.SetCurrentWeapon(weapon);
            }

            //销毁自己
            Destroy(gameObject);
        }
    }
}