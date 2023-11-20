using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MagicPlayer : StatusClass
{
    [Tooltip("移動する場所")]
    [SerializeField]
    [Header("0,前、1,後ろのポジション")]
    Transform[] _trans;
    [Tooltip("魔法職の位置の状態")]
    MagicPosition _magicpos;
    [Tooltip("魔法職の防御魔法")]
    BlockMagic _blockMagic;
    [Tooltip("魔法職の攻撃魔法")]
    AttackMagic _attackMagic;
    [Tooltip("魔法発動のコルーチンを動かすためのbool")]
    bool _magicTime;
    [SerializeField]
    float _attackLange = 3;

    [SerializeField] Text _enumtext;
    [SerializeField] AttackPlayer _attackPlayer;
    [SerializeField] BlockPlayer _blockPlayer;
    [SerializeField] Transform _magicObj;
    [SerializeField] Animator _animRobot;
    [SerializeField] GameObject _ship;

    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<EnemyController>();
        SetStatus();
        ShowSlider();
        Debug.Log($"MagicHP:{HP}");
        Debug.Log($"MagicAttack:{Attack}");
        Debug.Log($"MagicDiffence:{Diffence}");
    }

    private void OnEnable()
    {
        FightManager.OnEnterAction += ActionMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (_death) { return; }

        if (_survive == Survive.Survive)
        {
            if (FightManager.Instance.BattleState == BattleState.RPGBattle)
            {
                if (Input.GetButtonDown("MagicForward"))
                {
                    ChangeCondition(0, MagicPosition.AttackMagic);
                }
                if (Input.GetButtonDown("MagicBack"))
                {
                    ChangeCondition(1, MagicPosition.BlockMagic);
                }
                if (Input.GetButtonDown("LeftMagic"))
                {
                    ChangeMagic(0);
                }
                if (Input.GetButtonDown("RightMagic"))
                {
                    ChangeMagic(1);
                }

                if (!_magicTime)
                {
                    _magicTime = true;
                    StartCoroutine(MagicTime());
                }
                TimeMethod();
            }

            if (FightManager.Instance.BattleState == BattleState.ActionBattle)
            {
                if(Input.GetButtonDown("MagicAttack"))
                {
                    ActionAttack();
                }
                    
            }

            if (_hp == 0)
            {
                _survive = Survive.Death;
            }
        }
        else
        {
            if (!_death)
            {
                _death = true;
                Death();
            }
        }
    }

    void ActionMode()
    {
        ChangeCondition(0, MagicPosition.AttackMagic);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + transform.forward * 2, _attackLange);
    }

    void ChangeCondition(int i, MagicPosition magic)
    {
        transform.position = _trans[i].position;
        _magicpos = magic;
        ShowText(_magicpos.ToString());
        if (_magicpos == MagicPosition.AttackMagic)
        {
            _attackMagic = (AttackMagic)_blockMagic;
            Debug.Log(_attackMagic);
        }
        else
        {
            _blockMagic = (BlockMagic)_attackMagic;
            Debug.Log(_blockMagic);
        }
    }

    void ChangeMagic(int i)
    {
        if (_magicpos == MagicPosition.AttackMagic)
        {
            _attackMagic = (AttackMagic)i;
            ShowText(_attackMagic.ToString());
        }
        else
        {
            _blockMagic = (BlockMagic)i;
            ShowText(_blockMagic.ToString());
        }
    }

    IEnumerator MagicTime()
    {
        yield return new WaitForSeconds(5f);
        if (_survive == Survive.Survive && FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            if (_magicpos == MagicPosition.AttackMagic)
            {
                var set = DataBase.AttackMagics[DataBase._attackMagicSetNo[(int)_attackMagic]];
                ShowText($"{set.SkillName}！");
                switch (set.ID)
                {
                    case 11:
                        var insShip = Instantiate(_ship, InsObjPoint);
                        yield return new WaitForSeconds(1f);
                        _enemy.AddMagicDamage(Attack, set.AttackValue);

                        break;
                    default:
                        _animRobot.SetTrigger("NormalAttack");
                        _enemy.AddMagicDamage(Attack, set.AttackValue);
                        break;

                }
            }
            else
            {
                var set = DataBase.BlockMagics[DataBase._blockMagicSetNo[(int)_blockMagic]];
                ShowText($"{set.SkillName}！");
                _attackPlayer.AddBuff(set.OffencePower, set.DiffencePower, set.HealingHP);
                _blockPlayer.AddBuff(set.OffencePower, set.DiffencePower, set.HealingHP);
                AddBuff(set.OffencePower, set.DiffencePower, set.HealingHP);
            }
        }
        _magicTime = false;
    }

    void ActionAttack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * 2, _attackLange, default);
        _animRobot.SetTrigger("NormalAttack");
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "EnemyBullet")
            {
                _enemy.AddDamage(Attack);
                Destroy(col.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            AddDamage(10);
            Destroy(other.gameObject);
        }
    }

    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    void Death()
    {
        _magicObj.Rotate(90f, 0f, 0f);
        ShowText("俺は死んだぜ☆");
    }

    enum MagicPosition
    {
        AttackMagic,
        BlockMagic,

    }

    enum BlockMagic
    {
        LeftBlockMagic,
        RightBlockMagic,
    }

    enum AttackMagic
    {
        LeftAttackMagic,
        RightAttackMagic,
    }

}
