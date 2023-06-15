using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipController3D : MonoBehaviour
{
    Rigidbody _rb;
    [Tooltip("プレイヤーの動きの速さ")]
    [SerializeField]float _speed = 2f;
    [SerializeField] float _dashPower = 5f;
    [Tooltip("カメラの上下を動かすためのオブジェクト")]
    [SerializeField] Transform _eye;
    [Tooltip("カメラを左右に動かす")]
    [SerializeField] AxisState Horizontal;
    [Tooltip("カメラを上下に動かす")]
    [SerializeField] AxisState Vertical;
    [Tooltip("操作切り替えのbool型")]
    bool _airShipFly;
    [SerializeField] State _state = State.NomalMode;
    public bool _menu;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_menu)
        {
            float h = 0;
            float v = 0;
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
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

            dir.y = _rb.velocity.y;

            Vector3 dash = new Vector3(0, 0, 0);

            //飛行中、前に移動する処理。
            if (Input.GetButton("Fire2"))
            {
                dash += Vector3.forward * _dashPower;
                dash = Camera.main.transform.TransformDirection(dash);
                dash.y = 0;
            }

            //飛行機の動きの計算
            _rb.velocity = dir + dash;
            //カメラの動き
            Horizontal.Update(Time.deltaTime);
            Vertical.Update(Time.deltaTime);
            //カメラを動かす
            var horizontal = Quaternion.AngleAxis(Horizontal.Value, Vector3.up);
            transform.rotation = horizontal;
            var vertical = Quaternion.AngleAxis(Vertical.Value, Vector3.right);
            _eye.localRotation = vertical;
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            FightManager.Instance.InBattle(other.gameObject);
        }
    }

    enum State
    {
        FlyMode,
        DestroyMode,
        NomalMode,
    }
}
