using RPGBattle;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : StatusClass
{
    private BlockPlayerController _blockPlayer;
    private MagicPlayer _magicPlayer;
    private AttackPlayer _attackPlayer;
    [SerializeField] int _getSkillPoint = 50;
    [SerializeField] float _attackCoolTime = 5f;
    float _currentAttackCoolTime = 0f;
    bool _enemyAttackbool;
    [SerializeField] Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _blockPlayer = GameObject.FindGameObjectWithTag("BlockPlayer")?.GetComponent<BlockPlayerController>();
        _magicPlayer = GameObject.FindGameObjectWithTag("MagicPlayer")?.GetComponent<MagicPlayer>();
        _attackPlayer = GameObject.FindGameObjectWithTag("AttackPlayer")?.GetComponent<AttackPlayer>();
        SetStatus();
        HPViewAccess();
        ChantingViewAccess(_currentAttackCoolTime,_attackCoolTime);
        Debug.Log($"EnemyHP:{HP}\nEnemyAttack:{Attack}\nEnemyDiffence:{Diffence}");
    }

    // Update is called once per frame
    void Update()
    {
        if (RPGBattleManager.Instance.BattleState != BattleState.RPGBattle) { return; }

        if (_survive == Survive.Survive)
        {
            if (!_enemyAttackbool)
            {
                _enemyAttackbool = true;
                StartCoroutine(EnemyAttackCoolTime());
            }

            TimeMethod();

            if (HP <= 0)
            {
                _anim.SetBool("Death", true);
                _survive = Survive.Death;
                ConditionTextViewAccess("ぬわああああああああああああ")   ;
                RPGBattleManager.Instance.BattleEnd();
                FightManager.Instance.Win(_getSkillPoint);
            }
        }
    }

    IEnumerator EnemyAttackCoolTime()
    {
        for ( ; _currentAttackCoolTime < _attackCoolTime; _currentAttackCoolTime += Time.deltaTime)
        {
            ChantingViewAccess(_currentAttackCoolTime,_attackCoolTime);
            yield return new WaitForEndOfFrame();
        }
        if (_survive != Survive.Death && RPGBattleManager.Instance.BattleState == BattleState.RPGBattle)
        {
            var ram = Random.Range(0, 100);
            _anim.SetBool("Attack", true);
            if (ram < 50 && _magicPlayer.HP > 0 || _attackPlayer.HP <= 0)
            {
                ConditionTextViewAccess("Magicに攻撃");
                yield return EnemyAttack(true);
            }
            else
            {
                ConditionTextViewAccess("Attackerに攻撃");
                yield return EnemyAttack(false);
            }
        }
        _anim.SetBool("Attack", false);
        _currentAttackCoolTime = 0;
        ChantingViewAccess(_currentAttackCoolTime, _attackCoolTime);
        _enemyAttackbool = false;
    }

    /// <summary>敵の攻撃時の処理</summary>
    /// <param name="targetMagic"></param>
    /// <returns></returns>
    IEnumerator EnemyAttack(bool targetMagic)
    {
        yield return new WaitForSeconds(1.1f);

        if (_survive != Survive.Death && RPGBattleManager.Instance.BattleState == BattleState.RPGBattle)
        {
            AudioManager.Instance.SEPlay(SE.EnemyShordAttack);
            if (targetMagic)
            {
                TargetAttack(TargetGuard.Magician, _magicPlayer);
            }
            else
            {
                TargetAttack(TargetGuard.Attacker, _attackPlayer);
            }
        }
    }

    /// <summary>誰に攻撃するか</summary>
    /// <param name="targetGuard"></param>
    void TargetAttack(TargetGuard target, StatusClass player)
    {
        if (_blockPlayer._targetGuard == target)
        {
            GuardOrCounter();
        }
        else
        {
            ConditionTextViewAccess($"{target}にダメージ。");
            player.AddDamage(Attack, 2);
            player.ChantingTimeReset();
        }
    }

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

    public override void RPGMode()
    {
        _anim.Play("Stand");
        ConditionTextViewAccess("RPGモード！");
    }

    void Guard()
    {
        ConditionTextViewAccess("ガードされた！");
        _blockPlayer.AddDamage(Attack);
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
