using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CoolDownState : IRPGState
{
    [SerializeField] float _coolDownTime = 1f;
    float _currentTime = 0f;
    public void Init(BlockPlayerController player)
    {

    }

    public void StartState(BlockPlayerController player)
    {
        Debug.Log("クールダウン");
        player.ConditionTextViewAccess("クールダウン中");
        player._targetGuard = TargetGuard.None;
        player.transform.position = player._trans[(int)TargetGuard.None].position;
        _currentTime = 0f;
        player.ChantingViewAccess(_currentTime, _coolDownTime);
    }

    public void UpdateState(BlockPlayerController player)
    {
        _currentTime += Time.deltaTime;
        player.ChantingViewAccess(_currentTime,_coolDownTime);

        if(_currentTime > _coolDownTime)
        {
            player.OnChangeState(player.AttackState);
        }
    }

    public void EndState(BlockPlayerController player)
    {
        Debug.Log("クールダウン終了");
        _currentTime = 0f;
        player.ChantingViewAccess(_currentTime, _coolDownTime);
    }

}
