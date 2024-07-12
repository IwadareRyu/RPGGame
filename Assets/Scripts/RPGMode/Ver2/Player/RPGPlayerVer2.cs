using RPGBattle;
using System;
using UnityEngine;

public class RPGPlayerVer2 : StatusClass
{
    DataBase _dataBase;

    [NonSerialized]
    public SkillInfomation _useSkill;

    /// <summary>選んだスキル</summary>
    public ChoiceSkill _choiceSkill;

    /// <summary>現在のスキル</summary>
    IRPGStateVer2 _currentRPGState;

    /// <summary>クールタイムのState</summary>
    CoolDownStateVer2 _coolDownState = new();
    public CoolDownStateVer2 CoolDownState => _coolDownState;

    /// <summary>コマンドを選択するState</summary>
    CommandStateVer2 _commandState = new();
    public CommandStateVer2 CommandState => _commandState;

    /// <summary>スキルをチョイスしたときのState</summary>
    ChoiceSkillStateVer2 _choiceSkillState = new();
    public ChoiceSkillStateVer2 ChoiceSkillState => _choiceSkillState;

    /// <summary>カウンター状態のState</summary>
    [SerializeField] CounterStateVer2 _counterState = new();
    public CounterStateVer2 CounterState => _counterState;

    TrueCounterStateVer2 _trueCounterState = new();
    public TrueCounterStateVer2 TrueCounterState => _trueCounterState;

    public bool _blockTime = false;

    void Start()
    {
        _dataBase = DataBase.Instance;
        GetEnemy();
        SetStatus();
        HPViewAccess();
        ChantingViewAccess(0, 1);
        Debug.Log($"BlockerHP:{HP}\nBlockerAttack:{Attack}\nBlockerDiffence:{Diffence}");
        _currentRPGState = _coolDownState;
    }

    /// <summary>エネミーをゲットする。(後々別方法で取ってくる予定なのでメソッドを作った。)</summary>
    void GetEnemy()
    {
        _enemy = GameObject.FindGameObjectWithTag("RPGEnemy")?.GetComponent<EnemyController>();
    }


    void Update()
    {
        if (RPGBattleManager.Instance.BattleState != BattleState.RPGBattle) { return; }
        _currentRPGState.UpdateState(this);

        //if (HP <= 0) { OnChangeState(DeathState); }
    }

    /// <summary>今のStateを変える時に呼ぶメソッド</summary>
    /// <param name="state"></param>
    public void OnChangeState(IRPGStateVer2 state)
    {
        if (_currentRPGState != state)
        {
            _currentRPGState.EndState(this);
            _currentRPGState = state;
            _currentRPGState.StartState(this);
        }
    }

}

public enum ChoiceSkill
{
    AttackSkill,
    AssistSkill
}
