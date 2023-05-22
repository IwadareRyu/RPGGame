using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSkill", menuName = "Skill System/BlockSkill")]
public class BlockSkill : SkillObjects
{

    [SerializeField] float _attackValue = 1;
    [SerializeField] float _enemyDiffencePower = 1;
    [SerializeField] float _enemyOffencePower = 1;
    private void Awake()
    {
        _type = SkillTypeEnum.SkillType.BlockSkill;
    }
}
