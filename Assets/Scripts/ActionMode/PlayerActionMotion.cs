using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionMotion : MonoBehaviour
{
    Rigidbody _rb;
    [Header("文字の攻撃関連")]
    [SerializeField] TextMesh _sideAttackText;
    [SerializeField] TextMesh _downAttackText;
    [SerializeField] float _sideAttackStopPoint = 100f;
    [SerializeField] float _sideAttackTime = 1f;
    [SerializeField] float _downAttackTime = 1f;
    [Header("敵への攻撃関連")]
    [SerializeField] Transform _spownPos;
    [SerializeField] Rigidbody _ragdoll;
    [SerializeField] float _expPower = 10;
    [SerializeField] float _expUpPower = 10;
    [Header("SkillPtを生成するスクリプト")]
    [SerializeField] GetPtTextSpawn _textSpawnScripts;

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
        /// サイドアタック
        // スポーン処理
        var sideAttack = Instantiate(_sideAttackText, _spownPos.position, transform.rotation);
        sideAttack.transform.SetParent(transform);
        sideAttack.text = skillName[skillName.Length - 1];
        //サイドアタックSE再生
        AudioManager.Instance.SEPlay(SE.ActionAttack);
        // 移動処理
        yield return sideAttack.transform.DOLocalMoveX(_sideAttackStopPoint, _sideAttackTime).WaitForCompletion();
        // サイドアタックに入った敵を取ってくる処理
        var deathEnemy = sideAttack.GetComponent<SideAttackScripts>().ReturnEnemy();
        sideAttack.GetComponent<Collider>().enabled = false;
        Destroy(sideAttack);
        /// 敵を攻撃する処理
        //敵を取ってこれなかったら何もしない。
        if (deathEnemy.Count != 0)
        {
            var downAttackList = new List<PlayShader>();
            for (var i = 0; i < deathEnemy.Count; i++)
            {
                var pos = deathEnemy[i].transform.position;
                pos.y += 200;
                var downAttack = Instantiate(_downAttackText, pos, Quaternion.identity);
                //スキルの名前の先頭を反映
                downAttack.text = skillName[0] + "!!";
                //後々ダウンアタックをまとめてDestroyするために、リストに追加しておく。
                downAttackList.Add(downAttack.GetComponent<PlayShader>());
                pos.y -= 180;
                downAttack.transform.DOMove(pos, _downAttackTime);
            }   // 敵の頭上に攻撃文字を出現させる。
            //待つ
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
                }   //ラグドールの吹っ飛ばし(今回はあまり影響なし)
                // SkillPt獲得とTextの表示
                _textSpawnScripts.GetPtText(deathEnemy[i].GetPoint(), transform.position);
            }   //攻撃の文字で敵を倒す(ように見せる)。
            //爆発SE再生
            AudioManager.Instance.SEPlay(SE.Explosion);
            yield return new WaitForSeconds(_sideAttackTime);
            foreach (var downAttack in downAttackList) { Destroy(downAttack.gameObject); }
        }
    }
}
