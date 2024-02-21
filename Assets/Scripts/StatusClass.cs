using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusClass : MonoBehaviour
{
    [SerializeField] int _defaultHP;
    public int DefaulrHP => _defaultHP;
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

    [SerializeField]
    Transform _insObjPoint;
    public Transform InsObjPoint => _insObjPoint;
    

    DataBase _dataBase;
    public DataBase DataBase => _dataBase;

    public Survive _survive = Survive.Survive;

    public bool _death;

    //[SerializeField] Transform _swordUpPrehab;
    //[SerializeField] Transform _swordDownPrehab;
    //[SerializeField] Transform _sheldUpPrehab;
    //[SerializeField] Transform _sheldDownPrehab;
    [SerializeField] GameObject  _conditionPanel;

    public void Awake()
    {
        _dataBase = DataBase.Instance;
    }

    private void OnEnable()
    {
        FightManager.OnEnterAction += ActionMode;
        FightManager.OnEnterRPG += RPGMode;
    }

    private void OnDisable()
    {
        FightManager.OnEnterAction -= ActionMode;
        FightManager.OnEnterRPG -= RPGMode;
    }

    public virtual void ActionMode() { }
    public virtual void RPGMode() { }
    /// <summary>バフの効果時間の処理</summary>
    public void TimeMethod()
    {
        //timeの変数が0より高い時、秒数をはかる。0になったらboolをtrueにしてコルーチンの待機処理を抜ける。
        if(_attackBuffTime > 0) { _attackBuffTime = DeltaTime(_attackBuffTime); }
        else if (!_buffAttack) { _buffAttack = true; }
        if(_diffenceBuffTime > 0) { _diffenceBuffTime = DeltaTime(_diffenceBuffTime); }
        else if (!_buffDiffence) { _buffDiffence = true; }
    }

    /// <summary>時間を減らしていく処理</summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float DeltaTime(float time)
    {
        return time - Time.deltaTime;
    }

    /// <summary>キャラの戦闘ステータスを設定する処理</summary>
    public void SetStatus()
    {
        _hp = _defaultHP;
        _attack = _defaultAttack;
        _diffence = _defaultDiffence;
    }

    /// <summary>普通にダメージを受けた時の処理</summary>
    /// <param name="damage"></param>
    /// <param name="skillParsent"></param>
    public void AddDamage(float damage,float skillParsent = 1)
    {
        _hp = Mathf.Max(0,_hp - (int)(damage * skillParsent - _diffence));
        ShowSlider();
    }

    /// <summary>特殊なダメージを受けた時の処理(防御貫通)</summary>
    /// <param name="damage"></param>
    /// <param name="skillParsent"></param>
    public void AddMagicDamage(float damage, float skillParsent = 1)
    {
        _hp = _hp - (int)(damage * skillParsent);
        ShowSlider();
    }

    /// <summary>デバフとダメージを受けた時の処理</summary>
    /// <param name="damage"></param>
    /// <param name="skillParsent"></param>
    /// <param name="attackDebuff"></param>
    /// <param name="diffenceDebuff"></param>
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

    /// <summary>バフを受けた時の処理</summary>
    /// <param name="attackBuff"></param>
    /// <param name="diffenceBuff"></param>
    /// <param name="heal"></param>
    public void AddBuff(int attackBuff, int diffenceBuff,int heal)
    {
        if (_attack <= _defaultAttack && attackBuff != 0)
        {
            StartCoroutine(AttackBuffTIme(attackBuff));
        }
        if (_diffence <= _defaultDiffence && diffenceBuff != 0)
        {
            StartCoroutine(DiffenceBuffTIme(diffenceBuff));
        }
        if (heal != 0 && _hp > 0)
        {
            _hp = Mathf.Min(_defaultHP, _hp + heal);
            ShowSlider();
        }
    }

    /// <summary>HPのゲージを更新する処理</summary>
    public void ShowSlider()
    {
        _hpSlider.value = (float)_hp / (float)_defaultHP;
    }

    /// <summary>攻撃のバフ、デバフを受けた時のコルーチン</summary>
    /// <param name="attackBuff"></param>
    /// <returns></returns>
    IEnumerator AttackBuffTIme(int attackBuff)
    {
        _attackBuffTime = 0;
        yield return null;
        _attack = Mathf.Max(1, _defaultAttack + attackBuff);
        Debug.Log(_attack);
        _attackBuffTime = 30f;
        _buffAttack = false;
        //ここで30秒経つか、ステータス更新(デバフならバフ、バフならデバフを受けた時)まで待機処理
        yield return new WaitUntil(() => _buffAttack == true);
        _attack = _defaultAttack;
        Debug.Log("攻撃状態変化解除");
    }

    /// <summary>防御のバフ、デバフを受けた時のコルーチン</summary>
    /// <param name="diffenceDebuff"></param>
    /// <returns></returns>
    IEnumerator DiffenceBuffTIme(int diffenceDebuff)
    {
        _diffenceBuffTime = 0;
        yield return null;
        _diffence = Mathf.Max(1, _defaultDiffence + diffenceDebuff);
        Debug.Log(_diffence);
        _diffenceBuffTime = 30f;
        _buffDiffence = false;
        //ここで30秒経つか、ステータス更新(デバフならバフ、バフならデバフを受けた時)まで待機処理
        yield return new WaitUntil(() => _buffDiffence == true);
        _diffence = _defaultDiffence;
        Debug.Log("防御状態変化解除");
    }

    /// <summary>生きているか死んでいるかのstate</summary>
    public enum Survive
    {
        Survive,
        Death,
    }
}
