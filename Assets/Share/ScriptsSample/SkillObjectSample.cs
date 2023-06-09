using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill System/Skill")]
public class SkillObjectSample : ScriptableObject
{
    [Header("�X�L���̖��O"), SerializeField]
    string _skillName;
    public string SkillName => _skillName;
    [Header("�X�L���̎��"), Tooltip("Skill�̎��"),SerializeField]
    SkillTypeSample _type;
    public SkillTypeSample Type => _type;
    [Header("�X�L���̐���"),Tooltip("Skill�̐���"),SerializeField]
    [TextArea(10, 10)]
    string _description;
    public string Description => _description;
    [Header("�擾�ɕK�v�ȃX�L���|�C���g"), Tooltip("�擾�ɕK�v�ȃX�L���|�C���g"), SerializeField]
    int _skillPoint = 1;
    public int SkillPoint => _skillPoint;
    [Header("�U���̔{��"), Tooltip("�U���̔{��"), SerializeField]
    float _attackValue = 0;
    public float AttackValue => _attackValue;
    [Header("�h��̃o�t�A�f�o�t"), Tooltip("Diffence�̃o�t�A�f�o�t"), SerializeField]
    float _diffencePower = 0;
    public float DiffencePower => _diffencePower;
    [Header("�U���̃o�t�A�f�o�t"), Tooltip("Atack�̃o�t�A�f�o�t"), SerializeField]
    float _attackPower = 0;
    public float AttackPower => _attackPower;
    [Header("HP�̉񕜗�"), Tooltip("HP�̉񕜗�"), SerializeField]
    int _healingHP = 0;
    public int HealingHP => _healingHP;
}
