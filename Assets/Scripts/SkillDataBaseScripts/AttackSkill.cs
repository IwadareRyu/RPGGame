using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Skill System/AttackSkill")]
public class AttackSkill : SkillObjects
{

    [SerializeField] float _attackValue = 1;
    [SerializeField] int _requireAttack = 1;
    private void Awake()
    {
        _type = SkillTypeEnum.SkillType.BlockMagic;
    }
}
