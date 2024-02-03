using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEnemy : MonoBehaviour
{
    bool _stun = false;
    public bool Stun => _stun;
    [SerializeField] Animator _anim;
    void Update()
    {
        
    }

    public void ChangeStun()
    {
        _anim.Play("StunStart");
        _stun = true;
    }
    public void ResetStun()
    {
        _anim.Play("Stand");
        _stun = false;
    }
}
