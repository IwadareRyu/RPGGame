using MasterData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockMagic", menuName = "Skill System/BlockMagic")]
public class BlockMagic : SkillObjects
{

    [SerializeField] int _plusDiffencePower = 1;
    public int PlusDiffencePower => _plusDiffencePower;
    [SerializeField] int _plusAttackPower = 1;
    public int PlusAttackPower => _plusAttackPower;
    [SerializeField] int _skillPoint = 1;
    public int SkillPoint => _skillPoint;
    [SerializeField] int _healingHP = 0;
    public int HealingHP => _healingHP;

    public void BlockMagicLoad(Skill skillobj)
    {
        SkillObjectsLoad(skillobj);
        _plusAttackPower = skillobj.OffencePower;
        _plusDiffencePower = skillobj.DiffencePower;
        _skillPoint = skillobj.SkillPoint;
        _healingHP = skillobj.HealingHP;
    }
}
