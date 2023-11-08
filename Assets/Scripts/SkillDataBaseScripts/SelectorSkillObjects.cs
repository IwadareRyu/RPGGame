using MasterData;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillObjcts", menuName = "Skill System/SkillObjects")]
public class SelectorSkillObjects : ScriptableObject
{
    [SerializeReference, SubclassSelector] private IAttributeSkill[] skills;
}

public interface IAttributeSkill { };

public class AttackSkillSelect : IAttributeSkill
{
    [Header("スキルのID"), SerializeField]
    int _skillID;
    public int SkillID => _skillID;

    [Header("スキルの名前"), SerializeField]
    string _skillName;
    public string SkillName => _skillName;

    [Header("スキルの種類"), Tooltip("Skillの種類")]
    SkillType _type;
    public SkillType Type => _type;

    [TextArea(10, 10)]
    public string _description;
    public string Description => _description;

    [SerializeField] float _attackValue = 1;
    public float AttackValue => _attackValue;
    [SerializeField] int _requireAttack = 1;
    public int RequireAttack => _requireAttack;

    public void AttackSkillLoad(Skill skill)
    {
        _skillID = skill.ID;
        _skillName = skill.SkillName;
        _type = skill.SkillType;
        _description = skill.Description;
        _attackValue = skill.AttackValue;
        _requireAttack = skill.RequaireAttack;
    }
}

public class BlockSkillSelect : IAttributeSkill
{
    [Header("スキルのID"), SerializeField]
    int _skillID;
    public int SkillID => _skillID;

    [Header("スキルの名前"), SerializeField]
    string _skillName;
    public string SkillName => _skillName;

    [Header("スキルの種類"), Tooltip("Skillの種類")]
    SkillType _type;
    public SkillType Type => _type;

    [TextArea(10, 10)]
    public string _description;
    public string Description => _description;

    [SerializeField] float _attackValue = 1;
    public float AttackValue => _attackValue;

    [SerializeField] int _enemyDiffencePower = 1;
    public int EnemyDiffencePower => _enemyDiffencePower;

    [SerializeField] int _enemyOffencePower = 1;
    public int EnemyOffencePower => _enemyOffencePower;
    public void BlockSkillLoad(Skill skill)
    {
        _skillID = skill.ID;
        _skillName = skill.SkillName;
        _type = skill.SkillType;
        _description = skill.Description;
        _attackValue = skill.AttackValue;
        _enemyDiffencePower = skill.DiffencePower;
        _enemyOffencePower = skill.OffencePower;
    }
}

public class AttackMagicSelect : IAttributeSkill
{
    [Header("スキルのID"), SerializeField]
    int _skillID;
    public int SkillID => _skillID;

    [Header("スキルの名前"), SerializeField]
    string _skillName;
    public string SkillName => _skillName;

    [Header("スキルの種類"), Tooltip("Skillの種類")]
    SkillType _type;
    public SkillType Type => _type;

    [TextArea(10, 10)]
    public string _description;
    public string Description => _description;

    [SerializeField] float _attackValue;
    public float AttackValue => _attackValue;
    [SerializeField] int _skillPoint;
    public int SkillPoint => _skillPoint;

    [SerializeField] int[] _adjacent;
    public int[] Adjacent => _adjacent;
    public void BlockSkillLoad(Skill skill)
    {
        _skillID = skill.ID;
        _skillName = skill.SkillName;
        _type = skill.SkillType;
        _description = skill.Description;
        _attackValue = skill.AttackValue;
        _skillPoint = skill.SkillPoint;
        if (skill.Adjacent == "null")
        {
            _adjacent = Array.ConvertAll(skill.Adjacent.Split(), int.Parse);
        }
    }
}

public class BlockMagicSelect : IAttributeSkill
{
    [Header("スキルのID"), SerializeField]
    int _skillID;
    public int SkillID => _skillID;

    [Header("スキルの名前"), SerializeField]
    string _skillName;
    public string SkillName => _skillName;

    [Header("スキルの種類"), Tooltip("Skillの種類")]
    SkillType _type;
    public SkillType Type => _type;

    [TextArea(10, 10)]
    public string _description;
    public string Description => _description;

    [SerializeField] int _plusDiffencePower = 1;
    public int PlusDiffencePower => _plusDiffencePower;
    [SerializeField] int _plusAttackPower = 1;
    public int PlusAttackPower => _plusAttackPower;
    [SerializeField] int _skillPoint = 1;
    public int SkillPoint => _skillPoint;
    [SerializeField] int _healingHP = 0;
    public int HealingHP => _healingHP;

    public void BlockSkillLoad(Skill skill)
    {
        _skillID = skill.ID;
        _skillName = skill.SkillName;
        _type = skill.SkillType;
        _description = skill.Description;
        _plusAttackPower = skill.OffencePower;
        _plusDiffencePower = skill.DiffencePower;
        _skillPoint = skill.SkillPoint;
        _healingHP += skill.HealingHP;
    }
}