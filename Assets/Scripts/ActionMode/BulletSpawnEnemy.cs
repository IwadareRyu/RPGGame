using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawnEnemy : MonoBehaviour
{
    /*[SerializeField] */
    PlayerController _player;
    [SerializeField] Animator _anim;
    [Header("�e�̐ݒ�")]
    [Tooltip("�e�̓����̐ݒ�")]
    [SerializeField] BulletState _bulletMoveState;
    [Tooltip("�e�̑���")]
    [SerializeField] float _bulletSpeed = 3f;
    [Tooltip("�e�̉�鑬�x")]
    [SerializeField] float _bulletRota = 1f;
    [Tooltip("�e�ƒe�̊Ԋu")]
    [SerializeField] float _bulletRange = 30f;
    [Tooltip("�e���o���N�[���^�C��")]
    [SerializeField] float _spawnCoolTime = 2f;
    [SerializeField] float _bulletCoolTime = 0.2f;
    [SerializeField] float _bulletYPos = 10f;
    [Tooltip("�U���^�C�����ǂ���")]
    //[NonSerialized]
    public bool _isAttackTime;
    [SerializeField] StunEnemy _stunEnemy;
    [Header("�e�̃v�[��")]
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
        if (!_stunEnemy.Stun)
        {
            var playerPos = _player.transform.position;
            playerPos.y = transform.position.y;
            transform.LookAt(_player.transform.position);
        }
    }

    IEnumerator SpawnBullet()
    {
        while (true)
        {
            if (_isAttackTime && !_stunEnemy.Stun)
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
        _anim.SetTrigger("Attack");
        for (float i = 0; i < 360; i += _bulletRange)
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y + _bulletYPos, transform.position.z);
            bullet.transform.Rotate(0, i, 0);
            var bulletScript = bullet.GetComponent<BulletMoveScripts>();
            bulletScript.BulletMoveStart(_bulletSpeed,_bulletRota, _bulletMoveState);
        }
        yield return null;
    }
}