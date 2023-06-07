using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockMagic", menuName = "Skill System/BlockMagic")]
public class BlockMagic : SkillObjects
{

    [SerializeField] float _plusDiffencePower = 1;
    [SerializeField] float _plusAttackPower = 1;
    [SerializeField] int _skillPoint = 1;
    public int SkillPoint => _skillPoint;
    [SerializeField] int _healingHP = 0;
    private void Awake()
    {
        _type = SkillType.BlockMagic;
    }
}
