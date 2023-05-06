using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockPlayer : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Q�{�^���ō��Ɉړ����ď�Ԃ�CoolLeftCounter�ɂȂ�B
        if (Input.GetButton("BlockLeft") && 
            _condition != BlockorAttack.CoolTime &&
            _condition != BlockorAttack.RightBlock)
        {
            CoolCounter(1,BlockorAttack.CoolLeftCounter);
        }
        //E�{�^���ŉE�Ɉړ����ď�Ԃ�CoolRightCounter�ɂȂ�B
        else if (Input.GetButton("BlockRight") &&
            _condition != BlockorAttack.CoolTime &&
            _condition != BlockorAttack.LeftBlock)
        {
            CoolCounter(2, BlockorAttack.CoolRightCounter);
        }
        //�j���[�g�����ŃJ�E���^�[��Ԃ���Ȃ�����A�^�񒆂Ɉړ����ď�Ԃ�Attack�ɂȂ�B
        else if(_condition != BlockorAttack.CoolLeftCounter && 
            _condition != BlockorAttack.CoolRightCounter && 
            _condition != BlockorAttack.Counter)
        {
            DistanceMove(BlockorAttack.Attack);
        }

        //CoolLeftCounter��CoolRightCounter���Ɏ��s
        if(_condition == BlockorAttack.CoolLeftCounter || 
            _condition == BlockorAttack.CoolRightCounter)
        {
            if(!_counterTime)
            {
                Debug.Log(_condition);
                _counterTime = true;
                StartCoroutine(CoolCounterTime());
            }
        }

        //LeftBlock��RightBlock�̎��Ɏ��s
        if(_condition == BlockorAttack.LeftBlock || 
            _condition == BlockorAttack.RightBlock)
        {
            if (!_blockTime)
            {
                _blockTime = true;
                StartCoroutine(BlockTime());
            }
        }

        //Attack�̎��Ɏ��s
        if(_condition == BlockorAttack.Attack)
        {
            //�`���[�W��100�ȏ�ɂȂ������Ԃ��`���[�W�A�^�b�N�ɕς���B
            if (_guageAttack >= 100)
            {
                _condition = BlockorAttack.ChageAttack;
            }
            else if(!_attackTime)
            {
                _attackTime = true;
                StartCoroutine(AttackTime());
            }
        }

        //ChageAttack�̎��Ɏ��s
        if(_condition == BlockorAttack.ChageAttack)
        {
            //�`���[�W�A�^�b�N��������A�Q�[�W���O�ɂ��āAAttack��Ԃɖ߂�B
            Debug.Log(_condition);
            _guageAttack = 0;
            _condition = BlockorAttack.Attack;
        }
    }

    /// <summary>�u���b�N�����Ă���ۂ̃R���[�`��</summary>
    /// <returns></returns>
    IEnumerator BlockTime()
    {
        //��b��ɂ��ꂼ��̃u���b�N�̏����ǂ��炩�����s�B
        yield return new WaitForSeconds(1f);
        if (_condition == BlockorAttack.LeftBlock)
        {
            Debug.Log(_condition);
            _guageAttack += 5;
        }
        else if(_condition == BlockorAttack.RightBlock)
        {
            Debug.Log(_condition);
            _guageAttack += 5;
        }
        _blockTime = false;
    }

    /// <summary>�A�^�b�N�����Ă���ۂ̃R���[�`��</summary>
    /// <returns></returns>
    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(2f);
        if (_condition == BlockorAttack.Attack)
        {
            Debug.Log(_condition);
            _guageAttack += 1;
        }
        _attackTime = false;
    }

    /// <summary>�������v�Z���ē���̈ʒu�ֈړ����郁�\�b�h�B�ړ����I��������ԕω�</summary>
    /// <param name="i">�z��̗v�f��</param>
    /// <param name="state">��ԕω�</param>
    void DistanceMove(BlockorAttack state = BlockorAttack.LeftBlock)
    {
        if (_condition != BlockorAttack.ChageAttack)
        {
            float distance = Vector2.Distance(transform.position, _trans[0].position);
            if (distance > _stopdis)
            {
                _condition = BlockorAttack.CoolTime;
                Vector3 dir = (_trans[0].position - transform.position).normalized * _speed;
                dir.y = 0;
                transform.Translate(dir * Time.deltaTime);
            }
            else
            {
                _condition = state;
            }
        }
    }

    /// <summary>�J�E���^�[�ҋ@�ɂ��邽�߂̏���</summary>
    /// <param name="i"></param>
    /// <param name="counter"></param>
    void CoolCounter(int i = 1,BlockorAttack counter = BlockorAttack.CoolLeftCounter)
    {
        if (_condition == BlockorAttack.Attack)
        {
            transform.position = _trans[i].position;
            _condition = counter;
            Debug.Log("�J�E���^�[����");
        }
    }

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

    IEnumerator TrueCounrerTime(BlockorAttack tmp)
    {
        //�J�E���^�[�̏���//
        Debug.Log("�J�E���^�[�U���I");
        //�I���//
        yield return new WaitForSeconds(2f);
        if(tmp == BlockorAttack.CoolLeftCounter)
        {
            _condition = BlockorAttack.LeftBlock;
        }
        else
        {
            _condition = BlockorAttack.RightBlock;
        }
    }

    IEnumerator CoolCounterTime()
    {
        yield return new WaitForSeconds(2f);
        if (_condition == BlockorAttack.CoolLeftCounter || 
            _condition == BlockorAttack.CoolRightCounter)
        {
            Debug.Log("�J�E���^�[���s�A�h��Ԑ��ֈڍs");
            if (_condition == BlockorAttack.CoolLeftCounter)
            {
                _condition = BlockorAttack.LeftBlock;
            }
            else
            {
                _condition = BlockorAttack.RightBlock;
            }
        }
        _counterTime = false;
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
