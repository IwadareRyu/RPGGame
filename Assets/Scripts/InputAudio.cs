using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>配列のAudioSourceなら一括で入れることができるので
/// それを構造体の配列に入れるクラス</summary>
public class InputAudio : MonoBehaviour
{
    [SerializeField] AudioManager _audioManager;
    [ContextMenuItem("BGMInput", "BGMInput")]
    [SerializeField] AudioSource[] _bgmSource;
    [ContextMenuItem("SEInput", "SEInput")]
    [SerializeField] AudioSource[] _seSource;
    
    void BGMInput()
    {
        if(_audioManager == null) { _audioManager.GetComponent<AudioManager>(); }
        _audioManager.InputBGMSetting(_bgmSource);
        Debug.Log("BGMセット完了");
    }

    void SEInput()
    {
        if (_audioManager == null) { _audioManager.GetComponent<AudioManager>(); }
        _audioManager.InputSESetting(_seSource);
        Debug.Log("SEセット完了");
    }




}
