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
    [SerializeField] GameObject _attackObj;
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
            //_commandText[i].text = DataBase.AttackSkillData[DataBase._attackSkillSetNo[i]].SkillName.Substring(0,4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_survive == Survive.Survive)
        {
            if (!_commandbool)
            {
                _time += Time.deltaTime;
                //Debug.Log(_time);
            }

            if (_time > 10f)
            {
                if (!_commandbool)
                {
                    if (_commandObj) _commandObj.SetActive(true);
                    _commandbool = true;
                    ShowText("�R�}���h�H");
                }

                if (Input.GetButtonDown("Attack"))
                {
                    Attacker();
                }

                if (Input.GetButtonDown("Skill1"))
                {
                    //SkillAttack(0);
                }

                if (Input.GetButtonDown("Skill2"))
                {
                    //SkillAttack(1);
                }

                if (Input.GetButtonDown("Skill3"))
                {
                    //SkillAttack(2);
                }
            }

            if(HP <= 0)
            {
                _survive = Survive.Death;
            }
            TimeMethod();
        }
        else
        {
            if(!_death)
            {
                _death = true;
                Death();
            }
        }
    }

    /// <summary>���ʂ̍U���̏���</summary>
    void Attacker()
    {
        ShowText("�U���I");
        _enemy.AddDamage(Attack);
        CommandReset();
    }

    /// <summary>�X�L���ōU�������Ƃ��̏���</summary>
    /// <param name="i"></param>
    void SkillAttack(int i)
    {
        var set = DataBase.AttackSkills[DataBase._attackSkillSetNo[i]];
        ShowText($"{set.SkillName}�I");
        _enemy.AddDamage(Attack, set.AttackValue);
        CommandReset();
    }

    /// <summary>�R�}���h�̑ҋ@���ԂȂǂ����Z�b�g���鏈��</summary>
    void CommandReset()
    {
        _time = 0;
        if(_commandObj) _commandObj.SetActive(false);
        _commandbool = false;
    }

    /// <summary>�e�L�X�g���X�V����Ƃ��̏���</summary>
    /// <param name="str"></param>
    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    /// <summary>���񂾂Ƃ��̏���</summary>
    void Death()
    {
        _attackObj.transform.Rotate(90f, 0f, 0f);
        ShowText("���͎��񂾂���");
    }
}
