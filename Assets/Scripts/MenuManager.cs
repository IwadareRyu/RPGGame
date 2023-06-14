using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _skillMenu;
    AirShipController3D _controller;
    // Start is called before the first frame update
    void Start()
    {
        _skillMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<AirShipController3D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Menu"))
        {
            if (_skillMenu.active)
            {
                _skillMenu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _controller._menu = false;
            }
            else
            {
                _skillMenu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _controller._menu = true;
            }
        }
    }
}
