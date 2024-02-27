using System.Collections;

public interface IBulletSpawn
{
    abstract void Spawn(BulletSpawnEnemy spawnEnemy);
    virtual IEnumerator DelaySpawn(BulletSpawnEnemy spawnEnemy) { yield return null; }
}
