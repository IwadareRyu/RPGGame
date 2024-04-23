using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AttackStateBlock))]
[RequireComponent(typeof(ChargeAttackState))]

public class BlockPlayerController : StatusClass
{
    [Header("チャージ攻撃関連")]
    [Tooltip("チャージ攻撃の値")]
    public float _guageAttack = 0;
    float _maxGuageAttack;
    public float MaxGuageAttack => _maxGuageAttack;
    [Tooltip("ディフェンス職の状態"),Header("ディフェンス職の状態")]
    [SerializeField] BlockState _conditionState = BlockState.Attack;
    public BlockState Condition => _conditionState;
    [Tooltip("状態のテキスト"),Header("プレイヤーの状態を表示するテキスト")]
    [SerializeField] Text _enumtext;
    [Header("プレイヤーのアニメーション")]
    [SerializeField] Animator _blockAnim;
    [Tooltip("BlockPlayerの移動場所"),Header("BlockPlayerの移動場所")]
    public Transform[] _trans;
    bool _isCounter;
    public bool IsCounter => _isCounter;

    /// <summary>StateMachine用変数</summary>

    [Tooltip("State一覧")]
    IRPGState[] _states;
    [Header("State関連")]
    [Tooltip("ガード終了後、クールダウンのState")]
    [SerializeField] CoolDownState _coolDownState = new();
    [Tooltip("通常攻撃のState")]
    AttackStateBlock _attackState;
    [Tooltip("チャージ攻撃のState")]
    ChargeAttackState _chageAttackState;
    [Tooltip("カウンターのクールタイムのState")]
    [SerializeField] CoolCounterState _coolCounterState = new();
    [Tooltip("味方をガードするState")]
    GuardState _guardState = new();
    [Tooltip("カウンター攻撃をするState")]
    [SerializeField] CounterAttackState _counterAttackState = new();
    [Tooltip("戦闘不能のState")]
    DeathStateBlock _deathState = new();
    
    public CoolDownState CoolDownState => _coolDownState;
    public AttackStateBlock AttackState => _attackState;
    public ChargeAttackState ChageAttackState => _chageAttackState;
    public CoolCounterState CoolCounterState => _coolCounterState;
    public GuardState GuardState => _guardState;
    public CounterAttackState CounterAttackState => _counterAttackState;
    public DeathStateBlock DeathState => _deathState;

    [Tooltip("現在のState")]
    IRPGState _currentState;
    public IRPGState CurrentState => _currentState;
    ///

    [NonSerialized] public TargetGuard _targetGuard = TargetGuard.None;

    [NonSerialized] public DataBase _dataBase;

    private void Start()
    {
        _dataBase = DataBase.Instance;
        _enemy = GameObject.FindGameObjectWithTag("RPGEnemy")?.GetComponent<EnemyController>();
        _attackState = GetComponent<AttackStateBlock>();
        _chageAttackState = GetComponent<ChargeAttackState>();
        SetStatus();
        ShowSlider();
        Debug.Log($"BlockerHP:{HP}");
        Debug.Log($"BlockerAttack:{Attack}");
        Debug.Log($"BlockerDiffence:{Diffence}");
        _states = new IRPGState[7] { _coolDownState, _attackState, _chageAttackState, 
            _coolCounterState, _counterAttackState, 
            _guardState, _deathState };
        foreach (var state in _states)
        {
            state.Init(this);
        }
        _currentState = _attackState;
        _currentState.StartState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState(this);

        if(HP <= 0) { OnChangeState(DeathState); }
    }

    /// <summary>今のStateを変える時に呼ぶメソッド</summary>
    /// <param name="state"></param>
    public void OnChangeState(IRPGState state)
    {
        _currentState.EndState(this);
        _currentState = state;
        _currentState.StartState(this);
    }


    /// <summary>テキストの更新の処理</summary>
    /// <param name="str"></param>
    public void ShowText(string str)
    {
        _enumtext.text = str;
    }

    /// <summary>カウンター成功時に呼ばれるメソッド</summary>
    public void TrueCounter()
    {
        if (_currentState == CoolCounterState)
        {
            _isCounter = true;
            OnChangeState(CounterAttackState);
        }
    }

    public void EndCounter()
    {
        _isCounter = false;
        OnChangeState(GuardState);
    }

}
