using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    [Serializable]
    public class StateValue
    {
        [SerializeField]
        private State m_State;

        public State State => m_State;

        [SerializeField]
        private float m_Value;

        public float Value
        {
            get
            {
                var ratio = 0f;
                foreach (var modifier in m_Modifiers)
                {
                    if (modifier.State == m_State)
                    {
                        ratio += modifier.Ratio;
                    }
                }

                return m_Value * (1 + ratio);
            }
        }

        [SerializeField]
        private List<Modifier> m_Modifiers;

        /// <summary>
        /// 为该属性添加修正器，直接传入，内部拷贝了
        /// </summary>
        /// <param name="modifier"></param>
        public void AddModifier(Modifier modifier)
        {
            m_Modifiers.Add(modifier.Copy());
        }
    }

    [Serializable]
    public enum State
    {
        [LabelText("最大血量")]
        MaxHp,
        [LabelText("最大护盾")]
        MaxShield,
        [LabelText("最大能量")]
        MaxEnergy,
        [LabelText("速度")]
        Speed,
    }

    /// <summary>
    /// 属性修正器
    /// </summary>
    [Serializable]
    public class Modifier
    {
        [SerializeField]
        private State m_State;

        public State State => m_State;

        [LabelText("增加或减少比例")]
        [SerializeField]
        private float m_Ratio;

        public float Ratio => m_Ratio;

        public Modifier Copy()
        {
            return new Modifier()
            {
                m_State = m_State,
                m_Ratio = m_Ratio
            };
        }
    }
}