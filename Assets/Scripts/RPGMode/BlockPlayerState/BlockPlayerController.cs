using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlockPlayerController : StatusClass
{

    [Tooltip("ゲージアタックの値")]
    [SerializeField]
    public float _guageAttack = 0;
    [Tooltip("ディフェンス職の状態"), SerializeField]
    BlockState _conditionState = BlockState.Attack;
    public BlockState Condition => _conditionState;
    [Tooltip("移動の際止まる力")]
    float _stopdis = 0.1f;
    [Tooltip("移動スピード")]
    float _speed = 6f;
    [Tooltip("Blockの際、コルーチンを適切に動かすためのbool")]
    bool _blockTime;
    [Tooltip("Attackの際、コルーチンを適切に動かすためのbool")]
    bool _attackTime;
    [Tooltip("Counterの際、コルーチンを適切に動かすためのbool")]
    bool _counterTime;
    [Tooltip("CoolTimeの際、コルーチンを適切に動かすためのbool")]
    bool _coolTimebool;
    [Tooltip("状態のテキスト")]
    [SerializeField] Text _enumtext;
    [SerializeField] GameObject _blockObj;
    [SerializeField] GetOutScripts _getOutPrefab;
    GetOutScripts _getOutObj;
    [SerializeField] Transform _getInPos;
    [SerializeField] Animator _blockAnim;
    [SerializeField] Canvas _statusCanvas;
    [Tooltip("BlockPlayerの移動場所")]
    [SerializeField]
    Transform[] _trans;
    bool _isCounter;
    public bool IsCounter => _isCounter;

    /// <summary>StateMachine用変数</summary>
    [Tooltip("State一覧")]
    IRPGState[] _states;
    [SerializeField] AttackStateBlock _attackState;
    [SerializeField] ChageAttackState _chageAttackState;
    [SerializeField] CoolCounterState _coolCounterState = new();
    [SerializeField] GuardState _guardState = new();
    [SerializeField] CounterAttackState _counterAttackState = new();
    public AttackStateBlock AttackState => _attackState;
    public ChageAttackState ChageAttackState => _chageAttackState;
    public CoolCounterState CoolCounterState => _coolCounterState;
    public GuardState GuardState => _guardState;
    public CounterAttackState CounterAttackState => _counterAttackState;

    [Tooltip("現在のState")]
    IRPGState _currentState;
    public IRPGState CurrentState => _currentState;
    ///

    public TargetGuard _targetGuard = TargetGuard.None;

    public DataBase _dataBase;

    private void Start()
    {
        _dataBase = DataBase.Instance;
        _enemy = GameObject.FindGameObjectWithTag("RPGEnemy")?.GetComponent<EnemyController>();
        SetStatus();
        ShowSlider();
        Debug.Log($"BlockerHP:{HP}");
        Debug.Log($"BlockerAttack:{Attack}");
        Debug.Log($"BlockerDiffence:{Diffence}");
        _states = new IRPGState[1] { _attackState };
        foreach(var state in _states)
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
        if (_currentState ==  CoolCounterState)
        {
            _isCounter = true;
            OnChangeState(CounterAttackState);
        }
    }

}
