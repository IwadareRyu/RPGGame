using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class DataBase : SingletonMonovihair<DataBase>
{
    [Header("アタックスキル")]
    [SerializeField] AttackSkill[] _attackData;
    public AttackSkill[] AttackSkillData => _attackData;
    public bool[] _attackSkillbool;
    public int[] _attackSkillSetNo = new int[3] {0,0,0};
    private const string _attackSkillFileName = "AttackSkill";

    [Header("ブロックスキル")]
    [SerializeField] BlockSkill[] _blockData;
    public BlockSkill[] BlockSkillData => _blockData;
    public bool[] _blockSkillbool;
    public int[] _blockSkillSetNo = new int[1] {0};
    private const string _blockSkillFileName = "BlockSkill";

    [Header("攻撃魔法")]
    [SerializeField] AttackMagic[] _attackMagicData;
    public AttackMagic[] AttackMagicData => _attackMagicData;
    public bool[] _attackMagicbool;
    public int[] _attackMagicSetNo = new int[2] {0,0};
    private const string _attackMagicFileName = "AttackMagic";

    [Header("防御魔法")]
    [SerializeField] BlockMagic[] _blockMagicData;
    public BlockMagic[] BlockMagicData => _blockMagicData;
    public bool[] _blockMagicbool;
    public int[] _blockMagicSetNo = new int[2] {0,0};
    private const string _blockMagicFileName = "BlockMagic";

    [System.Serializable]
    public struct SkillData
    {
        public bool[] _getSkill;
        public int[] _SetNo;
    }

    public struct SkillPt
    {
        public int _skillPt;
    }

    SkillPt _pt;

    SkillData _skillData;
    SkillData _attackSkillData;
    SkillData _blockSkillData;
    SkillData _blockMagicSkillData;
    SkillData _attackMagicSkillData;

    [SerializeField]
    DataSaveTest _dataSave;

    protected override bool _dontDestroyOnLoad { get { return true; } }
    
    [Header("今持っているスキルポイント"),Tooltip("今持っているスキルポイント"),SerializeField]
    int _skillPoint = 5;
    public int SkillPoint => _skillPoint;
    private const string _skillPointFile = "SkillPoint";
    public void GetSkillPoint(int i)
    {
        _skillPoint += i;
    }

    private void OnEnable()
    {
        _attackMagicSkillData = SkillDataLoad(_attackMagicFileName);
        _attackMagicSetNo = SetNoLoad(_attackMagicSetNo,_attackMagicSkillData);
        _attackMagicbool = GetBoolLoad(_attackMagicbool, _attackMagicSkillData);
        _blockSkillData = SkillDataLoad(_blockSkillFileName);
        _blockSkillSetNo = SetNoLoad(_blockSkillSetNo, _blockSkillData);
        _attackSkillData = SkillDataLoad(_attackSkillFileName);
        _attackSkillSetNo = SetNoLoad(_attackSkillSetNo, _attackSkillData);
        _blockMagicSkillData = SkillDataLoad(_blockMagicFileName);
        _blockMagicSetNo = SetNoLoad(_blockMagicSetNo, _blockMagicSkillData);
        _skillPoint = SkillPointLoad(_skillPointFile);
        //_skillPoint = 0;
    }

    private void OnDisable()
    {
        SkillDataSave(_attackMagicSetNo,_attackMagicbool,_attackMagicFileName);
        SkillDataSave(_attackSkillSetNo, _attackSkillbool, _attackSkillFileName);
        SkillDataSave(_blockSkillSetNo, _blockSkillbool, _blockSkillFileName);
        SkillDataSave(_blockMagicSetNo, _blockMagicbool, _blockMagicFileName);
        PtDataSave(_skillPoint,_skillPointFile);
    }

    void SkillDataSave(int[] setNo, bool[] getSkillbool,string fileName)
    {
        _skillData._SetNo = setNo;
        _skillData._getSkill = getSkillbool;
        _dataSave.SaveSkill(_skillData, fileName);
    }

    void PtDataSave(int skillPt,string skillPtFile)
    {
        _pt._skillPt = skillPt;
        _dataSave.SaveSkillPoint(_pt, skillPtFile);
    }

    SkillData SkillDataLoad(string fileName)
    {
        return _dataSave.LoadSkill(fileName);
    }

    int[] SetNoLoad(int[] setNo,SkillData skillData)
    {
        if (skillData._SetNo.Length != 0 && setNo.Length == skillData._SetNo.Length)
        {
            return skillData._SetNo;
        }
        return setNo;
    }

    bool[] GetBoolLoad(bool[] getBool,SkillData skillData)
    {
        if(skillData._getSkill.Length != 0)
        {
            return skillData._getSkill;
        }
        return getBool;
    }

    int SkillPointLoad(string fileName)
    {
        _pt = _dataSave.LoadPoint(fileName);
        if(_pt._skillPt != null)
        {
            return _pt._skillPt;
        }
        return _skillPoint;
    }
}
