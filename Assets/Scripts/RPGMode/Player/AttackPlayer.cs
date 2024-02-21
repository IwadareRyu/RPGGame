using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : StatusClass
{
    float _time;
    bool _commandbool;
    [SerializeField] GameObject _commandObj;
    [SerializeField] Text[] _commandText;
    [SerializeField] Text _enumtext;
    [SerializeField] GameObject _attackObj;
    [SerializeField] float _attackLange = 3;
    [SerializeField] Animator _attackAnim;
    int _attackSkillCount;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("RPGEnemy")?.GetComponent<EnemyController>();
        if (_commandObj) _commandObj.SetActive(false);
        SetStatus();
        ShowSlider();
        Debug.Log($"AttackerHP:{HP}");
        Debug.Log($"AttackerAttack:{Attack}");
        Debug.Log($"AttackerDiffence:{Diffence}");
        for (var i = 0; i < _commandText.Length; i++)
        {
            var index = DataBase._attackSkillSetNo[i];
            _commandText[i].text = DataBase.AttackSkillSelectData.SkillInfomation[index]._skillName.Substring(0, 4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_survive == Survive.Survive)
        {
            if (FightManager.Instance.BattleState == BattleState.RPGBattle)
            {
                if (!_commandbool)
                {
                    _time += Time.deltaTime;
                }

                if (_time > 10f)
                {
                    if (!_commandbool)
                    {
                        if (_commandObj) _commandObj.SetActive(true);
                        _commandbool = true;
                        ShowText("コマンド？");
                    }

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
            }


            if (FightManager.Instance.BattleState == BattleState.ActionBattle)
            {
                if (Input.GetButtonDown("AttackerAttack"))
                {
                    ActionAttack();
                }
            }

            if (HP <= 0)
            {
                _survive = Survive.Death;
            }
            TimeMethod();
        }
        else
        {
            if (!_death)
            {
                _death = true;
                Death();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 2, _attackLange);
    }

    public override void ActionMode()
    {
        for (var i = 0; i < DataBase._attackSkillbool.Length; i++)
        {
            _attackSkillCount += DataBase._attackSkillbool[i] ? 1 : 0;
        }
        ShowText("RightShiftでAttack！");
        CommandReset();
    }

    /// <summary>普通の攻撃の処理</summary>
    void Attacker()
    {
        ShowText("攻撃！");
        _enemy.AddDamage(Attack);
        CommandReset();
    }

    /// <summary>スキルで攻撃したときの処理</summary>
    /// <param name="i"></param>
    void SkillAttack(int i)
    {
        var set = DataBase.AttackSkillSelectData.SkillInfomation[DataBase._attackSkillSetNo[i]];
        ShowText($"{set._skillName}！");
        _attackAnim.SetTrigger("NormalAttack");
        if(set._selectSkill is AttackSkillSelect attackSkill)
        _enemy.AddDamage(Attack, attackSkill.AttackValue);
        CommandReset();
    }

    void ActionAttack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * 2, _attackLange);
        _attackAnim.SetTrigger("NormalAttack");
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "EnemyBullet")
            {
                _enemy.AddMagicDamage(Attack * (Mathf.Max(0.1f, _attackSkillCount / 8)));
                Destroy(col.gameObject);
            }
        }
    }

    public override void RPGMode()
    {
        ShowText("戻ってきたか！");
    }

    /// <summary>コマンドの待機時間などをリセットする処理</summary>
    void CommandReset()
    {
        _time = 0;
        if (_commandObj) _commandObj.SetActive(false);
        _commandbool = false;
    }

    /// <summary>テキストを更新するときの処理</summary>
    /// <param name="str"></param>
    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    /// <summary>死んだときの処理</summary>
    void Death()
    {
        _attackObj.transform.Rotate(90f, 0f, 0f);
        ShowText("俺は死んだぜ☆");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            if (FightManager.Instance.BattleState == BattleState.ActionBattle)
            {
                AddDamage(10);
            }
            Destroy(other.gameObject);
        }
    }
}
