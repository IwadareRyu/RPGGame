﻿using System.Collections;
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
        player.ConditionTextViewAccess($"{player._targetGuard}をガード中");
    }

    public void UpdateState(BlockPlayerController player)
    {
        if(player._targetGuard == TargetGuard.Magician && !Input.GetButton("BlockLeft") ||
           player._targetGuard == TargetGuard.Attacker && !Input.GetButton("BlockRight"))
        {
            player.OnChangeState(player.CoolDownState);
        }
    }

    public void EndState(BlockPlayerController player)
    {
        Debug.Log("ガード終了");
    }
}
