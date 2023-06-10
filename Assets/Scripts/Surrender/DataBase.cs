using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : SingletonMonovihair<DataBase>
{
    [Header("アタックスキル")]
    [SerializeField] AttackSkill[] _attackData;
    public AttackSkill[] AttackSkillData => _attackData;
    public bool[] _attackSkillbool;
    public int[] _attackSkillSetNo = new int[3] {0,0,0};

    [Header("ブロックスキル")]
    [SerializeField] BlockSkill[] _blockData;
    public BlockSkill[] BlockSkillData => _blockData;
    public bool[] _blockSkillbool;
    public int[] _blockSkillSetNo = new int[1] {0};

    [Header("攻撃魔法")]
    [SerializeField] AttackMagic[] _attackMagicData;
    public AttackMagic[] AttackMagicData => _attackMagicData;
    public bool[] _attackMagicbool;
    public int[] _attackMagicSetNo = new int[2] {0,0};

    [Header("防御魔法")]
    [SerializeField] BlockMagic[] _blockMagicData;
    public BlockMagic[] BlockMagicData => _blockMagicData;
    public int[] _blockMagicSetNo = new int[2] {0,0};

    protected override bool _dontDestroyOnLoad { get { return true; } }

    public bool[] _blockMagicbool;

    [Header("今持っているスキルポイント"),Tooltip("今持っているスキルポイント"),SerializeField]
    int _skillPoint = 5;
    public int SkillPoint => _skillPoint;

    public void GetSkillPoint(int i)
    {
        _skillPoint += i;
    }
}
