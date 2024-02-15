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
    bool _enemyAttackbool;
    [SerializeField] Animator _anim;
    bool _fight;
    [Header("�A�N�V�������[�h�ݒ�")]
    [SerializeField] bool _aliveAction;
    [SerializeField] TimelineBullet _timelineBullet;
    [SerializeField]
    bool _debugActionModeBool;
    float _actionTime;

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
                _enemytext.text = "�ʂ킠����������������������";
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
                _enemytext.text = "Magic�ɍU��";
                yield return EnemyAttack(true);
            }
            else
            {
                _enemytext.text = "Attacker�ɍU��";
                yield return EnemyAttack(false);
            }
        }
        _anim.SetBool("Attack", false);
        _enemyAttackbool = false;
    }

    IEnumerator EnemyAttack(bool targetMagic)
    {
        yield return new WaitForSeconds(1.1f);

        if (_survive != Survive.Death && FightManager.Instance.BattleState == BattleState.RPGBattle)
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
                    _enemytext.text = "Magic�Ƀ_���[�W�I";
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
                    _enemytext.text = "Attacker�Ƀ_���[�W�I";
                    _attackPlayer.AddDamage(Attack);
                }
            }
        }
    }

    public override void ActionMode()
    {
        _anim.Play("LoopAttack");
        _enemytext.text = "�A�N�V�������[�h�I";
    }

    public override void RPGMode()
    {
        _anim.Play("Stand");
        _enemytext.text = "RPG���[�h�I";
    }

    void Guard()
    {
        _enemytext.text = "�K�[�h���ꂽ�I";
        _blockPlayer.AddDamage(Attack);
    }

    void Counter()
    {
        _enemytext.text = "�J�E���^�[���ꂽ�I";
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