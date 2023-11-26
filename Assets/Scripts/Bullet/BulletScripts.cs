using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScripts : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _destroyTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = -transform.forward * _speed;
        Destroy(gameObject,_destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
