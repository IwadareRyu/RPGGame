using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountEnemy : MonoBehaviour
{
    [SerializeField] EnemyTypeState _enemyType = EnemyTypeState.Skeleton;
    public EnemyTypeState EnemyType => _enemyType;
}
