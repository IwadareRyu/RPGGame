using System.Collections;
using UnityEngine;

public class BulletSpawnEnemy : MonoBehaviour
{
    PlayerController _player;
    Rigidbody _rb;
    [SerializeField] Animator _anim;
    [Header("弾の設定")]
    [Tooltip("弾の動きの設定")]
    [SerializeField] BulletState _bulletMoveState;
    [SerializeField] SpawnState _spawnBulletState = SpawnState.Circle;
    [Tooltip("弾の速さ")]
    [SerializeField] float _bulletSpeed = 3f;
    [Tooltip("弾の回る速度")]
    [SerializeField] float _bulletRota = 1f;
    [Tooltip("弾と弾の間隔")]
    [SerializeField] float _bulletRange = 30f;
    public float BulletRange => _bulletRange;
    [Header("弾のスポーンスパンの設定")]
    [Tooltip("弾を出すクールタイム")]
    [SerializeField] float _spawnCoolTime = 2f;
    [SerializeField] float _bulletYPos = 10f;
    [Tooltip("攻撃タイムかどうか")]
    //[NonSerialized]
    public bool _isAttackTime;
    [SerializeField] StunEnemy _stunEnemy;
    public StunEnemy StunEnemy => _stunEnemy;
    [Header("弾のプール")]
    [SerializeField] BulletPool _bulletPool;
    float _coolTime;
    Coroutine _activeCoroutine;

    ForwardSpawn _forwardSpawn = new();
    [SerializeField] CircleSpawn _circleSpawn = new();

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
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
        if (_isAttackTime) { _activeCoroutine = StartCoroutine(SpawnBullet()); }
        else if(_activeCoroutine != null){ StopCoroutine(_activeCoroutine); }
    }

    IEnumerator SpawnBullet()
    {
        while (true)
        {
            if (!_stunEnemy.Stun)
            {
                _coolTime += Time.deltaTime;
                if (_coolTime > _spawnCoolTime)
                {
                    _anim.SetTrigger("Attack");
                    switch (_spawnBulletState)
                    {
                        // 一つだけ弾を生成
                        case SpawnState.Forward:
                            _forwardSpawn.Spawn(this);
                            break;
                        //360度均等に弾を生成
                        case SpawnState.Circle:
                            _circleSpawn.Spawn(this);
                            break;
                        //360度均等に一つずつ弾を生成
                        case SpawnState.DelayCircle:
                            yield return StartCoroutine(_circleSpawn.DelaySpawn(this));
                            break;
                    }
                    _coolTime = 0;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>弾の生成</summary>
    public void InitBullet(float rota)
    {
        //Poolから取り出す
        var bullet = _bulletPool.GetBullet();
        //弾の初期位置、向きの設定
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y + _bulletYPos, transform.position.z);
        bullet.transform.Rotate(0, rota, 0);
        // 弾の動きを設定する
        var bulletScript = bullet.GetComponent<BulletMoveScripts>();
        bulletScript.BulletMoveStart(_bulletSpeed, _bulletRota, _bulletMoveState);
    }
}
