using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusClass : MonoBehaviour
{
    [SerializeField] int _defaultHP;
    int _hp = 100;
    public int HP => _hp;

    [SerializeField] int _defaultAttack;
    int _attack = 10;
    float _attackBuffTime = 0;
    bool _buffAttack;

    public int Attack => _attack;

    [SerializeField] int _defaultDiffence;
    int _diffence = 5;
    float _diffenceBuffTime = 0;
    bool _buffDiffence;

    public int Diffence => _diffence;
    [HideInInspector]
    public EnemyController _enemy;
    [SerializeField]
    Slider _hpSlider;

    DataBase _dataBase;
    public DataBase DataBase => _dataBase;

    public void Awake()
    {
        _dataBase = DataBase.Instance;
    }

    private void Update()
    {
        if(_attackBuffTime > 0) { _attackBuffTime = DeltaTime(_attackBuffTime); }
        else if (!_buffAttack) { _buffAttack = true; }
        if(_diffenceBuffTime > 0) { _diffenceBuffTime = DeltaTime(_diffenceBuffTime); }
        else if (!_buffDiffence) { _buffDiffence = true; }

    }

    public float DeltaTime(float time)
    {
        return time - Time.deltaTime;
    }

    public void SetStatus()
    {
        _hp = _defaultHP;
        _attack = _defaultAttack;
        _diffence = _defaultDiffence;
    }

    public void AddDamage(float damage,float skillParsent = 1)
    {
        _hp = _hp - (int)(damage * skillParsent - _diffence);
        ShowSlider();
    }

    public void AddMagicDamage(float damage, float skillParsent = 1)
    {
        _hp = _hp - (int)(damage * skillParsent);
        ShowSlider();
    }

    public void AddDebuffDamage(float damage, float skillParsent = 1,int attackDebuff = 0,int diffenceDebuff = 0)
    {
        _hp = _hp - (int)(damage * skillParsent);
        ShowSlider();
        if (_attack >= _defaultAttack && attackDebuff != 0)
        {
            StartCoroutine(AttackBuffTIme(attackDebuff));
        }
        if(_diffence >= _defaultDiffence && diffenceDebuff != 0)
        {
            StartCoroutine(DiffenceBuffTIme(diffenceDebuff));
        }
    }

    public void AddBuff(int attackBuff, int diffenceBuff)
    {
        if (_attack <= _defaultAttack && attackBuff != 0)
        {
            StartCoroutine(AttackBuffTIme(attackBuff));
        }
        if (_diffence <= _defaultDiffence && diffenceBuff != 0)
        {
            StartCoroutine(DiffenceBuffTIme(diffenceBuff));
        }
    }

    public void ShowSlider()
    {
        _hpSlider.value = _hp / _defaultHP;
    }

    IEnumerator AttackBuffTIme(int attackBuff)
    {
        _attackBuffTime = 0;
        yield return null;
        _attack = Mathf.Max(1, _defaultAttack + attackBuff);
        Debug.Log(_attack);
        _attackBuffTime = 30f;
        _buffAttack = false;
        yield return new WaitUntil(() => _buffAttack == true);
        _attack = _defaultAttack;
        Debug.Log("UŒ‚ó‘Ô•Ï‰»‰ğœ");
    }
    IEnumerator DiffenceBuffTIme(int diffenceDebuff)
    {
        _diffenceBuffTime = 0;
        yield return null;
        _diffence = Mathf.Max(1, _defaultDiffence + diffenceDebuff);
        Debug.Log(_diffence);
        _diffenceBuffTime = 30f;
        _buffDiffence = false;
        yield return new WaitUntil(() => _buffDiffence == true);
        _diffence = _defaultDiffence;
        Debug.Log("–hŒäó‘Ô•Ï‰»‰ğœ");
    }
}
