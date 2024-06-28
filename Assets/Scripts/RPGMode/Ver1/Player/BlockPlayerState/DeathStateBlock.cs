using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathStateBlock : IRPGState
{
    public void Init(BlockPlayerController player)
    {

    }

    public void StartState(BlockPlayerController player)
    {
        Debug.Log("戦闘不能");
        player.transform.position = player._trans[0].position;
        player.ConditionTextViewAccess("やられた～");
    }

    public void UpdateState(BlockPlayerController player)
    {
        if(player.HP > 0)
        {
            player.OnChangeState(player.AttackState);
        }
    }

    public void EndState(BlockPlayerController player)
    {

    }
}
