using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSpawn : IBulletSpawn
{
    public void Spawn(BulletSpawnEnemy spawnEnemy)
    {
        spawnEnemy.InitBullet(spawnEnemy.transform.rotation.eulerAngles.y);
    }
}
