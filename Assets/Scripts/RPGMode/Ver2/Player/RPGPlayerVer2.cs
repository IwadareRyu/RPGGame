using RPGBattle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RPGPlayerVer2 : StatusClass
{
    DataBase _dataBase;

    [NonSerialized]
    public SkillInfomation _useSkill;

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
    CounterStateVer2 _counterState = new();
    public CounterStateVer2 CounterState => _counterState;

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
        _currentRPGState.EndState(this);
        _currentRPGState = state;
        _currentRPGState.StartState(this);
    }

}
