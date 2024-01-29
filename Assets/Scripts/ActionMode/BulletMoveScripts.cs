using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMoveScripts : MonoBehaviour
{
    Transform _myTrans;
    float _rotaTime = 0.1f;
    [SerializeField] float _activeTime = 5f;
    float _time = 0f;
    [SerializeField] float _bulletScale = 3f;

    private void Awake()
    {
        _myTrans = GetComponent<Transform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,_bulletScale);
    }

    /// <summary>まっすぐ飛ぶ球の処理</summary>
    /// <param name="speed"></param>
    public IEnumerator ForwardMove(float speed)
    {
        for(float i = 0f;i <= _activeTime;i += Time.deltaTime)
        {
            Move(speed);
            if (ChackHit()) {  break; }
            yield return new WaitForFixedUpdate();
        }
        Reset();
    }

    /// <summary>曲がる球の処理</summary>
    /// <param name="speed"></param>
    public IEnumerator RotationMove(float speed,float rota)
    {
        for (float i = 0f; i <= _activeTime; i += Time.deltaTime)
        {
            Rotation(rota);
            Move(speed);
            if (ChackHit()) { break; }
            yield return new WaitForFixedUpdate();
        }
        Reset();
    }

    /// <summary>弾が動く</summary>
    /// <param name="speed"></param>
    public IEnumerator FlowerMove(float speed, float rota)
    {
        float tmpRota = 0;
        float nowRota = 0;
        for (float i = 0f; i <= _activeTime; i += Time.deltaTime)
        {
            if (nowRota < tmpRota + 270)
            {
                Rotation(rota);
                nowRota += rota;
            }
            Move(speed);
            if (ChackHit()) { break; };
            yield return new WaitForFixedUpdate();
        }
        Reset();
    }

    /// <summary>弾が回る</summary>
    /// <param name="speed"></param>
    private void Move(float speed)
    {
        transform.position += transform.forward * speed;
    }
    
    private void Rotation(float rota)
    {
        _time += Time.deltaTime;
        if (_time < _rotaTime)
        {
            transform.Rotate(0, rota, 0);
            _time = 0;
        }
    }

    /// <summary>当たり判定</summary>
    private bool ChackHit()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _bulletScale);
        foreach(var col in cols)
        {
            if(col.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>リセット</summary>
    private void Reset()
    {
        _time = 0;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
