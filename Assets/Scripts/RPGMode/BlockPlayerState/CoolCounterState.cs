using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CoolCounterState : IRPGState
{
    float _currentTime = 0f;
    [SerializeField] float _counterTime;
    public void Init(BlockPlayerController player)
    {

    }

    public void StartState(BlockPlayerController player)
    {
        _currentTime = 0f;
        Debug.Log("�J�E���^�[����");
        player.ShowText("�J�E���^�[�����悤�Ƃ��Ă���...");
    }

    public void UpdateState(BlockPlayerController player)
    {
        if (!player.IsCounter)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > 0.3f)
            {
                player.OnChangeState(player.GuardState);
            }
        }
    }

    public void EndState(BlockPlayerController player)
    {
        Debug.Log("�J�E���^�[�����I���");
    }
}
