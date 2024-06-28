using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>スキルをチョイスしたときのState</summary>
[Serializable]
public class ChoiceSkillStateVer2 : IRPGStateVer2
{
    [SerializeField] ChoiceSkill _choiceSkillState;
    public void Init(RPGPlayerVer2 player)
    {
        
    }

    public void StartState(RPGPlayerVer2 player)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(RPGPlayerVer2 player)
    {
        throw new System.NotImplementedException();
    }

    public void EndState(RPGPlayerVer2 player)
    {
        throw new System.NotImplementedException();
    }

    public enum ChoiceSkill
    {
        AttackSkill,
        AssistSkill
    }
}
