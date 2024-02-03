using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttackScripts : MonoBehaviour
{
    List<StunEnemy> _enemy = new List<StunEnemy>();

    public List<StunEnemy> ReturnEnemy()
    {
        return _enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<StunEnemy>(out var enemy))
        {
            enemy.ChangeStun();
            _enemy.Add(enemy);
        }
    }
}
