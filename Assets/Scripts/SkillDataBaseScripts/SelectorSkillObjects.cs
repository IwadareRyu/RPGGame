using MasterData;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillObjcts", menuName = "Skill System/SkillObjects")]
public class SelectorSkillObjects : ScriptableObject
{
    [SerializeField] SkillInfomation[] _skillInfomation;

    public SkillInfomation[] SkillInfomation => _skillInfomation;


    public void AttributeSkillLoad(ref MasterDataClass<Skill> skills)
    {
        for(var i = 0;i < _skillInfomation.Length && i < skills.Data.Length;i++)
        {
            _skillInfomation[i]._skillID = skills.Data[i].ID;
            _skillInfomation[i]._skillName = skills.Data[i].SkillName;
            _skillInfomation[i]._tmpSkillName = skills.Data[i].TmpName;
            _skillInfomation[i]._description = skills.Data[i].Description;
            _skillInfomation[i]._type = skills.Data[i].SkillType;
            _skillInfomation[i]._chastingTime = skills.Data[i].ChantingTime;
            if (_skillInfomation[i]._selectSkill is AttackSkillSelect attackSkill)
            {
                attackSkill.AttackSkillLoad(skills.Data[i]);
            }
            else if(_skillInfomation[i]._selectSkill is BlockSkillSelect blockSkill)
            {
                blockSkill.BlockSkillLoad(skills.Data[i]);
            }
            else if(_skillInfomation[i]._selectSkill is AttackMagicSelect attackMagic)
            {
                attackMagic.AttackMagicLoad(skills.Data[i]);
            }
            else if(_skillInfomation[i]._selectSkill is BlockMagicSelect blockMagic)
            {
                blockMagic.BlockMagicLoad(skills.Data[i]);
            }
        }
    }

}
[Serializable]
public struct SkillInfomation
{
    [Header("スキルのID"), SerializeField]
    public int _skillID;

    [Header("スキルの名前"), SerializeField]
    public string _skillName;

    public string _tmpSkillName;

    [Header("スキルの種類"), Tooltip("Skillの種類")]
    public SkillType _type;

    [TextArea(10, 10)]
    public string _description;

    [Header("スキルの詠唱時間"),Tooltip("スキルの詠唱時間")]
    public float _chastingTime;

    [SerializeReference, SubclassSelector] public IAttributeSkill _selectSkill;

}

public interface IAttributeSkill { };

public class AttackSkillSelect : IAttributeSkill
{
    [SerializeField] float _attackValue = 1;
    public float AttackValue => _attackValue;
    [SerializeField] int _requireAttack = 1;
    public int RequireAttack => _requireAttack;

    public void AttackSkillLoad(Skill skill)
    {
        _attackValue = skill.AttackValue;
        _requireAttack = skill.RequaireAttack;
    }
}

public class BlockSkillSelect : IAttributeSkill
{
    [SerializeField] float _attackValue = 1;
    public float AttackValue => _attackValue;

    [SerializeField] int _enemyDiffencePower = 1;
    public int EnemyDiffencePower => _enemyDiffencePower;

    [SerializeField] int _enemyOffencePower = 1;
    public int EnemyOffencePower => _enemyOffencePower;
    public void BlockSkillLoad(Skill skill)
    {
        _attackValue = skill.AttackValue;
        _enemyDiffencePower = skill.DiffencePower;
        _enemyOffencePower = skill.OffencePower;
    }
}

public class AttackMagicSelect : IAttributeSkill
{
    [SerializeField] float _attackValue;
    public float AttackValue => _attackValue;
    [SerializeField] int _skillPoint;
    public int SkillPoint => _skillPoint;
    public void AttackMagicLoad(Skill skill)
    {
        _attackValue = skill.AttackValue;
        _skillPoint = skill.SkillPoint;
    }
}

public class BlockMagicSelect : IAttributeSkill
{
    [SerializeField] int _plusDiffencePower = 1;
    public int PlusDiffencePower => _plusDiffencePower;
    [SerializeField] int _plusAttackPower = 1;
    public int PlusAttackPower => _plusAttackPower;
    [SerializeField] int _skillPoint = 1;
    public int SkillPoint => _skillPoint;
    [SerializeField] int _healingHP = 0;
    public int HealingHP => _healingHP;

    public void BlockMagicLoad(Skill skill)
    {
        _plusAttackPower = skill.OffencePower;
        _plusDiffencePower = skill.DiffencePower;
        _skillPoint = skill.SkillPoint;
        _healingHP += skill.HealingHP;
    }
}