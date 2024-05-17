using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [Header("モデルの状態を表示するテキスト")]
    [SerializeField] Text _conditionText;
    [Header("HPを表示するスライダー")]
    [SerializeField] Slider _hpSlider;
    [Header("スキルの詠唱時間を可視化するスライダー")]
    [SerializeField] Slider _chantingSlider;
    [Header("Attackerのみアタッチ")]
    [Header("コマンドのクールタイムを可視化するスライダー")]
    [SerializeField] Slider _commandCoolTimeSlider;

    public void ConditionTextView(string str)
    {
        _conditionText.text = str;
    }

    /// <summary>HPのゲージを更新する処理</summary>
    public void HPView(float currentHP,float maxHP)
    {
        SetSlider(_hpSlider, currentHP, maxHP);
    }

    /// <summary>攻撃の詠唱時間ゲージを更新する処理</summary>
    public void ChantingView(float currentChanting,float maxChanting)
    {
        SetSlider(_chantingSlider, currentChanting, maxChanting);
    }

    /// <summary>コマンドのゲージを更新する処理</summary>
    public void CommandView(float currentCommandTime,float maxCommandTime)
    {
        SetSlider(_commandCoolTimeSlider, currentCommandTime, maxCommandTime);
    }

    public void SetSlider(Slider setSlider, float currentNumber,float maxNumber)
    {
        setSlider.value = currentNumber / maxNumber;
    }
}
