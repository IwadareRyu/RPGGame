using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSpawn : IBulletSpawn
{
    /// <summary>1つのみ弾を生成</summary>
    public void Spawn(BulletSpawnEnemy spawnEnemy)
    {
        //SE再生
        AudioManager.Instance.SEPlay(SE.EnemyShot);
        //弾生成
        spawnEnemy.InitBullet(spawnEnemy.transform.rotation.eulerAngles.y);
    }
}
