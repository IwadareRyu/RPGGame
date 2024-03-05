using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CounterAttackState : IRPGState
{
    string _animationName;
    float _animationTime = 0f;
    float _currentTime;
    [SerializeField] float mag;
    public void Init(BlockPlayerController player)
    {
        //�A�j���[�V�����̖��O����
        _animationName = "aa";
    }

    public void StartState(BlockPlayerController player)
    {
        Debug.Log("�J�E���^�[�I");
        player.ShowText("�J�E���^�[�I");
        /// �A�j���[�V�������Đ����ăA�j���[�V�����̕b����������B

        _animationTime = 1f;
        _currentTime = 0f;
        ///
    }

    public void UpdateState(BlockPlayerController player)
    {
        _currentTime = Time.deltaTime;

        if(_currentTime > _animationTime)
        {
            player.OnChangeState(player.GuardState);
        }
    }

    public void EndState(BlockPlayerController player)
    {
        // ���̏���EndState�Ƀ_���[�W����
        player._enemy.AddDamage(player.Attack, 2);
        player._guageAttack += 10;
        Debug.Log("�J�E���^�[�I���");
    }

}
