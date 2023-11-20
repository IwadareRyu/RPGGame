using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyController : StatusClass
{
    private BlockPlayer _blockPlayer;
    private MagicPlayer _magicPlayer;
    private AttackPlayer _attackPlayer;
    [SerializeField] Text _enemytext;
    [SerializeField] int _getSkillPoint = 50;
    [SerializeField] float _actionTime = 60f;
    bool _enemyAttackbool;
    [SerializeField] Animator _anim;
    [SerializeField] bool _boss;
    bool _fight;

    // Start is called before the first frame update
    void Start()
    {
        _blockPlayer = GameObject.FindGameObjectWithTag("BlockPlayer")?.GetComponent<BlockPlayer>();
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
            else if (_hp <= 0 && !_fight)
            {
                _fight = true;
                _anim.SetBool("Death", true);
                _survive = Survive.Death;
                _enemytext.text = "ぬわああああああああああああ";
                FightManager.Instance.Win(_getSkillPoint);
            }

            if(_hp <= DefaulrHP * 0.75 && _boss)
            {
                FightManager.Instance.ActionEnter();
            }
        }
        else if(FightManager.Instance.BattleState == BattleState.ActionBattle)
        {
            _actionTime -= Time.deltaTime;
            if(_hp <= DefaulrHP * 0.25 || _actionTime < 0)
            {
                FightManager.Instance.RPGEnter();
            }
        }
    }

    IEnumerator EnemyAttackCoolTime()
    {
        yield return new WaitForSeconds(5f);
        if (_survive != Survive.Death)
        {
            var ram = Random.Range(0, 100);
            _anim.SetBool("Attack", true);
            if (ram < 50 && _magicPlayer.HP > 0 || _attackPlayer.HP <= 0)
            {
                _enemytext.text = "Magicに攻撃";
                StartCoroutine(EnemyAttack(true));
            }
            else
            {
                _enemytext.text = "Attackerに攻撃";
                StartCoroutine(EnemyAttack(false));
            }
        }
    }

    IEnumerator EnemyAttack(bool targetMagic)
    {
        yield return new WaitForSeconds(1.1f);

        if (_survive != Survive.Death)
        {
            if (targetMagic)
            {
                if (_blockPlayer.Condition == BlockPlayer.BlockState.LeftBlock)
                {
                    Guard();
                }
                else if (_blockPlayer.Condition == BlockPlayer.BlockState.CoolLeftCounter)
                {
                    Counter();
                }
                else
                {
                    _enemytext.text = "Magicにダメージ！";
                    _magicPlayer.AddDamage(Attack);
                }
            }
            else
            {
                if (_blockPlayer.Condition == BlockPlayer.BlockState.RightBlock)
                {
                    Guard();
                }
                else if (_blockPlayer.Condition == BlockPlayer.BlockState.CoolRightCounter)
                {
                    Counter();
                }
                else
                {
                    _enemytext.text = "Attackerにダメージ！";
                    _attackPlayer.AddDamage(Attack);
                }
            }
            _anim.SetBool("Attack", false);
        }
        _enemyAttackbool = false;

    }

    void Guard()
    {
        _enemytext.text = "ガードされた！";
        _blockPlayer.AddDamage(Attack);
    }

    void Counter()
    {
        _enemytext.text = "カウンターされた！";
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
