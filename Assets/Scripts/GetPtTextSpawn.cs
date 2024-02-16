using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPtTextSpawn : MonoBehaviour
{
    [Header("TextMeshProが子にいるWorldCanvas")]
    [SerializeField] Canvas _ptTextCanvasPrefab;
    [Header("Instantiateした際の物の高さの差")]
    [SerializeField] float _insDifference = 1f;
    [Header("テキストのy方向への移動の終点と始点の差")]
    [SerializeField] float _moveUpDifference = 0.5f;
    [SerializeField] float _fadeTime = 3f;
    [SerializeField] float _destroyTime = 5f;
    [SerializeField] bool _targetCamera = true;

    /// <summary>テキストの生成、表示処理</summary>
    /// <param name="moneyCount"></param>
    /// <param name="enemyTransform"></param>
    public void GetPtText(int moneyCount, Vector3 enemyTransform)
    {
        enemyTransform.y += _insDifference;
        enemyTransform.x += Random.Range(-_insDifference,_insDifference);
        enemyTransform.z += Random.Range(-_insDifference,_insDifference);
        if (_ptTextCanvasPrefab != null)
        {
            var textCanvas = Instantiate(_ptTextCanvasPrefab, enemyTransform, Quaternion.identity);
            var ptText = textCanvas.GetComponentInChildren<Text>();
            var targetPos = Camera.main.transform.rotation;
            if (_targetCamera)
            {
                textCanvas.transform.rotation = targetPos;
            }
            else
            {
                var rota = textCanvas.transform.rotation;
                rota.y += 180f;
                textCanvas.transform.rotation = rota;
            }
            //ポイント+-間での色変え処理
            ColorChangeText(ptText, moneyCount);
            //テキストを上に上昇させる処理
            TextMove(ptText, moneyCount, enemyTransform);
            //テキスト消す処理
            Destroy(textCanvas.gameObject, _destroyTime);
        }
    }

    /// <summary>ポイント+-間に応じたテキストの色変え処理</summary>
    /// <param name="moneyText"></param>
    /// <param name="moneyCount"></param>
    void ColorChangeText(Text moneyText, int moneyCount)
    {
        moneyText.color = moneyCount >= 0 ? Color.yellow : Color.red;
    }

    /// <summary>テキスト上昇処理</summary>
    /// <param name="moneyText"></param>
    /// <param name="moneyCount"></param>
    /// <param name="custmerTransform"></param>
    void TextMove(Text moneyText, int moneyCount, Vector3 custmerTransform)
    {
        //+か-で表示するテキストの内容を変える三項演算子。
        moneyText.text = moneyCount >= 0 ? $"+{moneyCount}SPt" : $"-{Mathf.Abs(moneyCount)}SPt";
        var fadeSeq = DOTween.Sequence();
        fadeSeq.Append(moneyText.DOFade(0f,_fadeTime))
            .Join(moneyText.transform.DOMoveY(custmerTransform.y + _moveUpDifference, _fadeTime));
        fadeSeq.Play().SetLink(moneyText.gameObject);
    }
}
