using System.Collections;
using UnityEngine;

public class BulletSpawnEnemy : MonoBehaviour
{
    PlayerController _player;
    Rigidbody _rb;
    [SerializeField] Animator _anim;
    [Header("�e�̐ݒ�")]
    [Tooltip("�e�̓����̐ݒ�")]
    [SerializeField] BulletState _bulletMoveState;
    [SerializeField] SpawnState _spawnBulletState = SpawnState.Circle;
    [Tooltip("�e�̑���")]
    [SerializeField] float _bulletSpeed = 3f;
    [Tooltip("�e�̉�鑬�x")]
    [SerializeField] float _bulletRota = 1f;
    [Tooltip("�e�ƒe�̊Ԋu")]
    [SerializeField] float _bulletRange = 30f;
    [Header("�e�̃X�|�[���X�p���̐ݒ�")]
    [Tooltip("�e���o���N�[���^�C��")]
    [SerializeField] float _spawnCoolTime = 2f;
    [SerializeField] float _delaySpawnCoolTime = 0.2f;
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
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(SpawnBullet());
    }

    public void Init()
    {
        _rb.isKinematic = true;
        _isAttackTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stunEnemy.Stun && _isAttackTime)
        {
            var playerPos = _player.transform.position;
            playerPos.y = transform.position.y;
            transform.LookAt(_player.transform.position);
        }
    }

    public void ChangeAttackTime()
    {
        _rb.isKinematic = !_rb.isKinematic;
        _isAttackTime = !_isAttackTime;
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
                    _anim.SetTrigger("Attack");
                    switch (_spawnBulletState)
                    {
                        case SpawnState.Forward:
                            ForwardSpawn();
                            break;
                        case SpawnState.Circle:
                            CircleSpawn();
                            break;
                        case SpawnState.DelayCircle:
                            yield return DelayCircleSpawn();
                            break;
                    }
                    _coolTime = 0;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    void ForwardSpawn()
    {
        InitBullet(transform.rotation.eulerAngles.y);
    }

    void CircleSpawn()
    {
        for (float i = 0; i < 360; i += _bulletRange)
        {
            InitBullet(i);
        }
    }

    IEnumerator DelayCircleSpawn()
    {
        for(float i = 0;i < 360;i += _bulletRange)
        {
            InitBullet(i);
            yield return new WaitForSeconds(_delaySpawnCoolTime);
        }
    }

    void InitBullet(float rota)
    {
        var bullet = _bulletPool.GetBullet();
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y + _bulletYPos, transform.position.z);
        bullet.transform.Rotate(0, rota, 0);
        var bulletScript = bullet.GetComponent<BulletMoveScripts>();
        bulletScript.BulletMoveStart(_bulletSpeed, _bulletRota, _bulletMoveState);
    }
}
