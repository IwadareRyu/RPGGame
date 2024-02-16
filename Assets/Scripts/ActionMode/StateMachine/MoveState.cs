using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : MonoBehaviour,IState
{
    [Header("プレイヤーの動き設定")]
    [Tooltip("プレイヤーの動きの速さ")]
    [SerializeField] float _speed = 2f;
    [SerializeField] float _dashPower = 5f;
    [SerializeField] float _gravityPower = 3f;
    Rigidbody _rb;
    public void Init(ActionStateMachine stateMachine)
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void StartState(ActionStateMachine stateMachine)
    {

    }

    public void UpdateState(ActionStateMachine stateMachine)
    {

    }

    public void EndState(ActionStateMachine stateMachine)
    {
        
    }    
}
