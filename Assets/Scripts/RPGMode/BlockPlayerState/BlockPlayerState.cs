using UnityEngine;

/// <summary>BlockPlayer��State</summary>
public enum BlockState
{
    [Tooltip("�ړ����Ă���Ԃ�CoolTime")]
    CoolTime,
    [Tooltip("�K�[�h")]
    Block,
    [Tooltip("�J�E���^�[����")]
    CoolCounter,
    [Tooltip("�J�E���^�[")]
    Counter,
    [Tooltip("�U��")]
    Attack,
    [Tooltip("�`���[�W�U��")]
    ChageAttack,
}

/// <summary>�ǂ̖�������邩</summary>
public enum TargetGuard
{
    None,
    Magician,
    Attacker,
}