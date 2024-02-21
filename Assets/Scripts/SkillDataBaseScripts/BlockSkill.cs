using MasterData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSkill", menuName = "Skill System/BlockSkill")]
public class BlockSkill : SkillObjects
{

    [SerializeField] float _attackValue = 1;
    public float AttackValue => _attackValue;
    [SerializeField] int _enemyDiffencePower = 1;
    public int EnemyDiffencePower => _enemyDiffencePower;
    [SerializeField] int _enemyOffencePower = 1;
    public int EnemyOffencePower => _enemyOffencePower;

    public void BlockSkillLoad(Skill skillobj)
    {
        SkillObjectsLoad(skillobj);
        _attackValue = skillobj.AttackValue;
        _enemyDiffencePower = skillobj.DiffencePower;
        _enemyOffencePower = skillobj.OffencePower;

    }
}
