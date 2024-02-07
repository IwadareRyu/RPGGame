using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionRPGUIChange : MonoBehaviour
{
    [Header("OperationCanvasの中にあるUIの設定")]
    [SerializeField] Transform _changeTrans;
    [SerializeField] Image _changeArrow;
    [Header("配列0がRPG、1がAction")]
    [SerializeField] Image[] _changeImages;
    Text[] _changeImageTexts;
    [SerializeField] RectTransform[] _ganreText;
    [Header("色設定")]
    [SerializeField] Color _activeColor;
    [SerializeField] Color _inactiveColor;
    [SerializeField] Color _alphaColor;
    Color _tmpTextColor;
    [Header("色変えるときの秒数設定")]
    [SerializeField] float _activeActionTime;
    [SerializeField] float _ganreChangeTime;
    [Tooltip("今のジャンル(_changeImagesのジャンルの一つが入る)")]
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
        //Start時は、Actionに変わることは隠しておく。
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

    /// <summary>UIを指定されたジャンルに変える処理</summary>
    /// <param name="ganre">ジャンルのindex</param>
    /// <returns></returns>
    public IEnumerator ChangeGenre(int ganre)
    {
        Debug.Log("チェーンジ！");
        if(_changeArrow.color == _alphaColor || _changeImages[(int)ChangeActionRPG.RPG].color == _alphaColor)
        {
            _changeArrow.DOColor(_activeColor,_activeActionTime);
            _changeImages[(int)ChangeActionRPG.Action].DOColor(_activeColor, _activeActionTime);
            yield return _changeImageTexts[(int)ChangeActionRPG.Action].DOColor(_tmpTextColor, _activeActionTime).WaitForCompletion();
            yield return new WaitForSeconds(_activeActionTime);
        }   //初回呼び出された際、Actionが現れる。
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
