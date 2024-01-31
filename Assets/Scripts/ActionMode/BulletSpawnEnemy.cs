using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawnEnemy : MonoBehaviour
{
    /*[SerializeField] */
    PlayerController _player;
    [Header("íeÇÃê›íË")]
    [Tooltip("íeÇÃìÆÇ´ÇÃê›íË")]
    [SerializeField] BulletState _bulletMoveState;
    [Tooltip("íeÇÃë¨Ç≥")]
    [SerializeField] float _bulletSpeed = 3f;
    [Tooltip("íeÇÃâÒÇÈë¨ìx")]
    [SerializeField] float _bulletRota = 1f;
    [Tooltip("íeÇ∆íeÇÃä‘äu")]
    [SerializeField] float _bulletRange = 30f;
    [Tooltip("íeÇèoÇ∑ÉNÅ[ÉãÉ^ÉCÉÄ")]
    [SerializeField] float _spawnCoolTime = 2f;
    [SerializeField] float _bulletCoolTime = 0.2f;
    [SerializeField] float _bulletYPos = 10f;
    [Tooltip("çUåÇÉ^ÉCÉÄÇ©Ç«Ç§Ç©")]
    //[NonSerialized]
    public bool _isAttackTime;
    [Header("íeÇÃÉvÅ[Éã")]
    [SerializeField] BulletPoolActive _bulletPool;
    float _coolTime;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        StartCoroutine(SpawnBullet());
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = _player.transform.position;
        playerPos.y = transform.position.y;
        transform.LookAt(_player.transform.position);
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
            bulletScript.BulletMoveStart(_bulletSpeed,_bulletRota, _bulletMoveState);
            //switch (_bulletMoveState)
            //{
            //    case BulletState.ForwardMove:
            //        StartCoroutine(bulletScript.ForwardMove(_bulletSpeed));
            //        break;
            //    case BulletState.RotationMove:
            //        StartCoroutine(bulletScript.RotationMove(_bulletSpeed, _bulletRota));
            //        break;
            //    case BulletState.FlowerMove:
            //        StartCoroutine(bulletScript.FlowerMove(_bulletSpeed, _bulletRota));
            //        break;
            //}
        }
        yield return null;
    }
}
