using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Skill System/AttackSkill")]
public class AttackSkill : SkillObjects
{

    [SerializeField] float _attackValue = 1;
    public float AttackValue => _attackValue;
    [SerializeField] int _requireAttack = 1;
    public int RequireAttack => _requireAttack;

    public void AttackSkillLoad(Skill skillData)
    {
        SkillObjectsLoad(skillData);
        _attackValue = skillData.AttackValue;
        _requireAttack = skillData.RequaireAttack;
    }
}
