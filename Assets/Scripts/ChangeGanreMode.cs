using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGanreMode : MonoBehaviour
{
    [SerializeField] ActionRPGUIChange _changeUI;
    [SerializeField] ChangeActionRPG _changeGanreState = ChangeActionRPG.RPG;
    [SerializeField] BulletSpawnEnemy[] enemys;
    [SerializeField] CapsuleCollider RPGEnemy;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("‚³‚“‚‡‚“");
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(_changeUI.ChangeGenre((int)_changeGanreState));
        }
    }
}
