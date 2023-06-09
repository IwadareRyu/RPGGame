using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPlayer : StatusClass
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
    [Tooltip("CoolTimeの際、コルーチンを適切に動かすためのbool")]
    bool _coolTimebool;
    [Tooltip("状態のテキスト")]
    [SerializeField] Text _enumtext;
    [SerializeField] GameObject _blockObj;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<EnemyController>();
        SetStatus();
        ShowSlider();
        Debug.Log($"BlockerHP:{HP}");
        Debug.Log($"BlockerAttack:{Attack}");
        Debug.Log($"BlockerDiffence:{Diffence}");
    }

    // Update is called once per frame
    void Update()
    {
        if (_survive == Survive.Survive)
        {
            //Qボタンで左に移動して状態がCoolLeftCounterになる。
            if (Input.GetButton("BlockLeft") &&
                _condition != BlockorAttack.CoolTime &&
                _condition != BlockorAttack.RightBlock &&
                _condition != BlockorAttack.CoolRightCounter &&
                _condition != BlockorAttack.Counter)
            {
                CoolCounter(1, BlockorAttack.CoolLeftCounter);
            }
            //Eボタンで右に移動して状態がCoolRightCounterになる。
            else if (Input.GetButton("BlockRight") &&
                _condition != BlockorAttack.CoolTime &&
                _condition != BlockorAttack.LeftBlock &&
                _condition != BlockorAttack.CoolLeftCounter &&
                _condition != BlockorAttack.Counter)
            {
                CoolCounter(2, BlockorAttack.CoolRightCounter);
            }
            //ニュートラルでカウンター状態じゃない限り、真ん中に移動して状態がAttackになる。
            else if (_condition != BlockorAttack.CoolLeftCounter &&
                _condition != BlockorAttack.CoolRightCounter &&
                _condition != BlockorAttack.Counter &&
                _condition != BlockorAttack.Attack)
            {
                if (!_coolTimebool)
                {
                    _coolTimebool = true;
                    CoolTime(BlockorAttack.Attack);
                }
            }

            //CoolLeftCounterかCoolRightCounter時に実行
            if (_condition == BlockorAttack.CoolLeftCounter ||
                _condition == BlockorAttack.CoolRightCounter)
            {
                if (!_counterTime)
                {
                    _counterTime = true;
                    StartCoroutine(CoolCounterTime());
                }
            }

            //LeftBlockかRightBlockの時に実行
            if (_condition == BlockorAttack.LeftBlock ||
                _condition == BlockorAttack.RightBlock)
            {
                if (!_blockTime)
                {
                    _blockTime = true;
                    StartCoroutine(BlockTime());
                }
            }

            //Attackの時に実行
            if (_condition == BlockorAttack.Attack)
            {
                //チャージが100以上になったら状態をチャージアタックに変える。
                if (_guageAttack >= 100)
                {
                    _condition = BlockorAttack.ChageAttack;
                    ShowText("チャージアタック");
                }
                else if (!_attackTime)
                {
                    _attackTime = true;
                    StartCoroutine(AttackTime());
                }
            }

            //ChageAttackの時に実行
            if (_condition == BlockorAttack.ChageAttack)
            {
                //チャージアタックをした後、ゲージを０にして、Attack状態に戻る。
                var set = DataBase.BlockSkillData[DataBase._blockSkillSetNo[1]];
                Debug.Log(set.SkillName);
                _enemy.AddDebuffDamage(Attack, set.AttackValue * 5, set.EnemyOffencePower * 5, set.EnemyDiffencePower * 5);
                _guageAttack = 0;
                _condition = BlockorAttack.Attack;
            }

            TimeMethod();

            if(_hp == 0)
            {
                _survive = Survive.Death;
            }
        }
        else
        {
            if (!_death)
            {
                _condition = BlockorAttack.Attack;
                _death = true;
                Death();
            }
        }
    }

    /// <summary>テキストの更新の処理</summary>
    /// <param name="str"></param>
    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    /// <summary>ブロックをしている際のコルーチン</summary>
    /// <returns></returns>
    IEnumerator BlockTime()
    {
        //一秒後にそれぞれのブロックの処理どちらかを実行。
        yield return new WaitForSeconds(1f);
        if (_survive != Survive.Death)
        {
            if (_condition == BlockorAttack.LeftBlock)
            {
                ShowText(_condition.ToString());
                _guageAttack += 5;
            }
            else if (_condition == BlockorAttack.RightBlock)
            {
                ShowText(_condition.ToString());
                _guageAttack += 5;
            }
        }
        _blockTime = false;
    }

    /// <summary>アタックをしている際のコルーチン</summary>
    /// <returns></returns>
    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(2f);
        if (_survive != Survive.Death)
        {
            if (_condition == BlockorAttack.Attack)
            {
                var set = DataBase.BlockSkillData[DataBase._blockSkillSetNo[0]];
                ShowText(set.SkillName);
                _guageAttack += 1;
                _enemy.AddDebuffDamage(Attack, set.AttackValue, set.EnemyOffencePower, set.EnemyDiffencePower);
            }
        }
        _attackTime = false;
    }

    /// <summary>ブロックからアタックに切り替わるときの状態変化</summary>
    /// <param name="i">配列の要素数</param>
    /// <param name="state">状態変化</param>
    void CoolTime(BlockorAttack state = BlockorAttack.LeftBlock)
    {
        if (_condition != BlockorAttack.ChageAttack)
        {
            StartCoroutine(CoolTimeCoroutine());
        }
    }

    /// <summary>BlockからAttackに切り替わるまでのクールタイムを書いたコルーチン</summary>
    /// <returns></returns>
    IEnumerator CoolTimeCoroutine()
    {
        _condition = BlockorAttack.CoolTime;
        transform.position = _trans[0].position;
        ShowText("CoolTime");
        yield return new WaitForSeconds(1.5f);
        if (_survive != Survive.Death)
        {
            _condition = BlockorAttack.Attack;
        }
        ShowText("CoolTime終了");
        _coolTimebool = false;
    }

    /// <summary>カウンター待機にするための処理</summary>
    /// <param name="i"></param>
    /// <param name="counter"></param>
    void CoolCounter(int i = 1,BlockorAttack counter = BlockorAttack.CoolLeftCounter)
    {
        if (_condition == BlockorAttack.Attack)
        {
            _condition = counter;
            transform.position = _trans[i].position;
            ShowText("カウンター準備");
        }
    }

    /// <summary>カウンターが成功したときの処理</summary>
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

    /// <summary>カウンターが成功したときの処理から呼ばれるコルーチン</summary>
    /// <param name="tmp"></param>
    /// <returns></returns>
    IEnumerator TrueCounrerTime(BlockorAttack tmp)
    {
        //カウンターの処理//
        Debug.Log("カウンター！");
        _enumtext.text = "カウンター！";
        _enemy.AddDamage(Attack,2);
        _guageAttack += 10;
        //終わり//
        yield return new WaitForSeconds(2f);
        if (_survive != Survive.Death)
        {
            if (tmp == BlockorAttack.CoolLeftCounter)
            {
                _condition = BlockorAttack.LeftBlock;
            }
            else
            {
                _condition = BlockorAttack.RightBlock;
            }
        }
    }

    /// <summary>カウンター待機時間、カウンター失敗したときのコルーチン</summary>
    /// <returns></returns>
    IEnumerator CoolCounterTime()
    {
        yield return new WaitForSeconds(0.3f);
        if (_survive != Survive.Death)
        {
            if (_condition == BlockorAttack.CoolLeftCounter ||
                _condition == BlockorAttack.CoolRightCounter)
            {
                Debug.Log("カウンター失敗、防御態勢へ移行");
                if (_condition == BlockorAttack.CoolLeftCounter)
                {
                    _enumtext.text = "LeftBlock";
                    _condition = BlockorAttack.LeftBlock;
                }
                else
                {
                    _enumtext.text = "RightBlock";
                    _condition = BlockorAttack.RightBlock;
                }
            }
        }
        _counterTime = false;
    }

    /// <summary>死んだときの処理</summary>
    void Death()
    {
        _blockObj.transform.Rotate(90f, 0f, 0f);
        ShowText("俺は死んだぜ☆");
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
