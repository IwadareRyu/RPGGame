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
    [Tooltip("BlockPlayer�̏��")]
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Q�{�^���ō��Ɉړ����ď�Ԃ�LeftBlock�ɂȂ�B
        if (Input.GetButton("BlockLeft"))
        {
            DistanceMove(1);
        }
        //E�{�^���ŉE�Ɉړ����ď�Ԃ�RightBlock�ɂȂ�B
        else if (Input.GetButton("BlockRight"))
        {
            DistanceMove(2,BlockorAttack.RightBlock);
        }
        //�j���[�g�����Ő^�񒆂Ɉړ����ď�Ԃ�Attack�ɂȂ�B
        else
        {
            DistanceMove(0,BlockorAttack.Attack);
        }

        //LeftBlock��RightBlock�̎��Ɏ��s
        if(_condition == BlockorAttack.LeftBlock || _condition == BlockorAttack.RightBlock)
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
            Debug.Log("ChageAttack!");
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
            Debug.Log("LeftBlock");
            _guageAttack += 5;
        }
        else if(_condition == BlockorAttack.RightBlock)
        {
            Debug.Log("RightBlock");
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
            Debug.Log("Attack");
            _guageAttack += 1;
        }
        _attackTime = false;
    }

    /// <summary>�������v�Z���ē���̈ʒu�ֈړ����郁�\�b�h�B�ړ����I��������ԕω�</summary>
    /// <param name="i">�z��̗v�f��</param>
    /// <param name="state">��ԕω�</param>
    void DistanceMove(int i,BlockorAttack state = BlockorAttack.LeftBlock)
    {
        if (_condition != BlockorAttack.ChageAttack)
        {
            float distance = Vector2.Distance(transform.position, _trans[i].position);
            if (distance > _stopdis)
            {
                _condition = BlockorAttack.CoolTime;
                Vector3 dir = (_trans[i].position - transform.position).normalized * _speed;
                dir.y = 0;
                transform.Translate(dir * Time.deltaTime);
            }
            else
            {
                _condition = state;
            }
        }
    }

    /// <summary>BlockPlayer�̏��</summary>
    public enum BlockorAttack
    {
        [Tooltip("�ړ����Ă���Ԃ�CoolTime")]
        CoolTime,
        [Tooltip("���̃K�[�h")]
        LeftBlock,
        [Tooltip("�E�̃K�[�h")]
        RightBlock,
        [Tooltip("�U��")]
        Attack,
        [Tooltip("�`���[�W�U��")]
        ChageAttack,
    }
}
