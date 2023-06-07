using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObjects : ScriptableObject
{
    [Header("�X�L���̖��O"),SerializeField]
    string _skillName;
    public string SkillName => _skillName;
    [Header("�X�L���̎��"),Tooltip("Skill�̎��")] 
    public SkillType _type;
    [TextArea(10, 10)] 
    public string _description;
}
