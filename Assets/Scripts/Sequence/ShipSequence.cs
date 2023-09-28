using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipSequence : MonoBehaviour
{
    [SerializeField] Animator _puroperaAnim;
    [SerializeField] GameObject _explosion;
    // Start is called before the first frame update
    void Start()
    {
        var initialTrans = transform.position;
        var fallPoint = GameObject.FindGameObjectWithTag("FallPoint").GetComponent<Transform>();
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOMove(fallPoint.position, 0.2f))
            .AppendCallback(() =>
            {
                _explosion.SetActive(true);
                _puroperaAnim.Play("Puropera");
            })
            .AppendInterval(1f)
            .Append(transform.DOMove(initialTrans, 1f))
            .OnComplete(() => Destroy(this.gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if(!FightManager.Instance.InFight)
        {
            Destroy(this.gameObject);
        }
    }
}
