using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill System/Skill")]
public class SkillObjectSample : ScriptableObject
{
    [Header("スキルの名前"), SerializeField]
    string _skillName;
    public string SkillName => _skillName;
    [Header("スキルの種類"), Tooltip("Skillの種類"),SerializeField]
    SkillTypeSample _type;
    public SkillTypeSample Type => _type;
    [Header("スキルの説明"),Tooltip("Skillの説明"),SerializeField]
    [TextArea(10, 10)]
    string _description;
    public string Description => _description;
    [Header("取得に必要なスキルポイント"), Tooltip("取得に必要なスキルポイント"), SerializeField]
    int _skillPoint = 1;
    public int SkillPoint => _skillPoint;
    [Header("攻撃の倍率"), Tooltip("攻撃の倍率"), SerializeField]
    float _attackValue = 0;
    public float AttackValue => _attackValue;
    [Header("防御のバフ、デバフ"), Tooltip("Diffenceのバフ、デバフ"), SerializeField]
    float _diffencePower = 0;
    public float DiffencePower => _diffencePower;
    [Header("攻撃のバフ、デバフ"), Tooltip("Atackのバフ、デバフ"), SerializeField]
    float _attackPower = 0;
    public float AttackPower => _attackPower;
    [Header("HPの回復量"), Tooltip("HPの回復量"), SerializeField]
    int _healingHP = 0;
    public int HealingHP => _healingHP;
}
