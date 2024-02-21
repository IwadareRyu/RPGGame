using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;
[Serializable]
public class SkillObjects : ScriptableObject
{
    [Header("スキルのID"),SerializeField]
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

    public void SkillObjectsLoad(Skill skillobj)
    {
        _skillID = skillobj.ID;
        _skillName = skillobj.SkillName;
        _type = skillobj.SkillType;
        _description = skillobj.Description;
    }
}