using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlayer : MonoBehaviour
{
    [Tooltip("移動する場所")]
    [SerializeField]
    Transform[] _trans;
    [Tooltip("")]
    Magic _magicCondition;
    [Tooltip("移動の際止まる力")]
    float _stopdis = 0.1f;
    float _speed = 6f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DistanceMove(int i)
    {
        float distance = Vector2.Distance(transform.position, _trans[i].position);
        if (distance > _stopdis)
        {

            Vector3 dir = (_trans[i].position - transform.position).normalized * _speed;
            dir.y = 0;
            transform.Translate(dir * Time.deltaTime);
        }
        else
        {

        }
    }

    enum Magic
    {
        BlockMagic,
        AttackMagic,
        CoolTime,
    }

}
