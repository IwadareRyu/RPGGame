using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObjects : ScriptableObject
{
    [Header("スキルの名前"),SerializeField]
    string _skillName;
    public string SkillName => _skillName;
    [Header("スキルの種類"),Tooltip("Skillの種類")] 
    public SkillType _type;
    [TextArea(10, 10)] 
    public string _description;
}
