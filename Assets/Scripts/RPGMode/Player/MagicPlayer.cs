using RPGBattle;
using System.Collections;
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
    [SerializeField] float _magicCoolTime = 5f;
    float _currentMagicCoolTime = 0f;
    int _magicSkillCount = 0;
    [SerializeField] float _attackLange = 3;
    [SerializeField] Animator _animRobot;
    [SerializeField] Text _enumtext;
    [SerializeField] AttackPlayer _attackPlayer;
    [SerializeField] BlockPlayerController _blockPlayer;
    [SerializeField] Transform _magicObj;
    [SerializeField] GameObject _ship;
    DataBase _dataBase;

    void Start()
    {
        _dataBase = DataBase.Instance;
        _enemy = GameObject.FindGameObjectWithTag("RPGEnemy")?.GetComponent<EnemyController>();
        SetStatus();
        HPViewAccess();
        ChantingViewAccess(_currentMagicCoolTime,_magicCoolTime);
        Debug.Log($"MagicHP:{HP}");
        Debug.Log($"MagicAttack:{Attack}");
        Debug.Log($"MagicDiffence:{Diffence}");
    }



    // Update is called once per frame
    void Update()
    {
        if (RPGBattleManager.Instance.BattleState != BattleState.RPGBattle) { return; }

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
            TimeMethod();

            if (HP == 0)
            {
                _survive = Survive.Death;
            }
        }
        else
        {
            Death();
        }
    }

    public override void ActionMode()
    {
        ChangeCondition(0, MagicPosition.AttackMagic);
        for (var i = 0; i < _dataBase._attackMagicbool.Length; i++)
        {
            _magicSkillCount += _dataBase._attackMagicbool[i] ? 1 : 0;
        }
        ShowText("LeftShiftでAttack！");
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
        _currentMagicCoolTime = 0;
        ChantingViewAccess(_currentMagicCoolTime, _magicCoolTime);
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
        _currentMagicCoolTime = 0;
        ChantingViewAccess(_currentMagicCoolTime, _magicCoolTime);
    }

    /// <summary>RPGモード時の攻撃サポート</summary>
    /// <returns></returns>
    IEnumerator MagicTime()
    {
        for (; _currentMagicCoolTime < _magicCoolTime; _currentMagicCoolTime += Time.deltaTime)
        {
            ChantingViewAccess(_currentMagicCoolTime, _magicCoolTime);
            yield return new WaitForEndOfFrame();
        }

        if (_survive == Survive.Survive && RPGBattleManager.Instance.BattleState == BattleState.RPGBattle)
        {
            if (_magicpos == MagicPosition.AttackMagic)
            {
                var set = _dataBase.AttackMagicSelectData.SkillInfomation[_dataBase._attackMagicSetNo[(int)_attackMagic]];
                ShowText($"{set._skillName}！");
                if (set._selectSkill is AttackMagicSelect attackMagic)
                {
                    switch (set._skillID)
                    {
                        case 11:
                            var insShip = Instantiate(_ship, InsObjPoint);
                            yield return new WaitForSeconds(1f);
                            AudioManager.Instance.SEPlay(SE.Explosion);
                            _enemy.AddMagicDamage(Attack, attackMagic.AttackValue);

                            break;
                        default:
                            _animRobot.SetTrigger("NormalAttack");
                            AudioManager.Instance.SEPlay(SE.MagicianAttack);
                            _enemy.AddMagicDamage(Attack, attackMagic.AttackValue);
                            break;

                    }
                }
            }
            else
            {
                var set = _dataBase.BlockMagicSelectData.SkillInfomation[_dataBase._blockMagicSetNo[(int)_blockMagic]];
                ShowText($"{set._skillName}！");
                if (set._selectSkill is BlockMagicSelect blockMagic)
                {
                    _attackPlayer.AddBuff(blockMagic.PlusAttackPower, blockMagic.PlusDiffencePower, blockMagic.HealingHP);
                    _blockPlayer.AddBuff(blockMagic.PlusAttackPower, blockMagic.PlusDiffencePower, blockMagic.HealingHP);
                    AddBuff(blockMagic.PlusAttackPower, blockMagic.PlusDiffencePower, blockMagic.HealingHP);
                    AudioManager.Instance.SEPlay(SE.Equip);
                }
            }
        }
        _currentMagicCoolTime = 0;
        ChantingViewAccess(_currentMagicCoolTime,_magicCoolTime);
        _magicTime = false;
    }

    public override void ChantingTimeReset()
    {
        _currentMagicCoolTime = 0;
        ChantingViewAccess(_currentMagicCoolTime, _magicCoolTime);
    }

    public override void RPGMode()
    {
        ShowText("無事だったか！");
    }

    /// <summary>テキスト表示</summary>
    /// <param name="str"></param>
    void ShowText(string str)
    {
        _enumtext.text = str;
    }

    /// <summary>死亡判定</summary>
    void Death()
    {
        RPGBattleManager.Instance.BattleEnd();
        FightManager.Instance.Lose();
        ShowText("☆Magicianは星になった☆");
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
