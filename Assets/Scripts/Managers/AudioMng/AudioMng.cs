using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMng : MonoSingleton<AudioMng>
{

    public AudioSource BgmPlayer;
    public AudioSource EffectPlayer;

    /// <summary>
    /// 背景音乐音量（0-10）
    /// </summary>
    public int BgmVolumn
    {
        get
        {
            return (int)(BgmPlayer.volume * 10);
        }
        set
        {
            if (!ValidateInput(value))
            {
                return;
            }
            BgmPlayer.volume = (float)value * 0.1f;
        }
    }

    /// <summary>
    /// 音效音量（0-10）
    /// </summary>
    public int EffectVolumn
    {
        get
        {
            return (int)(EffectPlayer.volume * 10);
        }
        set
        {
            if (!ValidateInput(value))
            {
                return;
            }
            EffectPlayer.volume = (float)value * 0.1f;
        }
    }

    public void Init()
    {
        BgmPlayer = gameObject.CreateLink<AudioSource>("BgmPlayer");
        BgmPlayer.loop = true;

        EffectPlayer = gameObject.CreateLink<AudioSource>("EffectPlayer");
        EffectPlayer.loop = false;
    }

    public void PlayBgm(MusicEnum music)
    {
        var clip = GetAudioClipByPath(music);
        BgmPlayer.clip = clip;
        BgmPlayer.Play();
    }

    public void PlayEffect(MusicEnum music)
    {
        var clip = GetAudioClipByPath(music);
        Debug.Log($"playEffect {clip}");
        EffectPlayer.clip = clip;
        EffectPlayer.Play();
    }

    private AudioClip GetAudioClipByPath(MusicEnum music)
    {
        string path = music.ToString();
        return Loader.LoadAudioClip(path);
    }

    private bool ValidateInput(int input)
    {
        if (input < 0 || input > 10)
        {
            return false;
        }
        return true;
    }

    protected override void Construct()
    {
        Init();
    }

    protected override void Release()
    {
    }
}
