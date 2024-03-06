﻿using UnityEngine;

/// <summary>BlockPlayerのState</summary>
public enum BlockState
{
    [Tooltip("移動している間のCoolTime")]
    CoolTime,
    [Tooltip("ガード")]
    Block,
    [Tooltip("カウンター準備")]
    CoolCounter,
    [Tooltip("カウンター")]
    Counter,
    [Tooltip("攻撃")]
    Attack,
    [Tooltip("チャージ攻撃")]
    ChageAttack,
}

/// <summary>どの味方を守るか</summary>
public enum TargetGuard
{
    None,
    Magician,
    Attacker,
}