using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

public class AudioManager : SingletonMonovihair<AudioManager>
{
    [Tooltip("マスター音量"), Header("初期マスター音量")]
    [SerializeField, Range(-10, -80)] float _masterVolume = -10;

    [Tooltip("BGMの音量"),Header("BGMの初期音量")]
    [SerializeField, Range(-10, -80)] float _bgmVolume = -10;

    [Tooltip("SEの音量"), Header("SEの初期音量")]
    [SerializeField, Range(-10, -80)] float _seVolume = -10;

    [Tooltip("BGM一覧")]
    [SerializeField] AudioSetting[] _bgmSetting;

    [Tooltip("SE一覧")]
    [SerializeField] AudioSetting[] _seSetting;

    [SerializeField] AudioMixer _audioMixer;

    protected override bool _dontDestroyOnLoad { get { return true; } }

    private void Start()
    {
        _audioMixer.SetFloat("MasterVol",_masterVolume);
        _audioMixer.SetFloat("BGMVol",_bgmVolume);
        _audioMixer.SetFloat("SEVol",_seVolume);
    }

    /// <summary>SEを再生するメソッド</summary>
    /// <param name="seState">指定したSEオーディオ</param>
    public void SEPlay(SE seState)
    {
        _seSetting[(int)seState]._audio.Play();
    }

    /// <summary>BGMを再生するメソッド</summary>
    /// <param name="bgmState">指定したBGMオーディオ</param>
    public void BGMPlay(BGM bgmState)
    {
        _bgmSetting[(int)bgmState]._audio.Play();
    }
}

/// <summary>音量をInspectorで個々に設定できるようにするclass</summary>
[Serializable]
public struct AudioSetting
{
    [Tooltip("AudioSource"),Header("AudioSourceを入れる")]
    [SerializeField] public AudioSource _audio;
    [Tooltip("音量"),Header("Audioの音量")]
    [SerializeField,Range(0f,1f)] public float _volume;
}

public enum SE
{
    Explosion,
    EnemyShot,
    Click,
    GetSkill,
    Equip,
    Win,
    Lose,
    ActionAttack,
    MagicianAttack,
    BlockerAttack,
    AttackerAttack,
}


public enum BGM
{
    Title,
    ActionPart,
    RPGPart,
    RPGBattle,
}
