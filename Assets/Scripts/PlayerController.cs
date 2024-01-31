using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] PlayerActionMotion _playerAction;
    [SerializeField] MeshRenderer _attackRangeMesh;
    [Tooltip("プレイヤーの動きの速さ")]
    [SerializeField] float _speed = 2f;
    [SerializeField] float _dashPower = 5f;
    [SerializeField] float _gravityPower = 3f;
    [Tooltip("操作切り替えのbool型")]
    bool _airShipFly;
    [SerializeField] State _state = State.NomalMode;
    public bool _menu;
    [SerializeField] Animator _robotAni;
    bool _waitMove = false;
    bool _action1 = false;
    bool _action2 = false;

    private void Awake()
    {
        _attackRangeMesh.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_menu && !_waitMove)
        {
            ///移動
            float h = 0;
            float v = 0;
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            float hAndV = Mathf.Abs(h) + Mathf.Abs(v);
            Vector3 dir = new Vector3(0, 0, 0);

            //飛行中かそうでないかで移動方法を変える。(力の加え方を変える。)
            dir = Vector3.forward * v + Vector3.right * h;

            //カメラの座標を基準にdirを代入。
            dir = Camera.main.transform.TransformDirection(dir);

            //カメラが斜め下を向いているときy方向に動いてしまうので、y軸は0にする。
            dir.y = 0;

            //入力がない場合は回転させず、ある時はその方向にキャラを向ける。
            if (dir != Vector3.zero) transform.forward = dir;

            //水平方向の速度の計算。
            dir = dir.normalized * _speed;

            //垂直方向の設定(重力の設定)
            dir.y = _rb.velocity.y;

            Vector3 dash = new Vector3(0, 0, 0);

            //ダッシュ
            if (Input.GetButton("Fire2"))
            {
                dash += Vector3.forward * _dashPower;
                dash.y = 0;
            }

            //人の移動設定
            _rb.velocity = dir + dash;
            _robotAni.SetFloat("Speed", hAndV);
            ///

            ///アクション
            if (!_action1 && !_action2)
            {
                if (Input.GetButtonDown("ActionAttack1"))
                {
                    _action1 = true;
                    _attackRangeMesh.enabled = true;
                }
                if (Input.GetButtonDown("ActionAttack2"))
                {
                    _action2 = true;
                    _attackRangeMesh.enabled = true;
                }
            }
            else if (_attackRangeMesh.enabled)
            {
                if (Input.GetButtonUp("ActionDecition"))
                {
                    _attackRangeMesh.enabled = false;
                    _waitMove = true;
                    StartCoroutine(Action());
                }
            }
            ///
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }

    IEnumerator Action()
    {
        _robotAni.Play("SideAttack");
        yield return StartCoroutine(_playerAction.ActionTime());
        _waitMove = false;
        _action1 = false;
        _action2 = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("接敵！");
            StartCoroutine(StandEncount(other));
        }
    }

    IEnumerator StandEncount(Collider other)
    {
        yield return new WaitUntil(() => !_menu);
        Debug.Log("戦闘開始！");
        StartCoroutine(FightManager.Instance.InBattle(other.gameObject));
    }

    enum State
    {
        FlyMode,
        DestroyMode,
        NomalMode,
    }
}
