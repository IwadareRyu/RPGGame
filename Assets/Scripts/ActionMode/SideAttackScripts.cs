using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttackScripts : MonoBehaviour
{
    List<BulletSpawnEnemy> _enemy = new List<BulletSpawnEnemy>();

    public List<BulletSpawnEnemy> ReturnEnemy()
    {
        return _enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BulletSpawnEnemy>(out var enemy))
        {
            _enemy.Add(enemy);
        }
    }
}
