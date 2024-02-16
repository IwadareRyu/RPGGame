using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPtTextSpawn : MonoBehaviour
{
    [Header("TextMeshPro���q�ɂ���WorldCanvas")]
    [SerializeField] Canvas _ptTextCanvasPrefab;
    [Header("Instantiate�����ۂ̕��̍����̍�")]
    [SerializeField] float _insDifference = 1f;
    [Header("�e�L�X�g��y�����ւ̈ړ��̏I�_�Ǝn�_�̍�")]
    [SerializeField] float _moveUpDifference = 0.5f;
    [SerializeField] float _fadeTime = 3f;
    [SerializeField] float _destroyTime = 5f;
    [SerializeField] bool _targetCamera = true;

    /// <summary>�e�L�X�g�̐����A�\������</summary>
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
            //�|�C���g+-�Ԃł̐F�ς�����
            ColorChangeText(ptText, moneyCount);
            //�e�L�X�g����ɏ㏸�����鏈��
            TextMove(ptText, moneyCount, enemyTransform);
            //�e�L�X�g��������
            Destroy(textCanvas.gameObject, _destroyTime);
        }
    }

    /// <summary>�|�C���g+-�Ԃɉ������e�L�X�g�̐F�ς�����</summary>
    /// <param name="moneyText"></param>
    /// <param name="moneyCount"></param>
    void ColorChangeText(Text moneyText, int moneyCount)
    {
        moneyText.color = moneyCount >= 0 ? Color.yellow : Color.red;
    }

    /// <summary>�e�L�X�g�㏸����</summary>
    /// <param name="moneyText"></param>
    /// <param name="moneyCount"></param>
    /// <param name="custmerTransform"></param>
    void TextMove(Text moneyText, int moneyCount, Vector3 custmerTransform)
    {
        //+��-�ŕ\������e�L�X�g�̓��e��ς���O�����Z�q�B
        moneyText.text = moneyCount >= 0 ? $"+{moneyCount}SPt" : $"-{Mathf.Abs(moneyCount)}SPt";
        var fadeSeq = DOTween.Sequence();
        fadeSeq.Append(moneyText.DOFade(0f,_fadeTime))
            .Join(moneyText.transform.DOMoveY(custmerTransform.y + _moveUpDifference, _fadeTime));
        fadeSeq.Play().SetLink(moneyText.gameObject);
    }
}
