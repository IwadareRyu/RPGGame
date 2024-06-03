using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttackScripts : MonoBehaviour
{
    List<BulletSpawnEnemy> _enemy = new List<BulletSpawnEnemy>();

    /// <summary>攻撃対象の敵のリストを返す処理。</summary>
    /// <returns></returns>
    public List<BulletSpawnEnemy> ReturnEnemy() => _enemy;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BulletSpawnEnemy>(out var enemy))
        {
            enemy.StunEnemy.ChangeStun();
            _enemy.Add(enemy);
        }   //このオブジェクトに入った敵をリストに追加。
    }
}
