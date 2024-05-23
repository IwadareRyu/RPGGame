using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface EnemyInterface
{
    public abstract bool TimeBoolean(EnemyController enemy,ref float coolTime);

    public abstract IEnumerator AttackTime(EnemyController enemy);

    public abstract bool EndAttackBoolean(EnemyController enemy);
}
