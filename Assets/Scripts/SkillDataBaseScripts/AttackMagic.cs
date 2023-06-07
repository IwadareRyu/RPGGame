using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackMagic", menuName = "Skill System/AttackMagic")]
public class AttackMagic : SkillObjects
{
    
    [SerializeField] float _attackValue = 1;
    [SerializeField] int _skillPoint = 1;
    private void Awake()
    {
        _type = SkillType.AttackMagic;
    }
}
