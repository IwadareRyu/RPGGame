using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockPlayer : MonoBehaviour
{
    [Tooltip("BlockPlayerの移動場所")]
    [SerializeField]
    Transform[] _trans;
    [Tooltip("ゲージアタックの値")]
    [SerializeField]
    float _guageAttack = 0;
    [Tooltip("BlockPlayerの状態")]
    BlockorAttack _condition = BlockorAttack.Attack;
    public BlockorAttack Condition => _condition;
    [Tooltip("移動の際止まる力")]
    float _stopdis = 0.1f;
    [Tooltip("移動スピード")]
    float _speed = 6f;
    [Tooltip("Blockの際、コルーチンを適切に動かすためのbool")]
    bool _blockTime;
    [Tooltip("Attackの際、コルーチンを適切に動かすためのbool")]
    bool _attackTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Qボタンで左に移動して状態がLeftBlockになる。
        if (Input.GetButton("BlockLeft"))
        {
            DistanceMove(1);
        }
        //Eボタンで右に移動して状態がRightBlockになる。
        else if (Input.GetButton("BlockRight"))
        {
            DistanceMove(2,BlockorAttack.RightBlock);
        }
        //ニュートラルで真ん中に移動して状態がAttackになる。
        else
        {
            DistanceMove(0,BlockorAttack.Attack);
        }

        //LeftBlockかRightBlockの時に実行
        if(_condition == BlockorAttack.LeftBlock || _condition == BlockorAttack.RightBlock)
        {
            if (!_blockTime)
            {
                _blockTime = true;
                StartCoroutine(BlockTime());
            }
        }

        //Attackの時に実行
        if(_condition == BlockorAttack.Attack)
        {
            //チャージが100以上になったら状態をチャージアタックに変える。
            if (_guageAttack >= 100)
            {
                _condition = BlockorAttack.ChageAttack;
            }
            else if(!_attackTime)
            {
                _attackTime = true;
                StartCoroutine(AttackTime());
            }
        }

        //ChageAttackの時に実行
        if(_condition == BlockorAttack.ChageAttack)
        {
            //チャージアタックをした後、ゲージを０にして、Attack状態に戻る。
            Debug.Log("ChageAttack!");
            _guageAttack = 0;
            _condition = BlockorAttack.Attack;
        }
    }

    /// <summary>ブロックをしている際のコルーチン</summary>
    /// <returns></returns>
    IEnumerator BlockTime()
    {
        //一秒後にそれぞれのブロックの処理どちらかを実行。
        yield return new WaitForSeconds(1f);
        if (_condition == BlockorAttack.LeftBlock)
        {
            Debug.Log("LeftBlock");
            _guageAttack += 5;
        }
        else if(_condition == BlockorAttack.RightBlock)
        {
            Debug.Log("RightBlock");
            _guageAttack += 5;
        }
        _blockTime = false;
    }

    /// <summary>アタックをしている際のコルーチン</summary>
    /// <returns></returns>
    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(2f);
        if (_condition == BlockorAttack.Attack)
        {
            Debug.Log("Attack");
            _guageAttack += 1;
        }
        _attackTime = false;
    }

    /// <summary>距離を計算して特定の位置へ移動するメソッド。移動し終わったら状態変化</summary>
    /// <param name="i">配列の要素数</param>
    /// <param name="state">状態変化</param>
    void DistanceMove(int i,BlockorAttack state = BlockorAttack.LeftBlock)
    {
        if (_condition != BlockorAttack.ChageAttack)
        {
            float distance = Vector2.Distance(transform.position, _trans[i].position);
            if (distance > _stopdis)
            {
                _condition = BlockorAttack.CoolTime;
                Vector3 dir = (_trans[i].position - transform.position).normalized * _speed;
                dir.y = 0;
                transform.Translate(dir * Time.deltaTime);
            }
            else
            {
                _condition = state;
            }
        }
    }

    /// <summary>BlockPlayerの状態</summary>
    public enum BlockorAttack
    {
        [Tooltip("移動している間のCoolTime")]
        CoolTime,
        [Tooltip("左のガード")]
        LeftBlock,
        [Tooltip("右のガード")]
        RightBlock,
        [Tooltip("攻撃")]
        Attack,
        [Tooltip("チャージ攻撃")]
        ChageAttack,
    }
}
