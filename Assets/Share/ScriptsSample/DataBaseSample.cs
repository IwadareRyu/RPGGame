using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseSample : SingletonMonovihair<DataBaseSample>
{
    [Header("スキルデータ"),Tooltip("SkillData")]
    [SerializeField] SkillObjectSample[] _skillData;
    public SkillObjectSample[] SkillData => _skillData;

    [Header("スキルの取得状況"),Tooltip("スキルの取得状況")]
    public bool[] _skillbool;
    [Header("スキルのセット\n(スキルデータの要素数から参照)"),Tooltip("スキルのセット")]
    public int[] _skillSetNo;

    [Header("今持っているスキルポイント"), Tooltip("今持っているスキルポイント"),SerializeField]
    int _skillPoint = 5;
    public int SkillPoint => _skillPoint;

    protected override bool _dontDestroyOnLoad { get { return true; } }

    public void GetSkillPoint(int i)
    {
        _skillPoint += i;
    }
}
