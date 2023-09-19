using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MasterData
{
    [Serializable]
    public class AttackSkill
    {
        public int ID;
        public string SkillName;
        public string Description;
        public int AttackValue;
        public int RequaireAttack;
    }

    [Serializable]
    public class MasterDataClass<T>
    {
        public string Version;
        public T[] Data;
    }
}
