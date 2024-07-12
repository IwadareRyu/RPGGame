using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary>カウンター状態のState</summary>
public class CounterStateVer2 : IRPGStateVer2
{
    [SerializeField] float _counterTime = 1.0f;
    [SerializeField] float _damagePersent = 2.0f;
    float _currentTime;
    public void Init(RPGPlayerVer2 player)
    {
        return;
    }

    public void StartState(RPGPlayerVer2 player)
    {
        _currentTime = 0f;
    }

    public void UpdateState(RPGPlayerVer2 player)
    {
        _currentTime += Time.deltaTime;
        if(_currentTime > _counterTime)
        {
            player.OnChangeState(player.CoolDownState);
        }
    }

    public void EndState(RPGPlayerVer2 player)
    {
        player._blockTime = true;
    }

    public void TrueCounter(RPGPlayerVer2 player)
    {
        player._enemy.AddDamage(player.Attack,_damagePersent);
        player.OnChangeState(player.CoolDownState);
    }
}
