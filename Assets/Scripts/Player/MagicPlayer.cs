using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MagicPlayer : StatusClass
{
    [Tooltip("�ړ�����ꏊ")]
    [SerializeField]
    [Header("0,�O�A1,���̃|�W�V����")]
    Transform[] _trans;
    [Tooltip("���@�E�̈ʒu�̏��")]
    MagicPosition _magicpos;
    [Tooltip("���@�E�̖h�䖂�@")]
    BlockMagic _blockMagic;
    [Tooltip("���@�E�̍U�����@")]
    AttackMagic _attackMagic;
    [Tooltip("���@�����̃R���[�`���𓮂������߂�bool")]
    bool _magicTime;

    int _magicSkillCount = 0;
    [SerializeField] float _attackLange = 3;
    [SerializeField] Animator _animRobot;
    [SerializeField] Text _enumtext;
    [SerializeField] AttackPlayer _attackPlayer;
    [SerializeField] BlockPlayer _blockPlayer;
    [SerializeField] Transform _magicObj;
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

            if (HP == 0)
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

    public override void ActionMode()
    {
        ChangeCondition(0, MagicPosition.AttackMagic);
        for(var i = 0;i < DataBase._attackMagicbool.Length;i++)
        {
            _magicSkillCount += DataBase._attackMagicbool[i] ? 1 : 0;
        }
        ShowText("LeftShift��Attack�I");
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 2, _attackLange);
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

    /// <summary>RPG���[�h���̍U���T�|�[�g</summary>
    /// <returns></returns>
    IEnumerator MagicTime()
    {
        yield return new WaitForSeconds(5f);
        if (_survive == Survive.Survive && FightManager.Instance.BattleState == BattleState.RPGBattle)
        {
            if (_magicpos == MagicPosition.AttackMagic)
            {
                var set = DataBase.AttackMagics[DataBase._attackMagicSetNo[(int)_attackMagic]];
                ShowText($"{set.SkillName}�I");
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
                ShowText($"{set.SkillName}�I");
                _attackPlayer.AddBuff(set.OffencePower, set.DiffencePower, set.HealingHP);
                _blockPlayer.AddBuff(set.OffencePower, set.DiffencePower, set.HealingHP);
                AddBuff(set.OffencePower, set.DiffencePower, set.HealingHP);
            }
        }
        _magicTime = false;
    }

    /// <summary>�A�N�V�������[�h���̍U��</summary>
    void ActionAttack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * 2, _attackLange);
        _animRobot.SetTrigger("NormalAttack");
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "EnemyBullet")
            {
                _enemy.AddMagicDamage(Attack * (Mathf.Max(0.1f,_magicSkillCount / 8)));
                Destroy(col.gameObject);
            }
        }
    }

    public override void RPGMode()
    {
        ShowText("�������������I");
    }

    /// <summary>�e�L�X�g�\��</summary>
    /// <param name="str"></param>
    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    /// <summary>���S����</summary>
    void Death()
    {
        _magicObj.Rotate(90f, 0f, 0f);
        ShowText("���͎��񂾂���");
    }

    /// <summary>�����蔻��</summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet" && FightManager.Instance.BattleState == BattleState.ActionBattle)
        {
            if (FightManager.Instance.BattleState == BattleState.ActionBattle)
            {
                AddDamage(10);
            }
            Destroy(other.gameObject);
        }
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
