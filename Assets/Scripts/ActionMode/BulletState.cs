/// <summary>�e�̓�����State</summary>
public enum BulletState
{
    //�܂�������Ԓe
    ForwardMove,

    //��]���鋅
    RotationMove,

    //������]������A�܂�������Ԓe�B
    HalfRotationMove,
}

/// <summary>�e�̐������@��State</summary>
public enum SpawnState
{
    //�����Ă�������̂݁B
    Forward,
    
    //��x��360�x�e���o���B
    Circle,
    
    //�����360�x�e���o���B
    DelayCircle,
}
