using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;
[Serializable]
public class SkillObjects : ScriptableObject
{
    [Header("�X�L����ID"),SerializeField]
    int _skillID;
    public int SkillID => _skillID;
    [Header("�X�L���̖��O"), SerializeField]
    string _skillName;
    public string SkillName => _skillName;
    [Header("�X�L���̎��"), Tooltip("Skill�̎��")]
    SkillType _type;
    public SkillType Type => _type;
    [TextArea(10, 10)]
    public string _description;
    public string Description => _description;

    public void SkillObjectsLoad(Skill skillobj)
    {
        _skillID = skillobj.ID;
        _skillName = skillobj.SkillName;
        _type = skillobj.SkillType;
        _description = skillobj.Description;
    }
}