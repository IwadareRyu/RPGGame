using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSpawn : IBulletSpawn
{
    public void Spawn(BulletSpawnEnemy spawnEnemy)
    {
        AudioManager.Instance.SEPlay(SE.EnemyShot);
        spawnEnemy.InitBullet(spawnEnemy.transform.rotation.eulerAngles.y);
    }
}
