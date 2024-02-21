using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateMachine : MonoBehaviour
{
    [SerializeField] IState[] _states;
    IState _nowState;

    private void Awake()
    {
        
    }
}
