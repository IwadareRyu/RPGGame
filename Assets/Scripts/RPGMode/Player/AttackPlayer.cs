using RPGBattle;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : StatusClass
{
    [SerializeField] Text[] _commandText;
    [SerializeField] GameObject _attackObj;
    [SerializeField] float _attackLange = 3;
    [SerializeField] Animator _attackAnim;
    DataBase _dataBase;
    int _attackSkillCount;
    [Tooltip("選んだスキルのSetID")]
    int _skillChoiceNumber;
    public int SkillChoiceNumber => _skillChoiceNumber;
    [Tooltip("コマンドのクールタイムのスクリプト")]
    [SerializeField] CommandAttacker _commandAttacker = new();
    public CommandAttacker CommandAttacker => _commandAttacker;
    [Tooltip("スキルのクールタイムのスクリプト")]
    [SerializeField]ChastTimeAttacker _chastTimeAttacker;
    public ChastTimeAttacker ChastTimeAttacker => _chastTimeAttacker;

    AttackPlayerCoolTimeClass _currentCoolTimeScripts;

    // Start is called before the first frame update
    void Start()
    {
        _dataBase = DataBase.Instance;
        _enemy = GameObject.FindGameObjectWithTag("RPGEnemy")?.GetComponent<EnemyController>();
        SetStatus();
        //UIの初期化
        HPViewAccess();
        ChantingViewAccess(0, 1);
        CommandCoolTimeViewAccess(0,1);
        Debug.Log($"AttackerHP:{HP}\nAttackerAttack:{Attack}\nAttackerDiffence:{Diffence}");
        for (var i = 0; i < _commandText.Length; i++)
        {
            var index = _dataBase._attackSkillSetNo[i];
            _commandText[i].text = _dataBase.AttackSkillSelectData.SkillInfomation[index]._skillName.Substring(0, 4);
        }

        // CoolTimeClassの設定。(最初はCommandAttackerスクリプトに設定)
        _currentCoolTimeScripts = _commandAttacker;
        _currentCoolTimeScripts.StartCoolTime(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (RPGBattleManager.Instance.BattleState != BattleState.RPGBattle) { return; }
        if (_survive == Survive.Survive)
        {
            _currentCoolTimeScripts.UpdateCoolTime(this);

            if (HP <= 0)
            {
                _survive = Survive.Death;
            }
            TimeMethod();
        }
        else
        {
            Death();
        }
    }

    /// <summary>スキルのSetIDをセットして次のStateに移行する処理</summary>
    /// <param name="choiceSkill">SetID</param>
    public void SetSkill(int choiceSkill)
    {
        _skillChoiceNumber = choiceSkill;
        ChangeCoolTimeScripts(_chastTimeAttacker);
    }

    /// <summary>現在見ているStateを変更する処理</summary>
    /// <param name="coolTimeClass">次のState</param>
    public void ChangeCoolTimeScripts(AttackPlayerCoolTimeClass coolTimeClass)
    {
        _currentCoolTimeScripts.EndCoolTime(this);
        _currentCoolTimeScripts = coolTimeClass;
        _currentCoolTimeScripts.StartCoolTime(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 2, _attackLange);
    }

    /// <summary>没処理</summary>
    public override void ActionMode()
    {
        for (var i = 0; i < _dataBase._attackSkillbool.Length; i++)
        {
            _attackSkillCount += _dataBase._attackSkillbool[i] ? 1 : 0;
        }
        ConditionTextViewAccess("RightShiftでAttack！");
    }

    /// <summary>スキルで攻撃したときの処理</summary>
    /// <param name="i">SetID</param>
    public void SkillAttack(int i)
    {
        //SetIDが-1なら通常攻撃に移行。
        if (i == -1) 
        {
            NormalAttack();
            return;
        }
        // Skillの倍率に合わせた攻撃をEnemyに与える。
        var set = _dataBase.AttackSkillSelectData.SkillInfomation[_dataBase._attackSkillSetNo[i]];
        ConditionTextViewAccess($"{set._skillName}！");
        _attackAnim.SetTrigger("NormalAttack");
        if (set._selectSkill is AttackSkillSelect attackSkill)
        {
            _enemy.AddDamage(Attack, attackSkill.AttackValue);
            AudioManager.Instance.SEPlay(SE.AttackerAttack);
        }
        ChangeCoolTimeScripts(_commandAttacker);
    }

    /// <summary>普通の攻撃の処理</summary>
    public void NormalAttack()
    {
        ConditionTextViewAccess("攻撃！");
        AudioManager.Instance.SEPlay(SE.AttackerAttack);
        //敵に攻撃力分ダメージを与える。
        _enemy.AddDamage(Attack);
        ChangeCoolTimeScripts(_commandAttacker);
    }

    public override void RPGMode()
    {
        ConditionTextViewAccess("戻ってきたか！");
    }

    /// <summary>死んだときの処理</summary>
    void Death()
    {
        RPGBattleManager.Instance.BattleEnd();
        FightManager.Instance.Lose();
        ConditionTextViewAccess("☆Attackerは星になった☆");
    }
}
