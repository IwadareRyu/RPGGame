using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Text _enumtext;
    [SerializeField] AttackPlayer _attackPlayer;
    [SerializeField] BlockPlayer _blockPlayer;
    [SerializeField] GameObject _magicObj;

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
        if (_survive == Survive.Survive)
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
            if(_hp == 0)
            {
                _survive = Survive.Death;
            }
            TimeMethod();
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

    void ChangeCondition(int i,MagicPosition magic)
    {
        transform.position = _trans[i].position;
        _magicpos = magic;
        ShowText(_magicpos.ToString());
        if(_magicpos == MagicPosition.AttackMagic)
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
        if(_magicpos == MagicPosition.AttackMagic)
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
        if (_survive == Survive.Survive)
        {
            if (_magicpos == MagicPosition.AttackMagic)
            {
                var set = DataBase.AttackMagicData[DataBase._attackMagicSetNo[(int)_attackMagic]];
                ShowText($"{set.SkillName}�I");
                _enemy.AddMagicDamage(Attack);
            }
            else
            {
                var set = DataBase.BlockMagicData[DataBase._blockMagicSetNo[(int)_blockMagic]];
                ShowText($"{set.SkillName}�I");
                _attackPlayer.AddBuff(set.PlusAttackPower, set.PlusDiffencePower, set.HealingHP);
                _blockPlayer.AddBuff(set.PlusAttackPower, set.PlusDiffencePower, set.HealingHP);
                AddBuff(set.PlusAttackPower, set.PlusDiffencePower, set.HealingHP);
            }
        }
        _magicTime = false;
    }

    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    void Death()
    {
        _magicObj.transform.Rotate(90f, 0f, 0f);
        ShowText("���͎��񂾂���");
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
