using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageAttackState : MonoBehaviour,IRPGState
{
    SkillInfomation _chageSkill;

    public void Init(BlockPlayerController player)
    {
        var set = player._dataBase.BlockSkillSelectData.SkillInfomation[player._dataBase._blockSkillSetNo[1]];
        _chageSkill = set;
    }

    public void StartState(BlockPlayerController player)
    {
        player.ShowText("�`���[�W�A�^�b�N�J�n");
        StartCoroutine(ChangeAttackTime(player));
    }

    public void UpdateState(BlockPlayerController player)
    {
        return;
    }

    public void EndState(BlockPlayerController player)
    {
        player.ShowText("�`���[�W�A�^�b�N�I��");
    }

    /// <summary>�`���[�W�A�^�b�N�̃A�j���[�V��������</summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator ChangeAttackTime(BlockPlayerController player)
    {
        //�`���[�W�A�^�b�N��������A�Q�[�W���O�ɂ��āAAttack��Ԃɖ߂�B
        Debug.Log(_chageSkill._skillName);
        player.ShowText(_chageSkill._skillName);
        ///Animation�v���C

        ///
        //�A�j���[�V�������������̍ہA�A�j���[�V�����̕b������
        float animationTime = 0f;
        //�`���[�W�A�^�b�N�A�j���[�V�������A���Ԃ��~�߂�Ȃ�܂��`����ς���B
        yield return new WaitForSeconds(animationTime);

        ChageAttack(player);
        player.OnChangeState(player.AttackState);
        yield return null;
    }

    /// <summary>�`���[�W�A�^�b�N</summary>
    /// <param name="player"></param>
    void ChageAttack(BlockPlayerController player)
    {
        if (_chageSkill._selectSkill is BlockSkillSelect blockSkill)
        {
            player._enemy.AddDebuffDamage(player.Attack, blockSkill.AttackValue, blockSkill.EnemyOffencePower, blockSkill.EnemyDiffencePower);
        }
    }

}
