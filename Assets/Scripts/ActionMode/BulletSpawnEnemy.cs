using System.Collections;
using UnityEngine;

public class BulletSpawnEnemy : MonoBehaviour
{
    [Header("弾の設定")]
    [Tooltip("弾の動きの設定")]
    [SerializeField] BulletState _bulletMoveState;
    [Tooltip("弾の速さ")]
    [SerializeField] float _bulletSpeed = 3f;
    [Tooltip("弾の回る速度")]
    [SerializeField] float _bulletRota = 1f;
    [Tooltip("弾と弾の間隔")]
    [SerializeField] float _bulletRange = 30f;
    [Tooltip("弾を出すクールタイム")]
    [SerializeField] float _spawnCoolTime = 2f;
    [SerializeField] float _bulletCoolTime = 0.2f;
    [SerializeField] float _bulletYPos = 10f;
    [Tooltip("攻撃タイムかどうか")]
    //[NonSerialized]
    public bool _isAttackTime;
    [Header("弾のプール")]
    [SerializeField] BulletPoolActive _bulletPool;
    float _coolTime;
    private void Start()
    {
        StartCoroutine(SpawnBullet());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnBullet()
    {
        while (true)
        {
            if (_isAttackTime)
            {
                _coolTime += Time.deltaTime;
                if (_coolTime > _spawnCoolTime)
                {
                    yield return StartCoroutine(CircleSpawn());
                    _coolTime = 0;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CircleSpawn()
    {
        for (float i = 0; i < 360; i += _bulletRange)
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y + _bulletYPos, transform.position.z);
            bullet.transform.Rotate(0, i, 0);
            var bulletScript = bullet.GetComponent<BulletMoveScripts>();

            switch (_bulletMoveState)
            {
                case BulletState.ForwardMove:
                    StartCoroutine(bulletScript.ForwardMove(_bulletSpeed));
                    break;
                case BulletState.RotationMove:
                    StartCoroutine(bulletScript.RotationMove(_bulletSpeed, _bulletRota));
                    break;
                case BulletState.FlowerMove:
                    StartCoroutine(bulletScript.FlowerMove(_bulletSpeed, _bulletRota));
                    break;
            }
        }
        yield return null;
    }
}
