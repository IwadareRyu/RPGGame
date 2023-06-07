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
        if(Input.GetButtonDown("MagicForward"))
        {
            ChangeCondition(0,MagicPosition.AttackMagic);
        }
        if(Input.GetButtonDown("MagicBack"))
        {
            ChangeCondition(1,MagicPosition.BlockMagic);
        }
        if(Input.GetButtonDown("LeftMagic"))
        {
            ChangeMagic(0);
        }
        if(Input.GetButtonDown("RightMagic"))
        {
            ChangeMagic(1);
        }

        if(!_magicTime)
        {
            _magicTime = true;
            StartCoroutine(MagicTime());
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
        if(_magicpos == MagicPosition.AttackMagic)
        {
            ShowText($"{_attackMagic}�I");
            _enemy.AddMagicDamage(Attack);
        }
        else
        {
            ShowText($"{_blockMagic}�I");
        }
        _magicTime = false;
    }

    void ShowText(string str)
    {
        _enumtext.text = str;
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
