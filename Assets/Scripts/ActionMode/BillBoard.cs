using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Transform _cameraPos;
    void Update()
    {
        _cameraPos = Camera.main.transform;
        transform.LookAt(_cameraPos);
    }
}
