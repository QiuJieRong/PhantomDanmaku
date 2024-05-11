using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyFramework.Runtime
{
    public static class GameEntry
    {
        private static readonly List<GameFrameworkComponent> s_GameFrameworkComponents = new List<GameFrameworkComponent>();

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架组件类型。</typeparam>
        /// <returns>要获取的游戏框架组件。</returns>
        public static T GetComponent<T>() where T : GameFrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <param name="type">要获取的游戏框架组件类型。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static GameFrameworkComponent GetComponent(Type type)
        {
            foreach (var gameFrameworkComponent in s_GameFrameworkComponents)
            {
                if (gameFrameworkComponent.GetType() == type)
                {
                    return gameFrameworkComponent;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <param name="typeName">要获取的游戏框架组件类型名称。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static GameFrameworkComponent GetComponent(string typeName)
        {
            foreach (var gameFrameworkComponent in s_GameFrameworkComponents)
            {
                if (gameFrameworkComponent.GetType().FullName == typeName)
                {
                    return gameFrameworkComponent;
                }
            }

            return null;
        }
        public static void RegisterComponent(GameFrameworkComponent gameFrameworkComponent)
        {
            if (gameFrameworkComponent == null)
            {
                Debug.LogError("框架组件为空");
                return;
            }

            if (s_GameFrameworkComponents.Contains(gameFrameworkComponent))
            {
                Debug.LogError("框架组件已注册");
            }
            
            s_GameFrameworkComponents.Add(gameFrameworkComponent);
        }
    }
}