using System;
using UnityEngine;

/// <summary>コマンドのクールタイム</summary>
[Serializable]
public class CommandAttacker : AttackPlayerCoolTimeClass
{
    [SerializeField] float _commandTime = 10f;
    float _commandCoolTime;
    bool _commandbool;
    [SerializeField] GameObject _commandObj;

    public void StartCoolTime(AttackPlayer player)
    {
        // コマンドの初期化
        if (_commandObj) _commandObj.SetActive(false);
        _commandbool = false;
        _commandCoolTime = 0;
        player.CommandCoolTimeViewAccess(_commandCoolTime, _commandTime);
    }

    public void UpdateCoolTime(AttackPlayer player)
    {
        if (!_commandbool)
        {
            //クールタイムの更新処理
            _commandCoolTime += Time.deltaTime;
            player.CommandCoolTimeViewAccess(_commandCoolTime, _commandTime);
        }

        //　時間になったらクールタイムの更新処理をやめ、コマンドの入力処理に入る。
        if (_commandCoolTime > _commandTime)
        {
            if (!_commandbool)
            {
                if (_commandObj) _commandObj.SetActive(true);
                _commandbool = true;
                player.ConditionTextViewAccess("コマンド？");
            }
            //コマンドの入力処理
            Command(player);
        }
    }

    /// <summary>選んだコマンドによってスキルを設定する処理</summary>
    void Command(AttackPlayer player)
    {
        if (Input.GetButtonDown("Attack"))
        {
            player.SetSkill(-1);
        }

        if (Input.GetButtonDown("Skill1"))
        {
            player.SetSkill(0);
        }

        if (Input.GetButtonDown("Skill2"))
        {
            player.SetSkill(1);
        }

        if (Input.GetButtonDown("Skill3"))
        {
            player.SetSkill(2);
        }
    }

    public void EndCoolTime(AttackPlayer player)
    {
        //コマンドの選択オブジェクトを非表示にする。
        //(ここではコマンドの初期化処理はしない)
        if (_commandObj) _commandObj.SetActive(false);
    }
}
