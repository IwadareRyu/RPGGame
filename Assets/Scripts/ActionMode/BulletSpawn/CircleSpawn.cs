using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CircleSpawn : IBulletSpawn
{
    [SerializeField] float _delaySpawnCoolTime = 0.1f;

    /// <summary>360度均等に弾を生成</summary>
    public void Spawn(BulletSpawnEnemy spawnEnemy)
    {
        //SE再生
        AudioManager.Instance.SEPlay(SE.EnemyShot);
        //360度均等に弾を生成
        for (float i = 0; i < 360; i += spawnEnemy.BulletRange)
        {
            //弾を生成
            spawnEnemy.InitBullet(i);
        }
    }

    /// <summary>360度均等に1つずつ球を生成</summary>
    public IEnumerator DelaySpawn(BulletSpawnEnemy spawnEnemy)
    {
        //360度均等に弾を生成
        for (float i = 0; i < 360; i += spawnEnemy.BulletRange)
        {
            // SEを鳴らす。
            AudioManager.Instance.SEPlay(SE.EnemyShot);
            //弾を生成。
            spawnEnemy.InitBullet(i);
            //何秒間か待ってからBulletRange度ずらして弾を生成する。
            yield return new WaitForSeconds(_delaySpawnCoolTime);
        }
    }
}
