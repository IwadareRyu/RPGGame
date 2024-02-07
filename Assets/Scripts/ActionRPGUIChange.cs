using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionRPGUIChange : MonoBehaviour
{
    [Header("OperationCanvas�̒��ɂ���UI�̐ݒ�")]
    [SerializeField] Transform _changeTrans;
    [SerializeField] Image _changeArrow;
    [Header("�z��0��RPG�A1��Action")]
    [SerializeField] Image[] _changeImages;
    Text[] _changeImageTexts;
    [SerializeField] RectTransform[] _ganreText;
    [Header("�F�ݒ�")]
    [SerializeField] Color _activeColor;
    [SerializeField] Color _inactiveColor;
    [SerializeField] Color _alphaColor;
    Color _tmpTextColor;
    [Header("�F�ς���Ƃ��̕b���ݒ�")]
    [SerializeField] float _activeActionTime;
    [SerializeField] float _ganreChangeTime;
    [Tooltip("���̃W������(_changeImages�̃W�������̈������)")]
    Image _nowChangeImage;
    Text _nowChangeText;
    RectTransform _nowGanreUI;
    void Start()
    {
        _changeImageTexts = new Text[_changeImages.Length];
        for(var i = 0;i < _changeImages.Length;i++)
        {
            _changeImageTexts[i] = _changeImages[i].GetComponentInChildren<Text>();
        }
        //Start���́AAction�ɕς�邱�Ƃ͉B���Ă����B
        _nowChangeImage = _changeImages[(int)ChangeActionRPG.RPG];
        _nowChangeText = _changeImageTexts[(int)ChangeActionRPG.RPG];
        _changeImages[(int)ChangeActionRPG.Action].color = _alphaColor;
        _tmpTextColor = _changeImageTexts[(int)ChangeActionRPG.Action].color;
        _changeImageTexts[(int)ChangeActionRPG.Action].color = _alphaColor;
        _changeArrow.color = _alphaColor;
        _ganreText[(int)ChangeActionRPG.Action].gameObject.SetActive(false);
        _nowGanreUI = _ganreText[(int)ChangeActionRPG.RPG];
    }

    private void OnEnable()
    {
        
    }

    /// <summary>UI���w�肳�ꂽ�W�������ɕς��鏈��</summary>
    /// <param name="ganre">�W��������index</param>
    /// <returns></returns>
    public IEnumerator ChangeGenre(int ganre)
    {
        Debug.Log("�`�F�[���W�I");
        if(_changeArrow.color == _alphaColor || _changeImages[(int)ChangeActionRPG.RPG].color == _alphaColor)
        {
            _changeArrow.DOColor(_activeColor,_activeActionTime);
            _changeImages[(int)ChangeActionRPG.Action].DOColor(_activeColor, _activeActionTime);
            yield return _changeImageTexts[(int)ChangeActionRPG.Action].DOColor(_tmpTextColor, _activeActionTime).WaitForCompletion();
            yield return new WaitForSeconds(_activeActionTime);
        }   //����Ăяo���ꂽ�ہAAction�������B
        if(_nowChangeImage != _changeImages[ganre])
        {
            yield return _changeTrans.DORotate(_changeTrans.rotation.eulerAngles + new Vector3(0,0,180), 1f).WaitForCompletion();
            _changeImages[ganre].color = _activeColor;
            _changeImageTexts[ganre].color = _tmpTextColor;
            _ganreText[ganre].gameObject.SetActive(true);
            if (_nowChangeImage != null && _nowChangeText != null)
            {
                _nowChangeImage.color = _inactiveColor;
                _tmpTextColor = _nowChangeText.color;
                _nowChangeText.color = _inactiveColor;
                _nowGanreUI.gameObject.SetActive(false);
            }
            _nowChangeImage = _changeImages[ganre];
            _nowChangeText = _changeImageTexts[ganre];
            _nowGanreUI = _ganreText[ganre];
        }
        yield return null;
    }
}

public enum ChangeActionRPG
{
    RPG = 0,
    Action = 1,
}
