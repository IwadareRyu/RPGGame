using System;
using System.Collections;
using UnityEngine;

/// <summary>通常攻撃のState</summary>
[Serializable]
public class AttackStateBlock : MonoBehaviour,IRPGState
{
    SkillInfomation _normalSkill;
    float _time;
    [SerializeField] float _attackCoolTime = 2f;
    bool _isAttackTime;

    public void Init(BlockPlayerController player)
    {
        // BlockSkillの通常攻撃スキルを_skillに代入する。
        var set = player._dataBase.BlockSkillSelectData.SkillInfomation[player._dataBase._blockSkillSetNo[0]];
        _normalSkill = set;
        _isAttackTime = false;
        _time = 0;
        player.ChantingViewAccess(_time,_attackCoolTime);
    }

    public void StartState(BlockPlayerController player)
    {
        _isAttackTime = false;
        Debug.Log("攻撃開始:Block");
        player.ShowText("攻撃開始");
    }

    public void UpdateState(BlockPlayerController player)
    {
        if (Input.GetButtonDown("BlockLeft"))
        {
            // Magicianの前に立ち、CoolCounterに移行。
            ChangeCoolCounterState(player, TargetGuard.Magician);

        }
        else if (Input.GetButtonDown("BlockRight"))
        {
            // Attackerの前に立ち、CoolCounterに移行。
            ChangeCoolCounterState(player, TargetGuard.Attacker);
        }

        if (!_isAttackTime)
        {
            _time += Time.deltaTime;
            player.ChantingViewAccess(_time, _attackCoolTime);
        }

        if (_time > _attackCoolTime)
        {
            //チャージが100以上になったら状態をチャージアタックのStateに移行する。
            if (player._guageAttack >= 100)
            {
                // チャージアタックに移行
                player.OnChangeState(player.ChageAttackState);
            }
            else
            {
                _isAttackTime = true;
                StartCoroutine(AttackTime(player));
                _time = 0f;
                player.ChantingViewAccess(_time, _attackCoolTime);
            }
        }
    }

    /// <summary>targetを前に立ち、targetを守るStateに移行するメソッド</summary>
    void ChangeCoolCounterState(BlockPlayerController player,TargetGuard target)
    {
        player._targetGuard = target;
        player.transform.position = player._trans[(int)target].position;
        player.OnChangeState(player.CoolCounterState);
    }

    /// <summary>スキルで敵にダメージを与えるメソッドa</summary>
    /// <param name="player"></param>
    IEnumerator AttackTime(BlockPlayerController player)
    {
        player.ShowText(_normalSkill._skillName);
        player._guageAttack += 1;
        ///Animationつける
        
        ///
        //アニメーション処理実装の際、アニメーションの秒数を代入
        float animationTime = 0f;
        for(var i = 0;i < animationTime;i++)
        {
            if(Input.GetButtonDown("BlockLeft") || Input.GetButtonDown("BlockRight"))
            {
                ///アニメーション中断処理

                ///
                Debug.Log("攻撃中断。");
                yield break;
            }   //ボタン入力されたらアニメーション中断してコルーチンを抜ける。
            yield return null;
        }

        // アニメーションのフラグから処理を呼ぶ可能性もあるが今の所ここに攻撃処理を書く。
        Attack(player);

        _isAttackTime = false;

        yield return null;
    }

    /// <summary>攻撃処理</summary>
    /// <param name="player"></param>
    void Attack(BlockPlayerController player)
    {
        if (_normalSkill._selectSkill is BlockSkillSelect blockSkill)
        {
            player._enemy.AddDebuffDamage(
                player.Attack,
                blockSkill.AttackValue,
                blockSkill.EnemyOffencePower,
                blockSkill.EnemyDiffencePower);
            AudioManager.Instance.SEPlay(SE.BlockerAttack);
        }
    }

    public void EndState(BlockPlayerController player)
    {
        _time = 0;
        player.ChantingViewAccess(_time, _attackCoolTime);
        Debug.Log("攻撃終了:Block");
    }
}
