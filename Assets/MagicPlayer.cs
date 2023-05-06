using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlayer : MonoBehaviour
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

    void Start()
    {
        
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
            Debug.Log(_attackMagic);
        }
        else
        {
            _blockMagic = (BlockMagic)i;
            Debug.Log(_blockMagic);
        }
    }

    IEnumerator MagicTime()
    {
        yield return new WaitForSeconds(5f);
        if(_magicpos == MagicPosition.AttackMagic)
        {
            Debug.Log(_attackMagic + "発動！");
        }
        else
        {
            Debug.Log(_blockMagic + "発動！");
        }
        _magicTime = false;
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
