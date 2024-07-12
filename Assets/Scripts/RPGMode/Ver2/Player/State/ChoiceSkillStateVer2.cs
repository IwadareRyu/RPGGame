using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>スキルをチョイスしたときのState</summary>
[Serializable]
public class ChoiceSkillStateVer2 : IRPGStateVer2
{
    float _skillTime;
    float _currentTime;
    public void Init(RPGPlayerVer2 player)
    {

    }

    public void StartState(RPGPlayerVer2 player)
    {
        _skillTime = player._useSkill._chastingTime;
        _currentTime = 0f;
    }

    public void UpdateState(RPGPlayerVer2 player)
    {
        // 

        // Skillチャージ
        _currentTime += Time.deltaTime;
        if(_currentTime > _skillTime)
        {
            // Skill発動
            if(player._useSkill._selectSkill is AttackMagicSelect attackMagicSkill)
            {
                player._enemy.AddDamage(player.Attack, attackMagicSkill.AttackValue);
            }
            else if (player._useSkill._selectSkill is BlockMagicSelect blockMagicSkill)
            {
                player.AddBuff(blockMagicSkill.PlusAttackPower,
                    blockMagicSkill.PlusDiffencePower,
                    blockMagicSkill.HealingHP);
            }
            player.OnChangeState(player.CoolDownState);
        }
    }

    public void EndState(RPGPlayerVer2 player)
    {
        
    }
}
