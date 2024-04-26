using RPGBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRPGTutorial : MonoBehaviour
{
    [SerializeField] Transform _tutorialObj;

    private void Start()
    {
        _tutorialObj.gameObject.SetActive(false);
    }
    void Update()
    {
        if (RPGBattleManager.Instance.BattleState != BattleState.RPGBattle) { return; }
        if(Input.GetButtonDown("Fire1"))
        {
            _tutorialObj.gameObject.SetActive(true);
        }
        
        if(Input.GetButtonUp("Fire1"))
        {
            _tutorialObj.gameObject.SetActive(false);
        }
    }
}
