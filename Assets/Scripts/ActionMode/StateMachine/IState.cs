using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Init(ActionStateMachine stateMachine);
    public void StartState(ActionStateMachine stateMachine);
    public void UpdateState(ActionStateMachine stateMachine);
    public void EndState(ActionStateMachine stateMachine);
}
