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
    [Tooltip("�f�B�t�F���X�E�̏��")]
    BlockorAttack _condition = BlockorAttack.Attack;
    public BlockorAttack Condition => _condition;
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
            //Q�{�^���ō��Ɉړ����ď�Ԃ�CoolLeftCounter�ɂȂ�B
            if (Input.GetButton("BlockLeft") &&
                _condition != BlockorAttack.CoolTime &&
                _condition != BlockorAttack.RightBlock &&
                _condition != BlockorAttack.CoolRightCounter &&
                _condition != BlockorAttack.Counter)
            {
                CoolCounter(1, BlockorAttack.CoolLeftCounter);
            }
            //E�{�^���ŉE�Ɉړ����ď�Ԃ�CoolRightCounter�ɂȂ�B
            else if (Input.GetButton("BlockRight") &&
                _condition != BlockorAttack.CoolTime &&
                _condition != BlockorAttack.LeftBlock &&
                _condition != BlockorAttack.CoolLeftCounter &&
                _condition != BlockorAttack.Counter)
            {
                CoolCounter(2, BlockorAttack.CoolRightCounter);
            }
            //�j���[�g�����ŃJ�E���^�[��Ԃ���Ȃ�����A�^�񒆂Ɉړ����ď�Ԃ�Attack�ɂȂ�B
            else if (_condition != BlockorAttack.CoolLeftCounter &&
                _condition != BlockorAttack.CoolRightCounter &&
                _condition != BlockorAttack.Counter &&
                _condition != BlockorAttack.Attack)
            {
                if (!_coolTimebool)
                {
                    _coolTimebool = true;
                    CoolTime(BlockorAttack.Attack);
                }
            }

            //CoolLeftCounter��CoolRightCounter���Ɏ��s
            if (_condition == BlockorAttack.CoolLeftCounter ||
                _condition == BlockorAttack.CoolRightCounter)
            {
                if (!_counterTime)
                {
                    _counterTime = true;
                    StartCoroutine(CoolCounterTime());
                }
            }

            //LeftBlock��RightBlock�̎��Ɏ��s
            if (_condition == BlockorAttack.LeftBlock ||
                _condition == BlockorAttack.RightBlock)
            {
                if (!_blockTime)
                {
                    _blockTime = true;
                    StartCoroutine(BlockTime());
                }
            }

            //Attack�̎��Ɏ��s
            if (_condition == BlockorAttack.Attack)
            {
                //�`���[�W��100�ȏ�ɂȂ������Ԃ��`���[�W�A�^�b�N�ɕς���B
                if (_guageAttack >= 100)
                {
                    _condition = BlockorAttack.ChageAttack;
                    ShowText("�`���[�W�A�^�b�N");
                }
                else if (!_attackTime)
                {
                    _attackTime = true;
                    StartCoroutine(AttackTime());
                }
            }

            //ChageAttack�̎��Ɏ��s
            if (_condition == BlockorAttack.ChageAttack)
            {
                //�`���[�W�A�^�b�N��������A�Q�[�W���O�ɂ��āAAttack��Ԃɖ߂�B
                var set = DataBase.BlockSkillData[DataBase._blockSkillSetNo[1]];
                Debug.Log(set.SkillName);
                _enemy.AddDebuffDamage(Attack, set.AttackValue * 5, set.EnemyOffencePower * 5, set.EnemyDiffencePower * 5);
                _guageAttack = 0;
                _condition = BlockorAttack.Attack;
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
                _condition = BlockorAttack.Attack;
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
        //��b��ɂ��ꂼ��̃u���b�N�̏����ǂ��炩�����s�B
        yield return new WaitForSeconds(1f);
        if (_survive != Survive.Death)
        {
            if (_condition == BlockorAttack.LeftBlock)
            {
                ShowText(_condition.ToString());
                _guageAttack += 5;
            }
            else if (_condition == BlockorAttack.RightBlock)
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
            if (_condition == BlockorAttack.Attack)
            {
                var set = DataBase.BlockSkillData[DataBase._blockSkillSetNo[0]];
                ShowText(set.SkillName);
                _guageAttack += 1;
                _enemy.AddDebuffDamage(Attack, set.AttackValue, set.EnemyOffencePower, set.EnemyDiffencePower);
            }
        }
        _attackTime = false;
    }

    /// <summary>�u���b�N����A�^�b�N�ɐ؂�ւ��Ƃ��̏�ԕω�</summary>
    /// <param name="i">�z��̗v�f��</param>
    /// <param name="state">��ԕω�</param>
    void CoolTime(BlockorAttack state = BlockorAttack.LeftBlock)
    {
        if (_condition != BlockorAttack.ChageAttack)
        {
            StartCoroutine(CoolTimeCoroutine());
        }
    }

    /// <summary>Block����Attack�ɐ؂�ւ��܂ł̃N�[���^�C�����������R���[�`��</summary>
    /// <returns></returns>
    IEnumerator CoolTimeCoroutine()
    {
        _condition = BlockorAttack.CoolTime;
        transform.position = _trans[0].position;
        ShowText("CoolTime");
        yield return new WaitForSeconds(1.5f);
        if (_survive != Survive.Death)
        {
            _condition = BlockorAttack.Attack;
        }
        ShowText("CoolTime�I��");
        _coolTimebool = false;
    }

    /// <summary>�J�E���^�[�ҋ@�ɂ��邽�߂̏���</summary>
    /// <param name="i"></param>
    /// <param name="counter"></param>
    void CoolCounter(int i = 1,BlockorAttack counter = BlockorAttack.CoolLeftCounter)
    {
        if (_condition == BlockorAttack.Attack)
        {
            _condition = counter;
            transform.position = _trans[i].position;
            ShowText("�J�E���^�[����");
        }
    }

    /// <summary>�J�E���^�[�����������Ƃ��̏���</summary>
    public void TrueCounter()
    {
        if (_condition == BlockorAttack.CoolLeftCounter || 
            _condition == BlockorAttack.CoolRightCounter)
        {
            var tmpCondition = _condition;
            _condition = BlockorAttack.Counter;
            StartCoroutine(TrueCounrerTime(tmpCondition));
        }
    }

    /// <summary>�J�E���^�[�����������Ƃ��̏�������Ă΂��R���[�`��</summary>
    /// <param name="tmp"></param>
    /// <returns></returns>
    IEnumerator TrueCounrerTime(BlockorAttack tmp)
    {
        //�J�E���^�[�̏���//
        Debug.Log("�J�E���^�[�I");
        _enumtext.text = "�J�E���^�[�I";
        _enemy.AddDamage(Attack,2);
        _guageAttack += 10;
        //�I���//
        yield return new WaitForSeconds(2f);
        if (_survive != Survive.Death)
        {
            if (tmp == BlockorAttack.CoolLeftCounter)
            {
                _condition = BlockorAttack.LeftBlock;
            }
            else
            {
                _condition = BlockorAttack.RightBlock;
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
            if (_condition == BlockorAttack.CoolLeftCounter ||
                _condition == BlockorAttack.CoolRightCounter)
            {
                Debug.Log("�J�E���^�[���s�A�h��Ԑ��ֈڍs");
                if (_condition == BlockorAttack.CoolLeftCounter)
                {
                    _enumtext.text = "LeftBlock";
                    _condition = BlockorAttack.LeftBlock;
                }
                else
                {
                    _enumtext.text = "RightBlock";
                    _condition = BlockorAttack.RightBlock;
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
    public enum BlockorAttack
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
