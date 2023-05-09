using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusClass : MonoBehaviour
{
    [SerializeField] float _minHP;
    float _hp = 100;
    public float HP => _hp;

    [SerializeField] float _minAttack;
    float _attack = 10;
    public float Attack => _attack;

    [SerializeField] float _minDiffence;
    float _diffence = 5;
    public float Diffence => _diffence;
    [HideInInspector]
    public EnemyController _enemy;
    [SerializeField] 
    Slider _hpSlider;
    public void SetStatus()
    {
        _hp = _minHP;
        _attack = _minAttack;
        _diffence = _minDiffence;
    }

    public void AddDamage(float damage,float skillParsent = 1)
    {
        _hp = _hp - (damage * skillParsent - _diffence);
        ShowSlider();
    }

    public void AddMagicDamage(float damage, float skillParsent = 1)
    {
        _hp = _hp - damage * skillParsent;
        ShowSlider();
    }

    public void ShowSlider()
    {
        _hpSlider.value = _hp / _minHP;
    }
}
