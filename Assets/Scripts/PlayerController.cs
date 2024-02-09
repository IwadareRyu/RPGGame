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
    [SerializeField]ChangeGanreState _ganreState = ChangeGanreState.RPG;
    public ChangeGanreState GanreState => _ganreState;
    Vector3 _respawnPos;
    public bool _menu;
    [SerializeField] Animator _robotAni;
    bool _waitMove = false;
    bool _pause = false;
    bool _leftAction = false;
    bool _rightAction = false;

    /// <summary>リスポーン地点の更新</summary>
    /// <param name="trans"></param>
    public void UpdateRespawnPos(Transform trans) => _respawnPos = trans.position;
    /// <summary>ジャンル変更時に呼ばれるメソッド</summary>
    /// <param name="state"></param>
    public void ChangeGanre(ChangeGanreState state) => _ganreState = state;

    private void Awake()
    {
        _attackRangeMesh.enabled = false;
        _rb = GetComponent<Rigidbody>();
        _ganreState = ChangeGanreState.RPG;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_menu && !_waitMove && !_pause)
        {
            //Playerの移動。
            PlayerMove();

            if (_ganreState == ChangeGanreState.Action)
            {
                ///アクション。
                Action();
            }
        }
        else
        {
            //ポーズ中y方向以外(重力の力以外)動かさない
            _rb.velocity = new Vector3(0,_rb.velocity.y,0);
        }
    }

    private void PlayerMove()
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
            dash += transform.forward.normalized * _dashPower;
            dash.y = 0;
        }

        //Playerの移動設定
        _rb.velocity = dir + dash;
        _robotAni.SetFloat("Speed", hAndV);
        ///
    }

    void Action()
    {
        if (!_leftAction && !_rightAction)
        {
            if (Input.GetButtonDown("ActionAttack1"))
            {
                _leftAction = true;
                _attackRangeMesh.enabled = true;
            }   //Qキーを押した時の処理
            if (Input.GetButtonDown("ActionAttack2"))
            {
                _rightAction = true;
                _attackRangeMesh.enabled = true;
            }   //Eキーを押した時の処理
        }
        else if (_attackRangeMesh.enabled)
        {
            if (Input.GetButtonUp("ActionDecition"))
            {
                _attackRangeMesh.enabled = false;
                _waitMove = true;
                //攻撃の実行
                StartCoroutine(ActionCoroutine(_leftAction ? 0 : 1));
            }   //アクションを選択した後、決定(左クリック)を押した時の処理
        }
        
    }

    /// <summary>Playerの攻撃が動いている間、待機するコルーチン。</summary>
    /// <param name="numver"></param>
    /// <returns></returns>
    IEnumerator ActionCoroutine(int numver)
    {
        _robotAni.Play("SideAttack");
        //AttackMagicにセットしているスキルの名前を取ってくる。
        var skillName = DataBase.Instance.AttackMagicSelectData.SkillInfomation[DataBase.Instance._attackMagicSetNo[numver]]._skillName.Split();
        //攻撃が動いている間、処理を待機する。
        yield return StartCoroutine(_playerAction.ActionTime(skillName));
        //攻撃が終わった後、操作できるようになる。
        _waitMove = false;
        _leftAction = false;
        _rightAction = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("接敵！");
            StartCoroutine(StandEncount(collision));
        }
    }

    IEnumerator StandEncount(Collision other)
    {
        yield return new WaitUntil(() => !_menu);
        Debug.Log("戦闘開始！");
        StartCoroutine(FightManager.Instance.InBattle(other.gameObject));
    }
}
