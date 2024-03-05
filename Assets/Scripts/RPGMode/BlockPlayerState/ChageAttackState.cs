using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageAttackState : MonoBehaviour,IRPGState
{
    SkillInfomation _chageSkill;

    public void Init(BlockPlayerController player)
    {
        var set = player._dataBase.BlockSkillSelectData.SkillInfomation[player._dataBase._blockSkillSetNo[1]];
        _chageSkill = set;
    }

    public void StartState(BlockPlayerController player)
    {
        player.ShowText("チャージアタック開始");
        StartCoroutine(ChangeAttackTime(player));
    }

    public void UpdateState(BlockPlayerController player)
    {
        return;
    }

    public void EndState(BlockPlayerController player)
    {
        player.ShowText("チャージアタック終了");
    }

    /// <summary>チャージアタックのアニメーション処理</summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator ChangeAttackTime(BlockPlayerController player)
    {
        //チャージアタックをした後、ゲージを０にして、Attack状態に戻る。
        Debug.Log(_chageSkill._skillName);
        player.ShowText(_chageSkill._skillName);
        ///Animationプレイ

        ///
        //アニメーション処理実装の際、アニメーションの秒数を代入
        float animationTime = 0f;
        //チャージアタックアニメーション中、時間を止めるならまた形式を変える。
        yield return new WaitForSeconds(animationTime);

        ChageAttack(player);
        player.OnChangeState(player.AttackState);
        yield return null;
    }

    /// <summary>チャージアタック</summary>
    /// <param name="player"></param>
    void ChageAttack(BlockPlayerController player)
    {
        if (_chageSkill._selectSkill is BlockSkillSelect blockSkill)
        {
            player._enemy.AddDebuffDamage(player.Attack, blockSkill.AttackValue, blockSkill.EnemyOffencePower, blockSkill.EnemyDiffencePower);
        }
    }

}
