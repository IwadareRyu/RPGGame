using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>味方をガードするState</summary>
public class GuardState : IRPGState
{

    public void Init(BlockPlayerController player)
    {

    }

    public void StartState(BlockPlayerController player)
    {
        Debug.Log("ガード開始");
        player.ShowText($"{player._targetGuard}をガード中");
    }

    public void UpdateState(BlockPlayerController player)
    {
        if(player._targetGuard == TargetGuard.Magician && Input.GetButtonUp("BlockLeft") ||
           player._targetGuard == TargetGuard.Attacker && Input.GetButtonUp("BlockRight"))
        {
            player.OnChangeState(player.CoolDownState);
        }
    }

    public void EndState(BlockPlayerController player)
    {
        Debug.Log("ガード終了");
    }
}
