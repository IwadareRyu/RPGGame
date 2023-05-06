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
    [Tooltip("ディフェンス職の状態")]
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
    [Tooltip("Counterの際、コルーチンを適切に動かすためのbool")]
    bool _counterTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Qボタンで左に移動して状態がCoolLeftCounterになる。
        if (Input.GetButton("BlockLeft") && 
            _condition != BlockorAttack.CoolTime &&
            _condition != BlockorAttack.RightBlock)
        {
            CoolCounter(1,BlockorAttack.CoolLeftCounter);
        }
        //Eボタンで右に移動して状態がCoolRightCounterになる。
        else if (Input.GetButton("BlockRight") &&
            _condition != BlockorAttack.CoolTime &&
            _condition != BlockorAttack.LeftBlock)
        {
            CoolCounter(2, BlockorAttack.CoolRightCounter);
        }
        //ニュートラルでカウンター状態じゃない限り、真ん中に移動して状態がAttackになる。
        else if(_condition != BlockorAttack.CoolLeftCounter && 
            _condition != BlockorAttack.CoolRightCounter && 
            _condition != BlockorAttack.Counter)
        {
            DistanceMove(BlockorAttack.Attack);
        }

        //CoolLeftCounterかCoolRightCounter時に実行
        if(_condition == BlockorAttack.CoolLeftCounter || 
            _condition == BlockorAttack.CoolRightCounter)
        {
            if(!_counterTime)
            {
                Debug.Log(_condition);
                _counterTime = true;
                StartCoroutine(CoolCounterTime());
            }
        }

        //LeftBlockかRightBlockの時に実行
        if(_condition == BlockorAttack.LeftBlock || 
            _condition == BlockorAttack.RightBlock)
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
            Debug.Log(_condition);
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
            Debug.Log(_condition);
            _guageAttack += 5;
        }
        else if(_condition == BlockorAttack.RightBlock)
        {
            Debug.Log(_condition);
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
            Debug.Log(_condition);
            _guageAttack += 1;
        }
        _attackTime = false;
    }

    /// <summary>距離を計算して特定の位置へ移動するメソッド。移動し終わったら状態変化</summary>
    /// <param name="i">配列の要素数</param>
    /// <param name="state">状態変化</param>
    void DistanceMove(BlockorAttack state = BlockorAttack.LeftBlock)
    {
        if (_condition != BlockorAttack.ChageAttack)
        {
            float distance = Vector2.Distance(transform.position, _trans[0].position);
            if (distance > _stopdis)
            {
                _condition = BlockorAttack.CoolTime;
                Vector3 dir = (_trans[0].position - transform.position).normalized * _speed;
                dir.y = 0;
                transform.Translate(dir * Time.deltaTime);
            }
            else
            {
                _condition = state;
            }
        }
    }

    /// <summary>カウンター待機にするための処理</summary>
    /// <param name="i"></param>
    /// <param name="counter"></param>
    void CoolCounter(int i = 1,BlockorAttack counter = BlockorAttack.CoolLeftCounter)
    {
        if (_condition == BlockorAttack.Attack)
        {
            transform.position = _trans[i].position;
            _condition = counter;
            Debug.Log("カウンター準備");
        }
    }

    public void TrueCounter()
    {
        if (_condition == BlockorAttack.CoolLeftCounter || 
            _condition == BlockorAttack.CoolRightCounter)
        {
            var tmpCondition = _condition;
            _condition = BlockorAttack.Counter;
            StartCoroutine(TrueCounrerTime(tmpCondition));
        }
    }

    IEnumerator TrueCounrerTime(BlockorAttack tmp)
    {
        //カウンターの処理//
        Debug.Log("カウンター攻撃！");
        //終わり//
        yield return new WaitForSeconds(2f);
        if(tmp == BlockorAttack.CoolLeftCounter)
        {
            _condition = BlockorAttack.LeftBlock;
        }
        else
        {
            _condition = BlockorAttack.RightBlock;
        }
    }

    IEnumerator CoolCounterTime()
    {
        yield return new WaitForSeconds(2f);
        if (_condition == BlockorAttack.CoolLeftCounter || 
            _condition == BlockorAttack.CoolRightCounter)
        {
            Debug.Log("カウンター失敗、防御態勢へ移行");
            if (_condition == BlockorAttack.CoolLeftCounter)
            {
                _condition = BlockorAttack.LeftBlock;
            }
            else
            {
                _condition = BlockorAttack.RightBlock;
            }
        }
        _counterTime = false;
    }

    /// <summary>BlockPlayerの状態</summary>
    public enum BlockorAttack
    {
        [Tooltip("移動している間のCoolTime")]
        CoolTime,
        [Tooltip("左方向のガード")]
        LeftBlock,
        [Tooltip("左方向のカウンター待機")]
        CoolLeftCounter,
        [Tooltip("右方向のガード")]
        RightBlock,
        [Tooltip("右方向のカウンター待機")]
        CoolRightCounter,
        [Tooltip("カウンター")]
        Counter,
        [Tooltip("攻撃")]
        Attack,
        [Tooltip("チャージ攻撃")]
        ChageAttack,
    }
}
