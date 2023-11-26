using System;
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
    [Tooltip("ディフェンス職の状態"),SerializeField]
    BlockState _conditionState = BlockState.Attack;
    public BlockState Condition => _conditionState;
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
    [SerializeField] GetOutScripts _getOutPrefab;
    GetOutScripts _getOutObj;
    [SerializeField] Transform _getInPos;
    [SerializeField] Animator _blockAnim;
    [SerializeField] Canvas _statusCanvas;

    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<EnemyController>();
        SetStatus();
        ShowSlider();
        Debug.Log($"BlockerHP:{HP}");
        Debug.Log($"BlockerAttack:{Attack}");
        Debug.Log($"BlockerDiffence:{Diffence}");
    }

    void Update()
    {

        if (_survive == Survive.Survive)
        {
            if (FightManager.Instance.BattleState == BattleState.RPGBattle)
            {
                //Qボタンで左に移動して左のキャラへの攻撃を守る状態になる。
                if (Input.GetButton("BlockLeft") &&
                    (_conditionState == BlockState.Attack ||
                    _conditionState == BlockState.CoolLeftCounter ||
                    _conditionState == BlockState.LeftBlock))
                {
                    CoolCounter(1, BlockState.CoolLeftCounter);
                }
                //Eボタンで右に移動して状態が左のキャラへの攻撃を守る状態になる。
                else if (Input.GetButton("BlockRight") &&
                    (_conditionState == BlockState.Attack ||
                    _conditionState == BlockState.CoolRightCounter ||
                    _conditionState == BlockState.RightBlock))
                {
                    CoolCounter(2, BlockState.CoolRightCounter);
                }
                //ニュートラルでカウンター状態じゃない限り、真ん中に移動して状態がAttackになる。
                else if (_conditionState == BlockState.LeftBlock ||
                    _conditionState == BlockState.RightBlock)
                {
                    if (!_coolTimebool)
                    {
                        _coolTimebool = true;
                        CoolTime(BlockState.Attack);
                    }
                }

                //CoolLeftCounterかCoolRightCounter時に実行
                if (_conditionState == BlockState.CoolLeftCounter ||
                    _conditionState == BlockState.CoolRightCounter)
                {
                    if (!_counterTime)
                    {
                        _counterTime = true;
                        StartCoroutine(CoolCounterTime());
                    }
                }

                //LeftBlockかRightBlockの時に実行
                if (_conditionState == BlockState.LeftBlock ||
                    _conditionState == BlockState.RightBlock)
                {
                    if (!_blockTime)
                    {
                        _blockTime = true;
                        StartCoroutine(BlockTime());
                    }
                }

                //Attackの時に実行
                if (_conditionState == BlockState.Attack)
                {
                    //チャージが100以上になったら状態をチャージアタックに変える。
                    if (_guageAttack >= 100)
                    {
                        _conditionState = BlockState.ChageAttack;
                        ShowText("チャージアタック");
                    }
                    else if (!_attackTime)
                    {
                        _attackTime = true;
                        StartCoroutine(AttackTime());
                    }
                }
                //ChageAttackの時に実行
                if (_conditionState == BlockState.ChageAttack)
                {
                    StartCoroutine(AttackTime());
                    _guageAttack = 0;
                    _conditionState = BlockState.Attack;
                }

                TimeMethod();

                if (HP == 0)
                {
                    _survive = Survive.Death;
                }
            }
        }
        else
        {
            if (!_death)
            {
                _conditionState = BlockState.Attack;
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

    /// <summary>ActionModeになった際、離脱する処理</summary>
    public override void ActionMode()
    {
        _getOutObj = Instantiate(_getOutPrefab, transform.position, transform.rotation);
        var tmpVec = transform.position;
        _conditionState = BlockState.Attack;
        tmpVec.y = transform.position.y - 100;
        transform.position = tmpVec;
        _statusCanvas.enabled = false;
    }

    public override void RPGMode()
    {
        StartCoroutine(GetInBlock());
    }

    IEnumerator GetInBlock()
    {
        yield return _getOutObj.GetIn(_getInPos, _trans[0]);
        transform.position = _trans[0].position;
        Destroy(_getOutObj.gameObject);
        FightManager.Instance.StartRPG();
        _statusCanvas.enabled = true;
        ShowText("待たせたな！");
    }

    /// <summary>ブロックをしている際のコルーチン</summary>
    /// <returns></returns>
    IEnumerator BlockTime()
    {
        yield return new WaitForSeconds(1f);
        if (_survive != Survive.Death && FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            if (_conditionState == BlockState.LeftBlock)
            {
                ShowText(_conditionState.ToString());
                _guageAttack += 5;
            }
            else if (_conditionState == BlockState.RightBlock)
            {
                ShowText(_conditionState.ToString());
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
        if (_survive != Survive.Death && FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            if (_conditionState == BlockState.Attack)
            {
                var set = DataBase.BlockSkills[DataBase._blockSkillSetNo[0]];
                ShowText(set.SkillName);
                _guageAttack += 1;
                _enemy.AddDebuffDamage(Attack, set.AttackValue, set.OffencePower, set.DiffencePower);
            }
            else if(_conditionState == BlockState.ChageAttack)
            {
                //チャージアタックをした後、ゲージを０にして、Attack状態に戻る。
                var set = DataBase.BlockSkills[DataBase._blockSkillSetNo[1]];
                Debug.Log(set.SkillName);
                _enemy.AddDebuffDamage(Attack, set.AttackValue * 5, set.OffencePower * 5, set.DiffencePower * 5);
            }
        }
        _attackTime = false;
    }

    /// <summary>ブロックからアタックに切り替わるときの状態変化</summary>
    /// <param name="i">配列の要素数</param>
    /// <param name="state">状態変化</param>
    void CoolTime(BlockState state = BlockState.LeftBlock)
    {
        if (_conditionState != BlockState.ChageAttack && FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            StartCoroutine(CoolTimeCoroutine());
        }
    }

    /// <summary>BlockからAttackに切り替わるまでのクールタイムを書いたコルーチン</summary>
    /// <returns></returns>
    IEnumerator CoolTimeCoroutine()
    {
        _conditionState = BlockState.CoolTime;
        transform.position = _trans[0].position;
        ShowText("CoolTime");
        yield return new WaitForSeconds(1.5f);
        if (_survive != Survive.Death)
        {
            _conditionState = BlockState.Attack;
        }
        ShowText("CoolTime終了");
        _coolTimebool = false;
    }

    /// <summary>カウンター待機にするための処理</summary>
    /// <param name="i"></param>
    /// <param name="counter"></param>
    void CoolCounter(int i = 1,BlockState counter = BlockState.CoolLeftCounter)
    {
        if (_conditionState == BlockState.Attack)
        {
            _conditionState = counter;
            transform.position = _trans[i].position;
            ShowText("カウンター準備");
        }
    }

    /// <summary>カウンターが成功したときの処理</summary>
    public void TrueCounter()
    {
        if (_conditionState == BlockState.CoolLeftCounter || 
            _conditionState == BlockState.CoolRightCounter)
        {
            var tmpCondition = _conditionState;
            _conditionState = BlockState.Counter;
            StartCoroutine(TrueCounrerTime(tmpCondition));
        }
    }

    /// <summary>カウンターが成功したときの処理から呼ばれるコルーチン</summary>
    /// <param name="tmp"></param>
    /// <returns></returns>
    IEnumerator TrueCounrerTime(BlockState tmp)
    {
        //カウンターの処理//
        Debug.Log("カウンター！");
        _enumtext.text = "カウンター！";
        _enemy.AddDamage(Attack,2);
        _guageAttack += 10;
        //終わり//
        yield return new WaitForSeconds(2f);
        _blockTime = false;
        if (_survive != Survive.Death)
        {
            if (tmp == BlockState.CoolLeftCounter)
            {
                _conditionState = BlockState.LeftBlock;
            }
            else
            {
                _conditionState = BlockState.RightBlock;
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
            if (_conditionState == BlockState.CoolLeftCounter ||
                _conditionState == BlockState.CoolRightCounter)
            {
                Debug.Log("カウンター失敗、防御態勢へ移行");
                _blockTime = false;
                if (_conditionState == BlockState.CoolLeftCounter)
                {
                    _enumtext.text = "LeftBlock";
                    _conditionState = BlockState.LeftBlock;
                }
                else
                {
                    _enumtext.text = "RightBlock";
                    _conditionState = BlockState.RightBlock;
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
    public enum BlockState
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
