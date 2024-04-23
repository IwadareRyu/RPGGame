using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _skillMenu;
    [SerializeField]PlayerController _controller;
    [SerializeField] CinemachineFreeLook _cinemachine;
    // Start is called before the first frame update
    void Start()
    {
        _skillMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ///余裕があったらFightManagerのUnityActionでスクリプトのenableを切り替えても良いかも？
        if(Input.GetButtonDown("Menu") && !FightManager.Instance.InFight)
        {
            AudioManager.Instance.SEPlay(SE.Click);
            if (_skillMenu.activeSelf)
            {
                _skillMenu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _controller._menu = false;
                _cinemachine.enabled = true;
            }
            else
            {
                _skillMenu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _controller._menu = true;
                _cinemachine.enabled = false;
            }
        }
    }
}
