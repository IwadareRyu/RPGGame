using System;
using UnityEngine;

/// <summary>クールタイムのState</summary>
[Serializable]
public class CoolDownStateVer2 : IRPGStateVer2
{
    [SerializeField] float _coolDownTime = 1f;
    float _currentTime = 0f;
    public void Init(RPGPlayerVer2 player)
    {
        return;
    }

    public void StartState(RPGPlayerVer2 player)
    {
        Debug.Log("クールダウン");
        player.ConditionTextViewAccess("クールダウン中");
        _currentTime = 0f;
        player.ChantingViewAccess(_currentTime, _coolDownTime);
    }

    public void UpdateState(RPGPlayerVer2 player)
    {
        _currentTime += Time.deltaTime;
        player.ChantingViewAccess(_currentTime, _coolDownTime);

        if (_currentTime > _coolDownTime)
        {
            //player.OnChangeState(player.AttackState);
        }
    }

    public void EndState(RPGPlayerVer2 player)
    {
        Debug.Log("クールダウン終了");
        _currentTime = 0f;
        player.ChantingViewAccess(_currentTime, _coolDownTime);
    }
}
