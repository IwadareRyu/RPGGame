using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnim : MonoBehaviour
{
    [SerializeField] EnemyController _enemy;

    public void AttackDamageTiming()
    {
        _enemy.EnemyAttack();
    }
}
