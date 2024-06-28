using MasterData;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>コマンドを選択するState</summary>
[Serializable]
public class CommandStateVer2 : IRPGStateVer2
{
    [SerializeField] Image _commandImage;
    [SerializeField] Image _attackCommandImage;
    [SerializeField] Image _assistCommandImage;
    Image _currentChoiceSkill;
    ChoicceSkill _choiceSkillState;
    bool _choiceSkill;

    public CommandStateVer2()
    {
        _attackCommandImage.enabled = false;
        _assistCommandImage.enabled = false;
        _currentChoiceSkill = _attackCommandImage;
    }

    public void Init(RPGPlayerVer2 player)
    {
        return;
    }

    public void StartState(RPGPlayerVer2 player)
    {
        _commandImage.enabled = true;
        _choiceSkill = false;
    }

    public void UpdateState(RPGPlayerVer2 player)
    {
        if (!_choiceSkill)
        {
            _choiceSkill = ChoiceAction(player);
        }
        else
        {
            _choiceSkill = ChoiceSkill(player);
        }
    }

    public bool ChoiceAction(RPGPlayerVer2 player)
    {

        if (Input.GetButtonDown("Attack"))
        {
            _currentChoiceSkill = _attackCommandImage;
            _currentChoiceSkill.enabled = true;
            _choiceSkillState = ChoicceSkill.AttackSkill;
            return true;
        }
        else if(Input.GetButtonDown("Skill2"))
        {
            _currentChoiceSkill = _assistCommandImage;
            _currentChoiceSkill.enabled = true;
            _choiceSkillState = ChoicceSkill.AssistSkill;
            return true;
        }
        else if(Input.GetButtonDown("Skill3"))
        {
            player.OnChangeState(player.CounterState);
        }
        return false;
    }

    public bool ChoiceSkill(RPGPlayerVer2 player)
    {
        if (Input.GetButtonUp("Attack") || Input.GetButtonUp("Skill2"))
        {
            _currentChoiceSkill.enabled = false;
            return false;
        }
        else if(Input.GetButtonDown("Skill1"))
        {
            //使うSkillを_useSkillにアタッチ
            if (_choiceSkillState == ChoicceSkill.AttackSkill)
            {
                player._useSkill = DataBase.Instance.AttackMagicSelectData.SkillInfomation[DataBase.Instance._attackMagicSetNo[0]];
            }
            else
            {
                player._useSkill = DataBase.Instance.BlockMagicSelectData.SkillInfomation[DataBase.Instance._blockMagicSetNo[0]];
            }
            player.OnChangeState(player.ChoiceSkillState);
        }
        else if(Input.GetButtonDown("Skill3"))
        {
            //使うSkillを_useSkillにアタッチ
            if (_choiceSkillState == ChoicceSkill.AttackSkill)
            {
                player._useSkill = DataBase.Instance.AttackMagicSelectData.SkillInfomation[DataBase.Instance._attackMagicSetNo[1]];
                
            }
            else
            {
                player._useSkill = DataBase.Instance.BlockMagicSelectData.SkillInfomation[DataBase.Instance._blockMagicSetNo[1]];
            }
            player.OnChangeState(player.ChoiceSkillState);
        }
        return true;
    }

    public void EndState(RPGPlayerVer2 player)
    {
        _commandImage.enabled = false;
        _currentChoiceSkill.enabled = false;
    }

    public enum ChoicceSkill
    {
        AttackSkill,
        AssistSkill
    }
}
