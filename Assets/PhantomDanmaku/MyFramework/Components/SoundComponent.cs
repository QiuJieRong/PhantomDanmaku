using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyFramework.Runtime
{
    public class SoundComponent : GameFrameworkComponent
    {
        //背景音乐
        private AudioSource bkAudioSource;
        private float musicVolume = 1;

        public float MusicVolume
        {
            get => musicVolume;
            private set { }
        }

        private float soundVolume = 1;

        public float SoundVolume
        {
            get => soundVolume;
            private set { }
        }

        //音效组件依附的对象
        private GameObject m_SoundObj;

        //所有音效的容器
        private readonly List<AudioSource> m_Sounds = new List<AudioSource>();

        //每帧去清除播放完毕的AudioSource组件
        private void Update()
        {
            for (int i = 0; i < m_Sounds.Count; i++)
            {
                //如果音效播放结束，就停止播放，删除实例化的组件
                if (!m_Sounds[i].isPlaying)
                {
                    //停止播放音效，并删除组件,并从sounds列表里移除
                    StopSound(m_Sounds[i]);
                    i--;
                }
            }
        }


        /// <summary>
        /// 开启背景音乐
        /// </summary>
        /// <param name="name">背景音乐的名字,会从路径“Resources/Music/name”去找</param>
        public void PlayBkMusic(string name)
        {
            if (bkAudioSource == null)
            {
                var obj = new GameObject("BKMusic");
                bkAudioSource = obj.AddComponent<AudioSource>();
            }

            ResMgr.Instance.LoadAsync<AudioClip>("Music/" + name, (clip) =>
            {
                bkAudioSource.clip = clip;
                bkAudioSource.loop = true;
                bkAudioSource.Play();
            });
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void PauseBkMusic()
        {
            bkAudioSource.Pause();
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBkMusic()
        {
            bkAudioSource.Stop();
        }

        public void SetBkMusicVolume(float volume)
        {
            musicVolume = volume;
            if (bkAudioSource != null)
                bkAudioSource.volume = musicVolume;
        }

        public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback = null)
        {
            if (m_SoundObj == null)
            {
                m_SoundObj = new GameObject("Sound");
            }

            ResMgr.Instance.LoadAsync<AudioClip>("Sound/" + name, (clip) =>
            {
                AudioSource audioSource = m_SoundObj.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.volume = soundVolume;
                audioSource.loop = isLoop;
                audioSource.Play();
                m_Sounds.Add(audioSource);
                callback?.Invoke(audioSource);
            });
        }

        public void StopSound(AudioSource audioSource)
        {
            if (m_Sounds.Contains(audioSource))
            {
                m_Sounds.Remove(audioSource);
                audioSource.Stop();
                GameObject.Destroy(audioSource);
            }
        }

        public void SetSoundVolume(float volume)
        {
            soundVolume = volume;
            foreach (var audioSource in m_Sounds)
            {
                audioSource.volume = soundVolume;
            }
        }

        public void ClearSounds()
        {
            m_Sounds.Clear();
        }
    }
}