using System;
using UnityEngine;

/// <summary>スキルの詠唱時間</summary>
[Serializable]
public class ChastTimeAttacker : AttackPlayerCoolTimeClass
{
    [SerializeField] float _normalAttackCoolTime = 1f;
    [Tooltip("スキルの詠唱時間")]
    float _chastCoolTime;
    [Tooltip("現在の詠唱時間")]
    float _currentChastCoolTime;

    public void StartCoolTime(AttackPlayer player)
    {
        // SkillChoiceNumberが-1なら通常攻撃、違うなら対応したスキル攻撃の詠唱時間を設定する。
        if (player.SkillChoiceNumber != -1)
        {
            _chastCoolTime = DataBase.Instance.AttackSkillSelectData.SkillInfomation[
                DataBase.Instance._attackSkillSetNo[
                    player.SkillChoiceNumber
                    ]
                ]._chastingTime;
        }
        else { _chastCoolTime = _normalAttackCoolTime; }

        //詠唱時間の初期化処理
        _currentChastCoolTime = 0f;
        player.ChantingViewAccess(_currentChastCoolTime, _chastCoolTime);
    }

    public void UpdateCoolTime(AttackPlayer player)
    {
        // 詠唱時間の更新処理
        _currentChastCoolTime += Time.deltaTime;
        player.ChantingViewAccess(_currentChastCoolTime, _chastCoolTime);
        if (_currentChastCoolTime < _chastCoolTime) { return; }
        // 詠唱が終わったら攻撃する。
        player.SkillAttack(player.SkillChoiceNumber);
    }

    public void EndCoolTime(AttackPlayer player)
    {
        // 詠唱時間の初期化処理(Sliderの値を0にするため初期化)
        _currentChastCoolTime = 0;
        player.ChantingViewAccess(_currentChastCoolTime, _chastCoolTime);
    }

}
