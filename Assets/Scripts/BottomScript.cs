using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomScript : MonoBehaviour
{
    public int _skillNo = -1;

    public void SkillNo_Sansyo()
    {
        ItemInventory.instance.SelectSkill(_skillNo);
    }
}
