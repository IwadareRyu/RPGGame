using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TwiceAttack : EnemyInterface
{
    [SerializeField] float _attackCoolTime = 5f;
    public IEnumerator AttackTime(EnemyController enemy)
    {
        yield return null;
    }

    public bool EndAttackBoolean(EnemyController enemy)
    {
        return false;
    }

    public bool TimeBoolean(EnemyController enemy,ref float coolTime)
    {
        coolTime += Time.deltaTime;
        enemy.ChantingViewAccess(coolTime, _attackCoolTime);
        if (coolTime > _attackCoolTime) return true;
        return false;
    }
}
