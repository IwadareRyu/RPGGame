using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRPGStateVer2
{
    /// <summary>Awakeで呼ばれるメソッド</summary>
    public void Init(RPGPlayerVer2 player);

    /// <summary>Stateに入った時呼ばれるメソッド</summary>
    public void StartState(RPGPlayerVer2 player);

    /// <summary>Update時、呼ばれるメソッド</summary>
    public void UpdateState(RPGPlayerVer2 player);

    /// <summary>Stateを抜ける時呼ばれるメソッド</summary>
    public void EndState(RPGPlayerVer2 player);
}
