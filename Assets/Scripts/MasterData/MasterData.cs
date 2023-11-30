using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MasterData
{
    [Serializable]
    public class Skill
    {
        public int ID;
        public string SkillName;
        public string Description;
        public int AttackValue;
        public int RequaireAttack;
        public int SkillPoint;
        public int HealingHP;
        public int DiffencePower;
        public int OffencePower;
        public string Adjacent;
        public string TmpName;
        public SkillType SkillType;
    }

    [Serializable]
    public class MasterDataClass<T>
    {
        public string Version;
        public T[] Data;
    }
}
