using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyFramework.Runtime
{
    public class ObjectPoolComponent : GameFrameworkComponent
    {
        private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

        //创建一个pool空对象在场景当中，放入对象池就作为子对象，拿出对象池就不作为子对象
        private GameObject poolObj;

        /// <summary>
        /// 从对象池里获取对象,如果没有该对象池或者对象数量为零，同步加载资源并返回实例化对象
        /// </summary>
        /// <param name="name">Resource文件夹下预设体的路径</param>
        /// <returns>返回值游戏对象</returns>
        public GameObject GetObj(string name)
        {
            GameObject obj = null;
            //如果字典里有这个类型的池子，并且数量大于0,就从这个池子里获取对象
            if (poolDic.ContainsKey(name) && poolDic[name].list.Count > 0)
            {
                obj = poolDic[name].GetObj();
            }
            //如果字典里没有这个键,或者数量小于0,就直接实例化对象返回
            else
            {
                obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
                obj.name = name;
            }

            return obj;
        }

        public GameObject GetObj(string name, GameObject prefab)
        {
            GameObject obj = null;
            //如果字典里有这个类型的池子，并且数量大于0,就从这个池子里获取对象
            if (poolDic.ContainsKey(name) && poolDic[name].list.Count > 0)
            {
                obj = poolDic[name].GetObj();
            }
            //如果字典里没有这个键,或者数量小于0,就直接实例化对象返回
            else
            {
                obj = GameObject.Instantiate(prefab);
                obj.name = name;
            }

            return obj;
        }

        /// <summary>
        /// 从对象池里获取对象,如果没有该对象池或者对象数量为零，异步加载资源并将实例化对象作为回调函数的参数传出去
        /// </summary>
        /// <param name="name">Resource文件夹下预设体的路径</param>
        /// <param name="callback">参数是实例化对象的回调函数</param>
        public void GetObjAsync(string name, UnityAction<GameObject> callback)
        {
            //如果字典里有这个类型的池子，并且数量大于0,就从这个池子里获取对象
            if (poolDic.ContainsKey(name) && poolDic[name].list.Count > 0)
            {
                //通过回调函数返回这个对象
                callback(poolDic[name].GetObj());
            }
            //如果字典里没有这个键,或者数量小于0,就直接实例化对象返回
            else
            {
                ResMgr.Instance.LoadAsync<GameObject>(name, (obj) =>
                {
                    obj.name = name;
                    //通过回调函数返回这个对象
                    callback(obj);
                });
            }
        }

        /// <summary>
        /// 将要不需要的对象放入对象池中
        /// </summary>
        /// <param name="name">Resources文件夹下预设体的路径</param>
        /// <param name="obj">要存放的不使用的对象</param>
        public void PushObj(GameObject obj)
        {
            //设置父对象
            if (poolObj == null)
                poolObj = new GameObject("Pool");

            //如果字典已经存在该对象的池子,就直接往池子里添加
            if (poolDic.ContainsKey(obj.name))
                poolDic[obj.name].PushObj(obj);
            //如果没有该池子，就new一个池子,使用构造函数将对象传入
            else
                poolDic.Add(obj.name, new PoolData(obj, poolObj));
        }

        /// <summary>
        /// 清空缓存池，用于场景切换后，对象池取消对这些被销毁的对象的引用
        /// </summary>
        public void Clear()
        {
            poolDic.Clear();
            poolObj = null;
        }
    }
    

    /// <summary>
    /// poolDic字典的值类型，包含挂载的父对象和所有存储的对象
    /// </summary>
    public class PoolData
    {
        //对象池中，对象的父节点
        public GameObject fatherObj;
        //该对象池装载对象的List
        public List<GameObject> list;

        public PoolData(GameObject obj, GameObject poolObj)
        {
            fatherObj = new GameObject(obj.name);
            fatherObj.transform.SetParent(poolObj.transform);
            list = new List<GameObject>();
            PushObj(obj);
        }
        public GameObject GetObj()
        {
            GameObject obj = list[0];
            list.RemoveAt(0);
            //激活
            obj.SetActive(true);
            //取消设置父对象
            obj.transform.parent = null;
            return obj;
        }

        public void PushObj(GameObject obj)
        {
            //往该对象池添加对象
            list.Add(obj);
            //设置父对象
            obj.transform.SetParent(fatherObj.transform, false);
            //失活
            obj.SetActive(false);
        }
    }
}