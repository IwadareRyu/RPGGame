using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AttackPlayerCoolTimeClass
{
    abstract void StartCoolTime(AttackPlayer player);
    abstract void UpdateCoolTime(AttackPlayer player);
    abstract void EndCoolTime(AttackPlayer player);
}
