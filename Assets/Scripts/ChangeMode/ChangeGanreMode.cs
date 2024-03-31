using Cinemachine;
using System.Collections;
using UnityEngine;

public class ChangeGanreMode : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _actionCinemachine;
    [SerializeField] ActionRPGUIChange _changeUI;
    [SerializeField] ChangeGanreState _changeGanreState = ChangeGanreState.RPG;
    [SerializeField] BulletSpawnEnemy[] enemys;
    [SerializeField] Rigidbody _rpgEnemy;

    private void Start()
    {
        foreach (var enemy in enemys)
        {
            enemy.Init();
        }   //Initでenemyの物理演算、弾幕を非アクティブにする。
        //RPGフロアの場合RPGのenemyの物理演算を非アクティブにする。
        if (_rpgEnemy != null) { _rpgEnemy.isKinematic = true; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (player.GanreState != _changeGanreState)
            {
                player.ChangeGanre(_changeGanreState);
                StartCoroutine(StartMode());
            }   // ジャンルを変える。
            else
            {
                ChangeMode();
            }
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            ChangeMode();
        }   //フロアの抜けた際、対象の敵の物理演算や弾幕を非アクティブにする。
    }

    /// <summary>ジャンルを変える際の処理</summary>
    /// <returns></returns>
    IEnumerator StartMode()
    {
        //ポーズ開始
        PauseManager.PauseResume();
        //UIを指定したジャンルにする
        yield return StartCoroutine(_changeUI.ChangeGenre((int)_changeGanreState));
        //ポーズ解除
        PauseManager.PauseResume();
        //物理演算、弾幕をアクティブにする。
        ChangeMode();
    }

    /// <summary>物理演算、弾幕のアクティブ、非アクティブを切り替えるメソッド</summary>
    void ChangeMode()
    {
        if (_changeGanreState == ChangeGanreState.Action)
        {
            AudioManager.Instance.BGMPlay(BGM.ActionPart);
            _actionCinemachine.enabled = true;
            foreach (var enemy in enemys)
            {
                enemy.ChangeAttackTime();
            }
        }   //Actionの場合、物理演算、弾幕をアクティブ、非アクティブにする。
        else
        {
            AudioManager.Instance.BGMPlay(BGM.RPGPart);
            _actionCinemachine.enabled = false;
            _rpgEnemy.isKinematic = !_rpgEnemy.isKinematic;
        }   //RPGの場合、物理演算をアクティブ、非アクティブにする。
    }
}
