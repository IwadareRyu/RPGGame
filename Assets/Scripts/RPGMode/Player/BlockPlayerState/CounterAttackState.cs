using System;
using UnityEngine;

/// <summary>カウンター攻撃をするState</summary>
[Serializable]
public class CounterAttackState : IRPGState
{
    string _animationName;
    float _animationTime = 0f;
    float _currentTime;
    [Tooltip("カウンター攻撃の倍率"), Header("カウンター攻撃の倍率")]
    [SerializeField] float _mag = 2f;
    public void Init(BlockPlayerController player)
    {
        //アニメーションの名前入力
        _animationName = "aa";
    }

    public void StartState(BlockPlayerController player)
    {
        Debug.Log("カウンター！");
        player.ConditionTextViewAccess("カウンター！");
        /// アニメーションを再生してアニメーションの秒数を代入する。

        _animationTime = 1f;
        _currentTime = 0f;
        ///
    }

    public void UpdateState(BlockPlayerController player)
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > _animationTime)
        {
            player.EndCounter();
        }
    }

    public void EndState(BlockPlayerController player)
    {
        // 今の所はEndStateにダメージ判定
        player._enemy.AddDamage(player.Attack, _mag);
        player._guageAttack += 10;
        Debug.Log("カウンター終わり");
    }

}
