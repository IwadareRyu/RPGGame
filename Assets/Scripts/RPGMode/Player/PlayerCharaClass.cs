using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharaClass : StatusClass
{
    public void LifePointSet(float hp)
    {
        int lifePoint = (int)hp / 10;
    }

    public override void AddDamage(float damage, float skillParsent = 1)
    {
        AddLifeDamage(damage * skillParsent);
    }

    void AddLifeDamage(float damage)
    {
        
    }



}
