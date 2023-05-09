using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : StatusClass
{
    private BlockPlayer _blockPlayer;
    private MagicPlayer _magicPlayer;
    private AttackPlayer _attackPlayer;
    [SerializeField] Text _enemytext;
    bool _enemyAttackbool;
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
        if(!_enemyAttackbool)
        {
            _enemyAttackbool = true;
            StartCoroutine(EnemyAttackCoolTime());
        }
    }

    IEnumerator EnemyAttackCoolTime()
    {
        yield return new WaitForSeconds(5f);
        var ram = Random.Range(0,100);
        if(ram < 50)
        {
            _enemytext.text = "Magicに攻撃";
            StartCoroutine(EnemyAttack(true));
        }
        else
        {
            _enemytext.text = "Attackerに攻撃";
            StartCoroutine(EnemyAttack(false));
        }
    }

    IEnumerator EnemyAttack(bool magicTAttackF)
    {
        yield return new WaitForSeconds(1.5f);
        if(magicTAttackF)
        {
            if(_blockPlayer.Condition == BlockPlayer.BlockorAttack.LeftBlock)
            {
                Guard();
            }
            else if(_blockPlayer.Condition == BlockPlayer.BlockorAttack.CoolLeftCounter)
            {
                Counter();
            }
            else
            {
                _enemytext.text = "Magicにダメージ！";
                _magicPlayer.AddDamage(Attack);
            }
        }
        else
        {
            if(_blockPlayer.Condition == BlockPlayer.BlockorAttack.RightBlock)
            {
                Guard();
            }
            else if(_blockPlayer.Condition == BlockPlayer.BlockorAttack.CoolRightCounter)
            {
                Counter();
            }
            else
            {
                _enemytext.text = "Attackerにダメージ！";
                _attackPlayer.AddDamage(Attack);
            }
        }
        _enemyAttackbool = false;
    }

    void Guard()
    {
        _enemytext.text = "ガードされた！";
        _blockPlayer.AddDamage(Attack);
    }

    void Counter()
    {
        _enemytext.text = "カウンターされた！";
        _blockPlayer.TrueCounter();
        _blockPlayer.AddDamage(Attack,0.5f);
    }

    enum EnemyEnum
    {
        CoolTime,
        RightAttack,
        LeftAttack,
        AllAttack,
    }
}
