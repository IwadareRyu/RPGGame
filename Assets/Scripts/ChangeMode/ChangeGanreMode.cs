using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
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
            enemy.gameObject.SetActive(false);
        }   //Initでenemyの物理演算、弾幕を非アクティブにする。

        //RPGフロアの場合RPGのenemyの物理演算を非アクティブにする。
        if (_rpgEnemy != null) 
        {
            _rpgEnemy.gameObject.SetActive(false);
        }
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
                ChangeMode(true);
            }
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            ChangeMode(false);
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
        ChangeMode(true);
    }

    /// <summary>物理演算、弾幕のアクティブ、非アクティブを切り替えるメソッド</summary>
    void ChangeMode(bool isEnter)
    {
        if (_changeGanreState == ChangeGanreState.Action)
        {
            AudioManager.Instance.BGMPlay(BGM.ActionPart);
            _actionCinemachine.enabled = true;
            foreach (var enemy in enemys)
            {
                if (enemy.gameObject.activeSelf) { enemy.ChangeAttackTime(); }
                enemy.gameObject.SetActive(isEnter);
                if (enemy.gameObject.activeSelf) { enemy.ChangeAttackTime(); }
            }
        }   //Actionの場合、物理演算、弾幕をアクティブ、非アクティブにする。
        else
        {
            AudioManager.Instance.BGMPlay(BGM.RPGPart);
            _actionCinemachine.enabled = false;
            _rpgEnemy.gameObject.SetActive(isEnter);
        }   //RPGの場合、物理演算をアクティブ、非アクティブにする。
    }
}
