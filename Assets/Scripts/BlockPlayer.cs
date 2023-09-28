using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPlayer : StatusClass
{
    [Tooltip("BlockPlayer�̈ړ��ꏊ")]
    [SerializeField]
    Transform[] _trans;
    [Tooltip("�Q�[�W�A�^�b�N�̒l")]
    [SerializeField]
    float _guageAttack = 0;
    [Tooltip("�f�B�t�F���X�E�̏��"),SerializeField]
    BlockState _condition = BlockState.Attack;
    public BlockState Condition => _condition;
    [Tooltip("�ړ��̍ێ~�܂��")]
    float _stopdis = 0.1f;
    [Tooltip("�ړ��X�s�[�h")]
    float _speed = 6f;
    [Tooltip("Block�̍ہA�R���[�`����K�؂ɓ��������߂�bool")]
    bool _blockTime;
    [Tooltip("Attack�̍ہA�R���[�`����K�؂ɓ��������߂�bool")]
    bool _attackTime;
    [Tooltip("Counter�̍ہA�R���[�`����K�؂ɓ��������߂�bool")]
    bool _counterTime;
    [Tooltip("CoolTime�̍ہA�R���[�`����K�؂ɓ��������߂�bool")]
    bool _coolTimebool;
    [Tooltip("��Ԃ̃e�L�X�g")]
    [SerializeField] Text _enumtext;
    [SerializeField] GameObject _blockObj;



    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<EnemyController>();
        SetStatus();
        ShowSlider();
        Debug.Log($"BlockerHP:{HP}");
        Debug.Log($"BlockerAttack:{Attack}");
        Debug.Log($"BlockerDiffence:{Diffence}");
    }

    // Update is called once per frame
    void Update()
    {

        if (_survive == Survive.Survive)
        {
            //Q�{�^���ō��Ɉړ����č��̃L�����ւ̍U��������ԂɂȂ�B
            if (Input.GetButton("BlockLeft") &&
                (_condition == BlockState.Attack || 
                _condition == BlockState.CoolLeftCounter || 
                _condition == BlockState.LeftBlock))
            {
                CoolCounter(1, BlockState.CoolLeftCounter);
            }
            //E�{�^���ŉE�Ɉړ����ď�Ԃ����̃L�����ւ̍U��������ԂɂȂ�B
            else if (Input.GetButton("BlockRight") &&
                (_condition == BlockState.Attack ||
                _condition == BlockState.CoolRightCounter ||
                _condition == BlockState.RightBlock))
            {
                CoolCounter(2, BlockState.CoolRightCounter);
            }
            //�j���[�g�����ŃJ�E���^�[��Ԃ���Ȃ�����A�^�񒆂Ɉړ����ď�Ԃ�Attack�ɂȂ�B
            else if (_condition == BlockState.LeftBlock ||
                _condition == BlockState.RightBlock)
            {
                if (!_coolTimebool)
                {
                    _coolTimebool = true;
                    CoolTime(BlockState.Attack);
                }
            }

            //CoolLeftCounter��CoolRightCounter���Ɏ��s
            if (_condition == BlockState.CoolLeftCounter ||
                _condition == BlockState.CoolRightCounter)
            {
                if (!_counterTime)
                {
                    _counterTime = true;
                    StartCoroutine(CoolCounterTime());
                }
            }

            //LeftBlock��RightBlock�̎��Ɏ��s
            if (_condition == BlockState.LeftBlock ||
                _condition == BlockState.RightBlock)
            {
                if (!_blockTime)
                {
                    _blockTime = true;
                    StartCoroutine(BlockTime());
                }
            }

            //Attack�̎��Ɏ��s
            if (_condition == BlockState.Attack)
            {
                //�`���[�W��100�ȏ�ɂȂ������Ԃ��`���[�W�A�^�b�N�ɕς���B
                if (_guageAttack >= 100)
                {
                    _condition = BlockState.ChageAttack;
                    ShowText("�`���[�W�A�^�b�N");
                }
                else if (!_attackTime)
                {
                    _attackTime = true;
                    StartCoroutine(AttackTime());
                }
            }
            //ChageAttack�̎��Ɏ��s
            if (_condition == BlockState.ChageAttack)
            {
                StartCoroutine(AttackTime());
                _guageAttack = 0;
                _condition = BlockState.Attack;
            }

            TimeMethod();

            if(_hp == 0)
            {
                _survive = Survive.Death;
            }
        }
        else
        {
            if (!_death)
            {
                _condition = BlockState.Attack;
                _death = true;
                Death();
            }
        }
    }

    /// <summary>�e�L�X�g�̍X�V�̏���</summary>
    /// <param name="str"></param>
    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    /// <summary>�u���b�N�����Ă���ۂ̃R���[�`��</summary>
    /// <returns></returns>
    IEnumerator BlockTime()
    {
        yield return new WaitForSeconds(1f);
        if (_survive != Survive.Death)
        {
            if (_condition == BlockState.LeftBlock)
            {
                ShowText(_condition.ToString());
                _guageAttack += 5;
            }
            else if (_condition == BlockState.RightBlock)
            {
                ShowText(_condition.ToString());
                _guageAttack += 5;
            }
        }
        _blockTime = false;
    }

    /// <summary>�A�^�b�N�����Ă���ۂ̃R���[�`��</summary>
    /// <returns></returns>
    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(2f);
        if (_survive != Survive.Death)
        {
            if (_condition == BlockState.Attack)
            {
                var set = DataBase.BlockSkills[DataBase._blockSkillSetNo[0]];
                ShowText(set.SkillName);
                _guageAttack += 1;
                _enemy.AddDebuffDamage(Attack, set.AttackValue, set.OffencePower, set.DiffencePower);
            }
            else if(_condition == BlockState.ChageAttack)
            {
                //�`���[�W�A�^�b�N��������A�Q�[�W���O�ɂ��āAAttack��Ԃɖ߂�B
                var set = DataBase.BlockSkills[DataBase._blockSkillSetNo[1]];
                Debug.Log(set.SkillName);
                _enemy.AddDebuffDamage(Attack, set.AttackValue * 5, set.OffencePower * 5, set.DiffencePower * 5);
            }
        }
        _attackTime = false;
    }

    /// <summary>�u���b�N����A�^�b�N�ɐ؂�ւ��Ƃ��̏�ԕω�</summary>
    /// <param name="i">�z��̗v�f��</param>
    /// <param name="state">��ԕω�</param>
    void CoolTime(BlockState state = BlockState.LeftBlock)
    {
        if (_condition != BlockState.ChageAttack)
        {
            StartCoroutine(CoolTimeCoroutine());
        }
    }

    /// <summary>Block����Attack�ɐ؂�ւ��܂ł̃N�[���^�C�����������R���[�`��</summary>
    /// <returns></returns>
    IEnumerator CoolTimeCoroutine()
    {
        _condition = BlockState.CoolTime;
        transform.position = _trans[0].position;
        ShowText("CoolTime");
        yield return new WaitForSeconds(1.5f);
        if (_survive != Survive.Death)
        {
            _condition = BlockState.Attack;
        }
        ShowText("CoolTime�I��");
        _coolTimebool = false;
    }

    /// <summary>�J�E���^�[�ҋ@�ɂ��邽�߂̏���</summary>
    /// <param name="i"></param>
    /// <param name="counter"></param>
    void CoolCounter(int i = 1,BlockState counter = BlockState.CoolLeftCounter)
    {
        if (_condition == BlockState.Attack)
        {
            _condition = counter;
            transform.position = _trans[i].position;
            ShowText("�J�E���^�[����");
        }
    }

    /// <summary>�J�E���^�[�����������Ƃ��̏���</summary>
    public void TrueCounter()
    {
        if (_condition == BlockState.CoolLeftCounter || 
            _condition == BlockState.CoolRightCounter)
        {
            var tmpCondition = _condition;
            _condition = BlockState.Counter;
            StartCoroutine(TrueCounrerTime(tmpCondition));
        }
    }

    /// <summary>�J�E���^�[�����������Ƃ��̏�������Ă΂��R���[�`��</summary>
    /// <param name="tmp"></param>
    /// <returns></returns>
    IEnumerator TrueCounrerTime(BlockState tmp)
    {
        //�J�E���^�[�̏���//
        Debug.Log("�J�E���^�[�I");
        _enumtext.text = "�J�E���^�[�I";
        _enemy.AddDamage(Attack,2);
        _guageAttack += 10;
        //�I���//
        yield return new WaitForSeconds(2f);
        _blockTime = false;
        if (_survive != Survive.Death)
        {
            if (tmp == BlockState.CoolLeftCounter)
            {
                _condition = BlockState.LeftBlock;
            }
            else
            {
                _condition = BlockState.RightBlock;
            }
        }
    }

    /// <summary>�J�E���^�[�ҋ@���ԁA�J�E���^�[���s�����Ƃ��̃R���[�`��</summary>
    /// <returns></returns>
    IEnumerator CoolCounterTime()
    {
        yield return new WaitForSeconds(0.3f);
        if (_survive != Survive.Death)
        {
            if (_condition == BlockState.CoolLeftCounter ||
                _condition == BlockState.CoolRightCounter)
            {
                Debug.Log("�J�E���^�[���s�A�h��Ԑ��ֈڍs");
                _blockTime = false;
                if (_condition == BlockState.CoolLeftCounter)
                {
                    _enumtext.text = "LeftBlock";
                    _condition = BlockState.LeftBlock;
                }
                else
                {
                    _enumtext.text = "RightBlock";
                    _condition = BlockState.RightBlock;
                }
            }
        }
        _counterTime = false;
    }

    /// <summary>���񂾂Ƃ��̏���</summary>
    void Death()
    {
        _blockObj.transform.Rotate(90f, 0f, 0f);
        ShowText("���͎��񂾂���");
    }

    /// <summary>BlockPlayer�̏��</summary>
    public enum BlockState
    {
        [Tooltip("�ړ����Ă���Ԃ�CoolTime")]
        CoolTime,
        [Tooltip("�������̃K�[�h")]
        LeftBlock,
        [Tooltip("�������̃J�E���^�[�ҋ@")]
        CoolLeftCounter,
        [Tooltip("�E�����̃K�[�h")]
        RightBlock,
        [Tooltip("�E�����̃J�E���^�[�ҋ@")]
        CoolRightCounter,
        [Tooltip("�J�E���^�[")]
        Counter,
        [Tooltip("�U��")]
        Attack,
        [Tooltip("�`���[�W�U��")]
        ChageAttack,
    }
}
