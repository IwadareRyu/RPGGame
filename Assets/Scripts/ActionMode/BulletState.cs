/// <summary>弾の動きのState</summary>
public enum BulletState
{
    //まっすぐ飛ぶ弾
    ForwardMove,

    //回転する球
    RotationMove,

    //少し回転した後、まっすぐ飛ぶ弾。
    HalfRotationMove,
}

/// <summary>弾の生成方法のState</summary>
public enum SpawnState
{
    //向いている方向のみ。
    Forward,
    
    //一度に360度弾を出す。
    Circle,
    
    //一つずつ360度弾を出す。
    DelayCircle,
}
