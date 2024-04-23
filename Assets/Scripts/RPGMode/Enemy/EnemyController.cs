using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyController : StatusClass
{
    private BlockPlayerController _blockPlayer;
    private MagicPlayer _magicPlayer;
    private AttackPlayer _attackPlayer;
    [SerializeField] Text _enemyText;
    [SerializeField] int _getSkillPoint = 50;
    bool _enemyAttackbool;
    [SerializeField] Animator _anim;
    bool _fight;
    [Header("アクションモード設定")]
    [SerializeField] bool _aliveAction;
    [SerializeField] TimelineBullet _timelineBullet;
    [SerializeField]
    bool _debugActionModeBool;
    float _actionTime;

    // Start is called before the first frame update
    void Start()
    {
        _blockPlayer = GameObject.FindGameObjectWithTag("BlockPlayer")?.GetComponent<BlockPlayerController>();
        _magicPlayer = GameObject.FindGameObjectWithTag("MagicPlayer")?.GetComponent<MagicPlayer>();
        _attackPlayer = GameObject.FindGameObjectWithTag("AttackPlayer")?.GetComponent<AttackPlayer>();

        SetStatus();
        ShowSlider();
        Debug.Log($"EnemyHP:{HP}");
        Debug.Log($"EnemyAttack:{Attack}");
        Debug.Log($"EnemyDiffence:{Diffence}");
    }

    // Update is called once per frame
    void Update()
    {
        if (_debugActionModeBool) 
        {
            AddDamage(HP * 0.3f);
            _debugActionModeBool = false;
        }
        if (_survive == Survive.Survive && 
            FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            if (!_enemyAttackbool)
            {
                _enemyAttackbool = true;
                StartCoroutine(EnemyAttackCoolTime());
            }

            TimeMethod();

            if (_magicPlayer.HP <= 0 && _attackPlayer.HP <= 0 && !_fight)
            {
                _fight = true;
                FightManager.Instance.Lose();
            }
            else if (HP <= 0 && !_fight)
            {
                _fight = true;
                _anim.SetBool("Death", true);
                _survive = Survive.Death;
                _enemyText.text = "ぬわああああああああああああ";
                FightManager.Instance.Win(_getSkillPoint);
            }

            if(HP <= DefaulrHP * 0.75 && _aliveAction)
            {
                FightManager.Instance.ActionEnter();
                _actionTime = _timelineBullet.ActionStart();
            }
        }
        else if(FightManager.Instance.BattleState == BattleState.ActionBattle)
        {
            _actionTime -= Time.deltaTime;
            if(HP <= DefaulrHP * 0.25 || _actionTime < 0)
            {
                FightManager.Instance.RPGEnter();
                _timelineBullet.ActionEnd();
                _aliveAction = false;
            }
        }
    }

    IEnumerator EnemyAttackCoolTime()
    {
        yield return new WaitForSeconds(5f);
        if (_survive != Survive.Death && FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            var ram = Random.Range(0, 100);
            _anim.SetBool("Attack", true);
            if (ram < 50 && _magicPlayer.HP > 0 || _attackPlayer.HP <= 0)
            {
                _enemyText.text = "Magicに攻撃";
                yield return EnemyAttack(true);
            }
            else
            {
                _enemyText.text = "Attackerに攻撃";
                yield return EnemyAttack(false);
            }
        }
        _anim.SetBool("Attack", false);
        _enemyAttackbool = false;
    }

    /// <summary>敵の攻撃時の処理</summary>
    /// <param name="targetMagic"></param>
    /// <returns></returns>
    IEnumerator EnemyAttack(bool targetMagic)
    {
        yield return new WaitForSeconds(1.1f);

        if (_survive != Survive.Death && FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            AudioManager.Instance.SEPlay(SE.EnemyShordAttack);
            if (targetMagic)
            {
                TargetAttack(TargetGuard.Magician,_magicPlayer);
            }
            else
            {
                TargetAttack(TargetGuard.Attacker,_attackPlayer);
            }
        }
    }

    /// <summary>誰に攻撃するか</summary>
    /// <param name="targetGuard"></param>
    void TargetAttack(TargetGuard target,StatusClass player)
    {
        if(_blockPlayer._targetGuard == target)
        {
            GuardOrCounter();
        }
        else
        {
            _enemyText.text = $"{target}にダメージ。";
            player.AddDamage(Attack,2);
        }
    }

    void GuardOrCounter()
    {
        if(_blockPlayer.CurrentState == _blockPlayer.CoolCounterState)
        {
            Counter();
        }
        else if(_blockPlayer.CurrentState == _blockPlayer.GuardState)
        {
            Guard();
        }
        else
        {
            Debug.LogWarning("ここに入るのはちょっとおかしいよ。");
        }
    }
    
    public override void ActionMode()
    {
        _anim.Play("LoopAttack");
        _enemyText.text = "アクションモード！";
    }

    public override void RPGMode()
    {
        _anim.Play("Stand");
        _enemyText.text = "RPGモード！";
    }

    void Guard()
    {
        _enemyText.text = "ガードされた！";
        _blockPlayer.AddDamage(Attack);
    }

    void Counter()
    {
        _enemyText.text = "カウンターされた！";
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
