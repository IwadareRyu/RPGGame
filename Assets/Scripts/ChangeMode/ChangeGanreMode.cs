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
    [SerializeField] bool _initialStage;

    private void Start()
    {
        foreach (var enemy in enemys)
        {
            enemy.Init();
        }   //Init��enemy�̕������Z�A�e�����A�N�e�B�u�ɂ���B
        //RPG�t���A�̏ꍇRPG��enemy�̕������Z���A�N�e�B�u�ɂ���B
        if (_rpgEnemy != null) { _rpgEnemy.isKinematic = true; }
        //�����t���A�̏ꍇ�AChangeMode()�őΏۂ̓G��L��������B
        if (_initialStage) 
        {
            ChangeMode(); 
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
            }   // �W��������ς���B
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
        }   //�t���A�̔������ہA�Ώۂ̓G�̕������Z��e�����A�N�e�B�u�ɂ���B
    }

    /// <summary>�W��������ς���ۂ̏���</summary>
    /// <returns></returns>
    IEnumerator StartMode()
    {
        //�|�[�Y�J�n
        PauseManager.PauseResume();
        //UI���w�肵���W�������ɂ���
        yield return StartCoroutine(_changeUI.ChangeGenre((int)_changeGanreState));
        //�|�[�Y����
        PauseManager.PauseResume();
        //�������Z�A�e�����A�N�e�B�u�ɂ���B
        ChangeMode();
    }

    /// <summary>�������Z�A�e���̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ��郁�\�b�h</summary>
    void ChangeMode()
    {
        if (_changeGanreState == ChangeGanreState.Action)
        {
            _actionCinemachine.enabled = true;
            foreach (var enemy in enemys)
            {
                enemy.ChangeAttackTime();
            }
        }   //Action�̏ꍇ�A�������Z�A�e�����A�N�e�B�u�A��A�N�e�B�u�ɂ���B
        else
        {
            _actionCinemachine.enabled = false;
            _rpgEnemy.isKinematic = !_rpgEnemy.isKinematic;
        }   //RPG�̏ꍇ�A�������Z���A�N�e�B�u�A��A�N�e�B�u�ɂ���B
    }
}
