using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionMotion : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] TextMesh _sideAttackText;
    [SerializeField] TextMesh _downAttackText;
    [SerializeField] Transform _spownPos;
    [SerializeField] Rigidbody _ragdoll;
    [SerializeField] float _sideAttackTime = 1f;
    [SerializeField] float _downAttackTime = 1f;
    [SerializeField] float _expPower = 10;
    [SerializeField] float _expUpPower = 10;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>プレイヤーの攻撃処理</summary>
    /// <param name="skillName">スキルの名前(空白区切り)</param>
    /// <returns></returns>
    public IEnumerator ActionTime(string[] skillName)
    {
        var sideAttack = Instantiate(_sideAttackText, _spownPos.position, transform.rotation);
        sideAttack.transform.SetParent(transform);
        sideAttack.text = skillName[skillName.Length - 1];

        yield return sideAttack.transform.DOLocalMoveX(100, _sideAttackTime).WaitForCompletion();
        var deathEnemy = sideAttack.GetComponent<SideAttackScripts>().ReturnEnemy();
        if (deathEnemy.Count != 0)
        {
            var downAttackList = new List<PlayShader>();
            for (var i = 0; i < deathEnemy.Count; i++)
            {
                var pos = deathEnemy[i].transform.position;
                pos.y += 200;
                var downAttack = Instantiate(_downAttackText, pos, Quaternion.identity);
                downAttack.text = skillName[0] + "!!";
                downAttackList.Add(downAttack.GetComponent<PlayShader>());
                pos.y -= 180;
                downAttack.transform.DOMove(pos, _downAttackTime);
            }
            Destroy(sideAttack);
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
                }
            }
            yield return new WaitForSeconds(_sideAttackTime);
            foreach (var downAttack in downAttackList) { Destroy(downAttack.gameObject); }
        }
    }
}
