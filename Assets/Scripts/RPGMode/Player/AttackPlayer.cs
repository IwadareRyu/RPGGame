using RPGBattle;
using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : StatusClass
{
    [SerializeField] float _commandTime = 10f;
    float _commandCoolTime;
    bool _commandbool;
    [SerializeField] GameObject _commandObj;
    [SerializeField] Text[] _commandText;
    [SerializeField] Text _enumtext;
    [SerializeField] GameObject _attackObj;
    [SerializeField] float _attackLange = 3;
    [SerializeField] Animator _attackAnim;
    DataBase _dataBase;
    int _attackSkillCount;
    // Start is called before the first frame update
    void Start()
    {
        _dataBase = DataBase.Instance;
        _enemy = GameObject.FindGameObjectWithTag("RPGEnemy")?.GetComponent<EnemyController>();
        if (_commandObj) _commandObj.SetActive(false);
        SetStatus();
        HPViewAccess();
        ChantingViewAccess(_commandCoolTime, _commandTime);
        Debug.Log($"AttackerHP:{HP}");
        Debug.Log($"AttackerAttack:{Attack}");
        Debug.Log($"AttackerDiffence:{Diffence}");
        for (var i = 0; i < _commandText.Length; i++)
        {
            var index = _dataBase._attackSkillSetNo[i];
            _commandText[i].text = _dataBase.AttackSkillSelectData.SkillInfomation[index]._skillName.Substring(0, 4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RPGBattleManager.Instance.BattleState != BattleState.RPGBattle) { return; }
        if (_survive == Survive.Survive)
        {
            if (!_commandbool)
            {
                _commandCoolTime += Time.deltaTime;
                ChantingViewAccess(_commandCoolTime,_commandTime);
            }

            if (_commandCoolTime > _commandTime)
            {
                if (!_commandbool)
                {
                    if (_commandObj) _commandObj.SetActive(true);
                    _commandbool = true;
                    ShowText("コマンド？");
                }

                Command();
            }

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

    void Command()
    {
        if (Input.GetButtonDown("Attack"))
        {
            Attacker();
        }

        if (Input.GetButtonDown("Skill1"))
        {
            SkillAttack(0);
        }

        if (Input.GetButtonDown("Skill2"))
        {
            SkillAttack(1);
        }

        if (Input.GetButtonDown("Skill3"))
        {
            SkillAttack(2);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 2, _attackLange);
    }

    public override void ActionMode()
    {
        for (var i = 0; i < _dataBase._attackSkillbool.Length; i++)
        {
            _attackSkillCount += _dataBase._attackSkillbool[i] ? 1 : 0;
        }
        ShowText("RightShiftでAttack！");
        CommandReset();
    }

    /// <summary>普通の攻撃の処理</summary>
    void Attacker()
    {
        ShowText("攻撃！");
        AudioManager.Instance.SEPlay(SE.AttackerAttack);
        _enemy.AddDamage(Attack);
        CommandReset();
    }

    /// <summary>スキルで攻撃したときの処理</summary>
    /// <param name="i"></param>
    void SkillAttack(int i)
    {
        var set = _dataBase.AttackSkillSelectData.SkillInfomation[_dataBase._attackSkillSetNo[i]];
        ShowText($"{set._skillName}！");
        _attackAnim.SetTrigger("NormalAttack");
        if (set._selectSkill is AttackSkillSelect attackSkill)
        {
            _enemy.AddDamage(Attack, attackSkill.AttackValue);
            AudioManager.Instance.SEPlay(SE.AttackerAttack);
        }
        CommandReset();
    }

    public override void RPGMode()
    {
        ShowText("戻ってきたか！");
    }

    /// <summary>コマンドの待機時間などをリセットする処理</summary>
    void CommandReset()
    {
        _commandCoolTime = 0;
        ChantingViewAccess(_commandCoolTime, _commandTime);
        if (_commandObj) _commandObj.SetActive(false);
        _commandbool = false;
    }

    /// <summary>テキストを更新するときの処理</summary>
    /// <param name="str"></param>
    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    public override void ChantingTimeReset()
    {
        CommandReset();
    }

    /// <summary>死んだときの処理</summary>
    void Death()
    {
        RPGBattleManager.Instance.BattleEnd();
        FightManager.Instance.Lose();
        ShowText("☆Attackerは星になった☆");
    }
}
