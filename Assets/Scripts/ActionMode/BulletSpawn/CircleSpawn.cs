using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CircleSpawn : IBulletSpawn
{
    [SerializeField] float _delaySpawnCoolTime = 0.1f;

    public IEnumerator DelaySpawn(BulletSpawnEnemy spawnEnemy)
    {
        for (float i = 0; i < 360; i += spawnEnemy.BulletRange)
        {
            spawnEnemy.InitBullet(i);
            yield return new WaitForSeconds(_delaySpawnCoolTime);
        }
    }

    public void Spawn(BulletSpawnEnemy spawnEnemy)
    {
        for (float i = 0; i < 360; i += spawnEnemy.BulletRange)
        {
            spawnEnemy.InitBullet(i);
        }
    }
}
