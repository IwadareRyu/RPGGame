using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : StatusClass
{
    float _time;
    bool _commandbool;
    [SerializeField]GameObject _commandObj;
    [SerializeField] Text[] _commandText;
    [SerializeField] Text _enumtext;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<EnemyController>();
        if (_commandObj) _commandObj.SetActive(false);
        SetStatus();
        ShowSlider();
        Debug.Log($"AttackerHP:{HP}");
        Debug.Log($"AttackerAttack:{Attack}");
        Debug.Log($"AttackerDiffence:{Diffence}");
        for(var i = 0;i < _commandText.Length;i++)
        {
            _commandText[i].text = DataBase.AttackSkillData[DataBase._attackSkillSetNo[i]].SkillName.Substring(0,4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_commandbool)
        {
            _time += Time.deltaTime;
            //Debug.Log(_time);
        }

        if (_time > 10f)
        {

            if(!_commandbool)
            {
                if(_commandObj) _commandObj.SetActive(true);
                _commandbool = true;
                ShowText("コマンド？");
            }

            if (Input.GetButtonDown("Attack"))
            {
                Attacker();
            }

            if(Input.GetButtonDown("Skill1"))
            {
                SkillAttack(0);
            }

            if(Input.GetButtonDown("Skill2"))
            {
                SkillAttack(1);
            }
            
            if(Input.GetButtonDown("Skill3"))
            {
                SkillAttack(2);
            }
        }
    }

    void Attacker()
    {
        ShowText("攻撃！");
        _enemy.AddDamage(Attack);
        CommandReset();
    }

    void SkillAttack(int i)
    {
        var set = DataBase.AttackSkillData[DataBase._attackSkillSetNo[i]];
        ShowText($"{set.SkillName}！");
        _enemy.AddDamage(Attack, set.AttackValue);
        CommandReset();
    }

    void CommandReset()
    {
        _time = 0;
        if(_commandObj) _commandObj.SetActive(false);
        _commandbool = false;
    }

    void ShowText(string str)
    {
        _enumtext.text = str;
    }
}
