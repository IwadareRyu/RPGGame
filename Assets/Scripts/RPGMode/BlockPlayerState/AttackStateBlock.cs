using System;
using UnityEngine;

[Serializable]
public class AttackStateBlock : IRPGState
{
    float _time;
    [SerializeField] float _attackCoolTime = 2f;

    public void Init(BlockPlayerController player)
    {
        _time = 0;
    }

    public void StartState(BlockPlayerController player)
    {
        Debug.Log("攻撃開始:Block");
    }

    public void UpdateState(BlockPlayerController player)
    {
        if (Input.GetButtonDown("BlockLeft"))
        {
            // LeftCounterに移行。
            //player.OnChangeState(player.LeftCounterActiveTime);
        }
        else if (Input.GetButtonDown("BlockRight"))
        {
            // RightCounterに移行。
            //player.OnChangeState(player.RightCounterActiveTime);
        }

        _time += Time.deltaTime;
        if (_time > _attackCoolTime)
        {
            //チャージが100以上になったら状態をチャージアタックのStateに移行する。
            if (player._guageAttack >= 100)
            {
                // チャージアタックに移行
                //stateMachine.OnChangeState(stateMachine.ChageAttack);
            }
            else
            {
                Attack(player);
            }
        }
    }

    /// <summary>スキルで敵にダメージを与えるメソッドa</summary>
    /// <param name="player"></param>
    void Attack(BlockPlayerController player)
    {
        var set = player.DataBase.BlockSkillSelectData.SkillInfomation[player.DataBase._blockSkillSetNo[0]];
        player.ShowText(set._skillName);
        player._guageAttack += 1;
        if (set._selectSkill is BlockSkillSelect blockSkill)
        {
            player._enemy.AddDebuffDamage(
                player.Attack,
                blockSkill.AttackValue,
                blockSkill.EnemyOffencePower,
                blockSkill.EnemyDiffencePower);
        }
    }

    public void EndState(BlockPlayerController player)
    {
        _time = 0;
        Debug.Log("攻撃終了:Block");
    }
}
