using System.Collections;
using UnityEngine;

public class BulletSpawnEnemy : MonoBehaviour
{
    [Header("’e‚ÌÝ’è")]
    [Tooltip("’e‚Ì“®‚«‚ÌÝ’è")]
    [SerializeField] BulletState _bulletMoveState;
    [Tooltip("’e‚Ì‘¬‚³")]
    [SerializeField] float _bulletSpeed = 3f;
    [Tooltip("’e‚Ì‰ñ‚é‘¬“x")]
    [SerializeField] float _bulletRota = 1f;
    [Tooltip("’e‚Æ’e‚ÌŠÔŠu")]
    [SerializeField] float _bulletRange = 30f;
    [Tooltip("’e‚ðo‚·ƒN[ƒ‹ƒ^ƒCƒ€")]
    [SerializeField] float _spawnCoolTime = 2f;
    [SerializeField] float _bulletCoolTime = 0.2f;
    [SerializeField] float _bulletYPos = 10f;
    [Tooltip("UŒ‚ƒ^ƒCƒ€‚©‚Ç‚¤‚©")]
    //[NonSerialized]
    public bool _isAttackTime;
    [Header("’e‚Ìƒv[ƒ‹")]
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
