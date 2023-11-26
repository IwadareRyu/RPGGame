using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutScripts : MonoBehaviour
{
    [SerializeField] Animator _getOutAnim;
    [SerializeField] Animator _avatorGetInAnim;
    [SerializeField] GameObject _explosion;
    [SerializeField] float _impulse = 10;
    [SerializeField] float _dis = 0.1f;
    [SerializeField] float _speed = 3.0f;
    Vector3 _vec;
    Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Instantiate(_explosion,transform.position,Quaternion.identity);
        _getOutAnim.Play("GetOut");
        _rb.AddForce((transform.up - transform.forward) * _impulse,ForceMode.Impulse);
    }

    public IEnumerator GetIn(Transform getInPos,Transform blockPos)
    {
        _getOutAnim.enabled = false;
        _avatorGetInAnim.Play("GetIn");
        var tmp = getInPos.position;
        tmp.y -= 1;
        transform.position = tmp;
        transform.rotation = getInPos.rotation;
        _vec = (blockPos.position - transform.position).normalized;
        _rb.velocity = _vec * _speed;
        while (Vector3.Distance(transform.position,blockPos.position) > _dis)
        {
            yield return null;
        }
        _rb.velocity = Vector3.zero;

    }
}
