using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseSample : SingletonMonovihair<DataBaseSample>
{
    [Header("�X�L���f�[�^"),Tooltip("SkillData")]
    [SerializeField] SkillObjectSample[] _skillData;
    public SkillObjectSample[] SkillData => _skillData;

    [Header("�X�L���̎擾��"),Tooltip("�X�L���̎擾��")]
    public bool[] _skillbool;
    [Header("�X�L���̃Z�b�g\n(�X�L���f�[�^�̗v�f������Q��)"),Tooltip("�X�L���̃Z�b�g")]
    public int[] _skillSetNo;

    [Header("�������Ă���X�L���|�C���g"), Tooltip("�������Ă���X�L���|�C���g"),SerializeField]
    int _skillPoint = 5;
    public int SkillPoint => _skillPoint;

    protected override bool _dontDestroyOnLoad { get { return true; } }

    public void GetSkillPoint(int i)
    {
        _skillPoint += i;
    }
}
