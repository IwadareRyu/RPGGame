using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>チャージ攻撃のState</summary>
public class ChargeAttackState : MonoBehaviour,IRPGState
{
    SkillInfomation _chageSkill;

    public void Init(BlockPlayerController player)
    {
        var set = player._dataBase.BlockSkillSelectData.SkillInfomation[player._dataBase._blockSkillSetNo[1]];
        _chageSkill = set;
    }

    public void StartState(BlockPlayerController player)
    {
        player.ShowText("チャージ攻撃開始");
        StartCoroutine(ChangeAttackTime(player));
    }

    public void UpdateState(BlockPlayerController player)
    {
        return;
    }

    public void EndState(BlockPlayerController player)
    {
        player.ShowText("チャージ攻撃終了");
    }

    /// <summary>チャージ攻撃のアニメーション処理</summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator ChangeAttackTime(BlockPlayerController player)
    {
        //チャージ攻撃をした後、ゲージを０にして、Attack状態に戻る。
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

    /// <summary>チャージ攻撃</summary>
    /// <param name="player"></param>
    void ChageAttack(BlockPlayerController player)
    {
        if (_chageSkill._selectSkill is BlockSkillSelect blockSkill)
        {
            player._enemy.AddDebuffDamage(
                player.Attack, 
                blockSkill.AttackValue, 
                blockSkill.EnemyOffencePower, 
                blockSkill.EnemyDiffencePower
                );
            AudioManager.Instance.SEPlay(SE.BlockerAttack);
        }
    }

}
