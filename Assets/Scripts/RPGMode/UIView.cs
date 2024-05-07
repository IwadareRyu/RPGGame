using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _chantingSlider;

    /// <summary>HPのゲージを更新する処理</summary>
    public void HPView(float currentHP,float MaxHP)
    {
        _hpSlider.value = currentHP / MaxHP;
    }

    /// <summary>攻撃の詠唱時間ゲージを更新する処理</summary>
    public void ChantingView(float currentChanting,float maxChanting)
    {
        _chantingSlider.value = currentChanting / maxChanting;
    }
}
