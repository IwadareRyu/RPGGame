using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class NormalAttack : EnemyInterface
{
    [SerializeField] float _attackCoolTime = 3f;
    [SerializeField] int _attackCount = 1;
    bool _endAttackTime = false;

    public bool TimeBoolean(EnemyController enemy, ref float coolTime)
    {
        coolTime += Time.deltaTime;
        enemy.ChantingViewAccess(coolTime, _attackCoolTime);
        if (coolTime > _attackCoolTime) return true;
        return false;
    }

    public IEnumerator AttackTime(EnemyController enemy)
    {
        enemy._anim.SetBool("Attack", true);
        enemy.TargetChange();
        yield return new WaitUntil(() => _endAttackTime);
        enemy._anim.SetBool("Attack", false);
        _endAttackTime = false;
    }

    public bool EndAttackBoolean(EnemyController enemy)
    {
        if (enemy.CurrentAttackCount >= _attackCount)
        {
            _endAttackTime = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
