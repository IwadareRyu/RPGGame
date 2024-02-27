using UnityEngine;

public class BulletMoveScripts : MonoBehaviour
{
    float _rotaTime = 0.1f;
    [SerializeField] float _activeTime = 5f;
    public float ActiveTime => _activeTime;
    float _time = 0f;
    [SerializeField] float _bulletScale = 3f;
    public float BulletScale => _bulletScale;

    ForwardMove _forwardMove = new();
    RotationMove _rotationMove = new();
    HalfRotationMove _halfRotationMove = new();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _bulletScale);
    }

    #region 弾の種類
    public void BulletMoveStart(float bulletSpeed, float bulletRota, BulletState bulletMoveState)
    {
        switch (bulletMoveState)
        {
            case BulletState.ForwardMove:
                StartCoroutine(_forwardMove.BulletMove(this, bulletSpeed));
                break;
            case BulletState.RotationMove:
                StartCoroutine(_rotationMove.BulletMove(this, bulletSpeed, bulletRota));
                break;
            case BulletState.HalfRotationMove:
                StartCoroutine(_halfRotationMove.BulletMove(this, bulletSpeed, bulletRota));
                break;
        }
    }
    #endregion

    #region 弾の動きの処理
    /// <summary>弾がTransformで動く処理</summary>
    /// <param name="speed"></param>
    public void Move(float speed)
    {
        transform.position += transform.forward * speed;
        //transform.Translate(transform.forward * speed);
    }

    /// <summary>回転処理</summary>
    /// <param name="rota"></param>
    public void Rotation(float rota)
    {
        _time += Time.deltaTime;
        if (_time < _rotaTime)
        {
            transform.Rotate(0, rota, 0);
            _time = 0;
        }
    }

    /// <summary>当たり判定処理</summary>
    public bool ChackHit()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _bulletScale);
        foreach (var col in cols)
        {
            if (col.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    /// <summary>リセット</summary>
    public void Reset()
    {
        _time = 0;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
