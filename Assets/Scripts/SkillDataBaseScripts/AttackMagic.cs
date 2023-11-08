using MasterData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackMagic", menuName = "Skill System/AttackMagic")]
public class AttackMagic : SkillObjects
{
    [SerializeField] float _attackValue;
    public float AttackValue => _attackValue;
    [SerializeField] int _skillPoint;
    public int SkillPoint => _skillPoint;

    int[] _adjacent;
    public int[] Adjacent => _adjacent;

    public void AttackMagicLoad(Skill skillobj)
    {
        SkillObjectsLoad(skillobj);
        _attackValue = skillobj.AttackValue;
        _skillPoint = skillobj.SkillPoint;
        if (skillobj.Adjacent != "null")
        {
            _adjacent = Array.ConvertAll(skillobj.Adjacent.Split(), int.Parse);
        }
    }
}