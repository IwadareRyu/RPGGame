using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttackScripts : MonoBehaviour
{
    List<StunEnemy> _enemy = new List<StunEnemy>();

    /// <summary>攻撃対象の敵のリストを返す処理。</summary>
    /// <returns></returns>
    public List<StunEnemy> ReturnEnemy() => _enemy;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<StunEnemy>(out var enemy))
        {
            enemy.ChangeStun();
            _enemy.Add(enemy);
        }   //このオブジェクトに入った敵をリストに追加。
    }
}
