using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : StatusClass
{
    Skill _skill;
    float _time;
    bool _commandbool;
    [SerializeField]GameObject _commandObj;
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
        _skill = (Skill)i;
        ShowText($"{_skill}！");
        _enemy.AddDamage(Attack, 1.2f);
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

    enum Skill
    {
        SkillOne,
        SkillTwo,
        SkillThree,
    }
}
