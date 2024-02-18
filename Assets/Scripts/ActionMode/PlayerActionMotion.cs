using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionMotion : MonoBehaviour
{
    Rigidbody _rb;
    [Header("�����̍U���֘A")]
    [SerializeField] TextMesh _sideAttackText;
    [SerializeField] TextMesh _downAttackText;
    [SerializeField] float _sideAttackStopPoint = 100f;
    [SerializeField] float _sideAttackTime = 1f;
    [SerializeField] float _downAttackTime = 1f;
    [Header("�G�ւ̍U���֘A")]
    [SerializeField] Transform _spownPos;
    [SerializeField] Rigidbody _ragdoll;
    [SerializeField] float _expPower = 10;
    [SerializeField] float _expUpPower = 10;
    [Header("SkillPt�𐶐�����X�N���v�g")]
    [SerializeField] GetPtTextSpawn _textSpawnScripts;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>�v���C���[�̍U������</summary>
    /// <param name="skillName">�X�L���̖��O(�󔒋�؂�)</param>
    /// <returns></returns>
    public IEnumerator ActionTime(string[] skillName)
    {
        /// �T�C�h�A�^�b�N
        // �X�|�[������
        var sideAttack = Instantiate(_sideAttackText, _spownPos.position, transform.rotation);
        sideAttack.transform.SetParent(transform);
        sideAttack.text = skillName[skillName.Length - 1];
        // �ړ�����
        yield return sideAttack.transform.DOLocalMoveX(_sideAttackStopPoint, _sideAttackTime).WaitForCompletion();
        // �T�C�h�A�^�b�N�ɓ������G������Ă��鏈��
        var deathEnemy = sideAttack.GetComponent<SideAttackScripts>().ReturnEnemy();
        sideAttack.GetComponent<Collider>().enabled = false;
        Destroy(sideAttack);
        /// �G���U�����鏈��
        //�G������Ă���Ȃ������牽�����Ȃ��B
        if (deathEnemy.Count != 0)
        {
            var downAttackList = new List<PlayShader>();
            for (var i = 0; i < deathEnemy.Count; i++)
            {
                var pos = deathEnemy[i].transform.position;
                pos.y += 200;
                var downAttack = Instantiate(_downAttackText, pos, Quaternion.identity);
                //�X�L���̖��O�̐擪�𔽉f
                downAttack.text = skillName[0] + "!!";
                //��X�_�E���A�^�b�N���܂Ƃ߂�Destroy���邽�߂ɁA���X�g�ɒǉ����Ă����B
                downAttackList.Add(downAttack.GetComponent<PlayShader>());
                pos.y -= 180;
                downAttack.transform.DOMove(pos, _downAttackTime);
            }   // �G�̓���ɍU���������o��������B
            //�҂�
            yield return new WaitForSeconds(_downAttackTime);
            for (var i = 0; i < deathEnemy.Count; i++)
            {
                var pos = deathEnemy[i].transform.position;
                pos.y += 10;
                deathEnemy[i].ResetStun();
                deathEnemy[i].gameObject.SetActive(false);
                downAttackList[i].PlayParticle();
                var ragdollRbs = Instantiate(_ragdoll, pos, Quaternion.identity).GetComponents<Rigidbody>();
                pos.y -= 5f;
                foreach (var ragdoll in ragdollRbs)
                {
                    ragdoll.AddExplosionForce(_expPower, pos, 10,
                        _expUpPower, ForceMode.Impulse);
                }   //���O�h�[���̐�����΂�(����͂��܂�e���Ȃ�)
                // SkillPt�l����Text�̕\��
                _textSpawnScripts.GetPtText(deathEnemy[i].GetPoint(), transform.position);
            }   //�U���̕����œG��|��(�悤�Ɍ�����)�B
            yield return new WaitForSeconds(_sideAttackTime);
            foreach (var downAttack in downAttackList) { Destroy(downAttack.gameObject); }
        }
    }
}
