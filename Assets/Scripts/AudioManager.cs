using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

[RequireComponent(typeof(InputAudio))]
public class AudioManager : SingletonMonovihair<AudioManager>
{
    [Tooltip("マスター音量"), Header("初期マスター音量")]
    [SerializeField, Range(-10, -80)] float _masterVolume = -10;

    [Tooltip("BGMの音量"),Header("BGMの初期音量")]
    [SerializeField, Range(-10, -80)] float _bgmVolume = -10;

    [Tooltip("SEの音量"), Header("SEの初期音量")]
    [SerializeField, Range(-10, -80)] float _seVolume = -10;

    [ContextMenuItem("SetVolumeBGM","SetVolumeBGM")]
    [Tooltip("BGM一覧")]
    [SerializeField] AudioSettingStruct[] _bgmSetting;

    [ContextMenuItem("SetVolumeSE", "SetVolumeSE")]
    [Tooltip("SE一覧")]
    [SerializeField] AudioSettingStruct[] _seSetting;

    [SerializeField] AudioMixer _audioMixer;

    AudioSettingStruct _nowBGM;

    [SerializeField] float _changeBGMDurationTime = 1f;

    [SerializeField] bool _startSetBGM = false;

    protected override bool _dontDestroyOnLoad { get { return true; } }

    private void Start()
    {
        _audioMixer.SetFloat("MasterVol",_masterVolume);
        _audioMixer.SetFloat("BGMVol",_bgmVolume);
        _audioMixer.SetFloat("SEVol",_seVolume);
        _nowBGM = new();
        if (_startSetBGM) { BGMPlay(BGM.Title); }
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
        if (_nowBGM._audio != _bgmSetting[(int)bgmState]._audio)
        {
            _bgmSetting[(int)bgmState]._audio.Play();


            if (_nowBGM._audio == null)
            {
                _nowBGM = _bgmSetting[(int)bgmState];
                _nowBGM._audio.volume = _nowBGM._volume;
            }   // 現在流しているBGMがない場合、普通に再生。
            else
            {
                ChangeAudioPlay(bgmState);
            }   //現在流しているBGMがある場合、BGMを切り替えるメソッドを呼び出す。
        }
    }

    /// <summary>BGMをフェードイン、フェードアウトして切り替えるメソッド。</summary>
    /// <param name="bgmState"></param>
    void ChangeAudioPlay(BGM bgmState)
    {
            var beforeBGM = _nowBGM;
            _nowBGM = _bgmSetting[(int)bgmState];
            _nowBGM._audio.volume = 0f;
            var bgmSeq = DOTween.Sequence();
            bgmSeq.Append(_nowBGM._audio.DOFade(_nowBGM._volume, _changeBGMDurationTime))
                  .Join(beforeBGM._audio.DOFade(0f, _changeBGMDurationTime));
            bgmSeq.Play().SetLink(gameObject).OnComplete(() => beforeBGM._audio.Stop());
    }

    public void StopBGM()
    {
        // tmpBGMに_nowBGMを代入することで、別でPlayBGMが呼ばれたときに誤作動が起こらないようにしている。
        var tmpBGM = _nowBGM;
        _nowBGM._audio = null;
        tmpBGM._audio.DOFade(0f, _changeBGMDurationTime)
            .OnComplete(() =>
            {
                tmpBGM._audio.Stop();
            }).SetLink(gameObject);
    }


    /// <summary>_bgmSettingにAudioSourceを入れるメソッド</summary>
    /// <param name="audios"></param>
    public void InputBGMSetting(AudioSource[] audios)
    {
        _bgmSetting = new AudioSettingStruct[audios.Length];
        _bgmSetting = InputAudioSetting(audios,_bgmSetting);
    }

    /// <summary>_seSettingにAudioSourceを入れるメソッド</summary>
    /// <param name="audios"></param>
    public void InputSESetting(AudioSource[] audios)
    {
        _seSetting = new AudioSettingStruct[audios.Length];
        _seSetting = InputAudioSetting(audios,_seSetting);
    }

    /// <summary>AudioSettingStructにAudioSourceのクラスとボリュームの値を入れるメソッド</summary>
    /// <param name="audios"></param>
    /// <param name="audioSettings"></param>
    AudioSettingStruct[] InputAudioSetting(AudioSource[] audios,AudioSettingStruct[] audioSettings)
    {
        for(var i = 0;i < audios.Length;i++)
        {
            audioSettings[i]._audio = audios[i];
            audioSettings[i]._volume = audios[i].volume;
        }
        return audioSettings;
    }

    /// <summary>_bgmSettingのボリュームをAudioSourceに反映させるメソッド</summary>
    void SetVolumeBGM()
    {
        for(var i = 0;i < _bgmSetting.Length;i++)
        {
            _bgmSetting[i]._audio.volume = _bgmSetting[i]._volume;
        }
    }

    /// <summary>_seSettingのボリュームをAudioSourceに反映させるメソッド</summary>
    void SetVolumeSE()
    {
        for (var i = 0; i < _seSetting.Length; i++)
        {
            _seSetting[i]._audio.volume = _seSetting[i]._volume;
        }
    }
}

/// <summary>AudioSourceのクラスとAudioの音量をいれる構造体</summary>
[Serializable]
public struct AudioSettingStruct
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
    EnemyShordAttack,
    ActionHitBullet
}


public enum BGM
{
    Title,
    ActionPart,
    RPGPart,
    RPGBattle,
}
