using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 使用SoundMgr需要将音乐资源和音效资源分别放在Resources文件夹下的Music和Sound下面
/// 加载的时候只用传资源的文件名字即可
/// </summary>
public class SoundMgr : SingletonBase<SoundMgr>
{
    //背景音乐
    private AudioSource bkAudioSource;
    private float musicVolume = 1;
    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        private set { }
    }

    private float soundVolume = 1;
    public float SoundVolume
    {
        get
        {
            return soundVolume;
        }
        private set { }
    }

    //音效组件依附的对象
    private GameObject soundObj;
    //所有音效的容器
    private List<AudioSource> sounds = new List<AudioSource>();

    //构造函数里添加帧更新监听
    public SoundMgr()
    {
        MonoMgr.Instance.AddUpdateListener(Update);
    }
    //每帧去清除播放完毕的AudioSource组件
    private void Update()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            //如果音效播放结束，就停止播放，删除实例化的组件
            if (!sounds[i].isPlaying)
            {
                //停止播放音效，并删除组件,并从sounds列表里移除
                StopSound(sounds[i]);
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
            GameObject obj = new GameObject("BKMusic");
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
        if (soundObj == null)
        {
            soundObj = new GameObject("Sound");
        }
        ResMgr.Instance.LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = soundVolume;
            audioSource.loop = isLoop;
            audioSource.Play();
            sounds.Add(audioSource);
            callback?.Invoke(audioSource);
        });
    }

    public void StopSound(AudioSource audioSource)
    {
        if (sounds.Contains(audioSource))
        {
            sounds.Remove(audioSource);
            audioSource.Stop();
            GameObject.Destroy(audioSource);
        }
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        foreach (AudioSource audioSource in sounds)
        {
            audioSource.volume = soundVolume;
        }
    }

    public void ClearSounds()
    {
        sounds.Clear();
    }

}
