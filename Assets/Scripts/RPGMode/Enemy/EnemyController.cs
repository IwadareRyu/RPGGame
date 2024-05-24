using RPGBattle;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class EnemyController : StatusClass
{
    private BlockPlayerController _blockPlayer;
    private MagicPlayer _magicPlayer;
    private AttackPlayer _attackPlayer;
    [SerializeField] int _getSkillPoint = 50;
    float _currentAttackCoolTime = 0f;
    int _currentAttackCount = 0;
    public int CurrentAttackCount => _currentAttackCount;
    bool _enemyAttackbool;
    public Animator _anim;
    [SerializeField] NormalAttack[] _normalAttacks;
    int _currentNormalAttackIndex = 0;
    [SerializeField] TwiceAttack _twiceAttack = new();
    EnemyInterface _currentAttackScripts;
    TargetGuard _currentTargetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _blockPlayer = GameObject.FindGameObjectWithTag("BlockPlayer")?.GetComponent<BlockPlayerController>();
        _magicPlayer = GameObject.FindGameObjectWithTag("MagicPlayer")?.GetComponent<MagicPlayer>();
        _attackPlayer = GameObject.FindGameObjectWithTag("AttackPlayer")?.GetComponent<AttackPlayer>();
        _currentAttackScripts = _normalAttacks[_currentNormalAttackIndex];
        SetStatus();
        HPViewAccess();
        ChantingViewAccess(_currentAttackCoolTime, 1);
        Debug.Log($"EnemyHP:{HP}\nEnemyAttack:{Attack}\nEnemyDiffence:{Diffence}");
    }

    // Update is called once per frame
    void Update()
    {
        TimeMethod();
        if (RPGBattleManager.Instance.BattleState != BattleState.RPGBattle || _enemyAttackbool) { return; }


        if (_survive == Survive.Survive)
        {
            if (_currentAttackScripts.TimeBoolean(this, ref _currentAttackCoolTime))
            {
                _currentAttackCoolTime = 0f;
                _enemyAttackbool = true;
                StartCoroutine(EnemyAttackCoolTime());
            }

            if (HP <= 0)
            {
                _anim.SetBool("Death", true);
                _survive = Survive.Death;
                ConditionTextViewAccess("ぬわああああああああああああ");
                RPGBattleManager.Instance.BattleEnd();
                FightManager.Instance.Win(_getSkillPoint);
            }
        }
    }

    IEnumerator EnemyAttackCoolTime()
    {
        yield return _currentAttackScripts.AttackTime(this);
        _enemyAttackbool = false;
        _currentAttackCount = 0;
        ChantingViewAccess(_currentAttackCoolTime, 1);
    }

    public void TargetChange()
    {
        var ram = UnityEngine.Random.Range(0, 100);
        if (ram < 50)
        {
            ConditionTextViewAccess("Magicに攻撃");
            _currentTargetPlayer = TargetGuard.Magician;
        }
        else
        {
            ConditionTextViewAccess("Attackerに攻撃");
            _currentTargetPlayer = TargetGuard.Attacker;
        }
    }

    /// <summary>敵の攻撃時の処理</summary>
    /// <param name="targetMagic"></param>
    /// <returns></returns>
    public void EnemyAttack()
    {
        if (_survive != Survive.Death && RPGBattleManager.Instance.BattleState == BattleState.RPGBattle)
        {
            AudioManager.Instance.SEPlay(SE.EnemyShordAttack);
            if(_currentTargetPlayer == TargetGuard.Magician) { TargetAttack(_magicPlayer); }
            else { TargetAttack(_attackPlayer); }
        }
        // 攻撃回数をカウント
        AddAttackCount();
        //指定の攻撃回数に達してれば次の攻撃に移る、してなければ攻撃のターゲットを変える。
        if (!_currentAttackScripts.EndAttackBoolean(this)) { TargetChange(); }
        else { ChangeNormalAttack(); }
    }

    /// <summary>誰に攻撃するか</summary>
    /// <param name="targetGuard"></param>
    void TargetAttack(StatusClass player)
    {
        if (_blockPlayer._targetGuard == _currentTargetPlayer)
        {
            GuardOrCounter();
        }
        else
        {
            ConditionTextViewAccess($"{_currentTargetPlayer}にダメージ。");
            player.AddDamage(Attack, 2);
            player.ChantingTimeReset();
        }
    }

    /// <summary>Blockerがカウンター状態かGuard状態</summary>
    void GuardOrCounter()
    {
        if (_blockPlayer.CurrentState == _blockPlayer.CoolCounterState)
        {
            Counter();
        }
        else if (_blockPlayer.CurrentState == _blockPlayer.GuardState)
        {
            Guard();
        }
        else
        {
            Debug.LogWarning("ここに入るのはちょっとおかしいよ。");
        }
    }

    public void AddAttackCount()
    {
        _currentAttackCount++;
    }

    public void ChangeNormalAttack()
    {
        _currentNormalAttackIndex = (_currentNormalAttackIndex + 1) % _normalAttacks.Length;
        Debug.Log(_currentNormalAttackIndex);
        _currentAttackScripts = _normalAttacks[_currentNormalAttackIndex];
    }

    public override void RPGMode()
    {
        _anim.Play("Stand");
        ConditionTextViewAccess("RPGモード！");
    }

    void Guard()
    {
        ConditionTextViewAccess("ガードされた！");
        _blockPlayer.AddDamage(Attack);
        _blockPlayer.EndGuard();
    }

    void Counter()
    {
        ConditionTextViewAccess("カウンターされた！");
        _blockPlayer.TrueCounter();
        _blockPlayer.AddDamage(Attack);
    }

    enum EnemyEnum
    {
        CoolTime,
        RightAttack,
        LeftAttack,
        AllAttack,
    }
}
