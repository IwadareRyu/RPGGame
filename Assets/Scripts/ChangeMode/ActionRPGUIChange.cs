using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionRPGUIChange : MonoBehaviour
{
    [Header("OperationCanvasの中にあるUIの設定")]
    [SerializeField] Transform _changeTrans;
    [SerializeField] Image _changeArrow;
    [Header("配列0がRPG、1がAction")]

    [SerializeField] GanreUI[] _ganreUI;
    [Header("色設定")]
    [SerializeField] Color _activeColor;
    [SerializeField] Color _inactiveColor;
    [SerializeField] Color _alphaColor;
    Color _tmpTextColor;
    [Header("色変えるときの秒数設定")]
    [SerializeField] float _activeActionTime;
    [SerializeField] float _ganreChangeTime;
    [Tooltip("今のジャンル")]
    GanreUI _nowGanre;
    void Start()
    {
        for (var i = 0; i < _ganreUI.Length; i++)
        {
            _ganreUI[i].changeImageTexts = _ganreUI[i].changeImage.GetComponentInChildren<Text>();
        }
        //Start時は、Actionに変わることは隠しておく。
        _nowGanre = _ganreUI[(int)ChangeGanreState.RPG];
        _ganreUI[(int)ChangeGanreState.Action].changeImage.color = _alphaColor;
        _ganreUI[(int)ChangeGanreState.Action].changeImageTexts.color = _alphaColor;
        _changeArrow.color = _alphaColor;
        _ganreUI[(int)ChangeGanreState.Action].ganreText.gameObject.SetActive(false);
        _nowGanre = _ganreUI[(int)ChangeGanreState.RPG];
    }

    private void OnEnable()
    {

    }

    /// <summary>UIを指定されたジャンルに変える処理</summary>
    /// <param name="ganre">変えるジャンルのindex</param>
    /// <returns></returns>
    public IEnumerator ChangeGenre(int ganre)
    {
        Debug.Log("チェーンジ！");
        if (_changeArrow.color == _alphaColor || _ganreUI[(int)ChangeGanreState.RPG].changeImage.color == _alphaColor)
        {
            yield return StartCoroutine(InitialAction());
        }   //初回呼び出された際、Actionが現れる。
        if (_nowGanre.ganreText != _ganreUI[ganre].ganreText)
        {
            //文字が回転するまで待機。
            yield return _changeTrans.DORotate(_changeTrans.rotation.eulerAngles + new Vector3(0, 0, 180), 1f).WaitForCompletion();
            //次のジャンルをアクティブ、明るくする。
            _ganreUI[ganre].changeImage.color = _activeColor;
            _ganreUI[ganre].changeImageTexts.color = _ganreUI[ganre].textColor;
            _ganreUI[ganre].ganreText.gameObject.SetActive(true);
            if (_nowGanre.ganreText != null)
            {
                _nowGanre.changeImage.color = _inactiveColor;
                _nowGanre.changeImageTexts.color = _inactiveColor;
                _nowGanre.ganreText.gameObject.SetActive(false);
            }   //現在のジャンルのUIを暗く、非アクティブにして、ジャンルを変える。
            //次のジャンルに切り替える。
            _nowGanre = _ganreUI[ganre];
        }
        yield return null;
    }

    /// <summary>ActionのUIを登場させるメソッド</summary>
    /// <returns></returns>
    IEnumerator InitialAction()
    {
        var actionIndex = (int)ChangeGanreState.Action;
        _changeArrow.DOColor(_activeColor, _activeActionTime);
        _ganreUI[actionIndex].changeImage.DOColor(_activeColor, _activeActionTime);
        yield return _ganreUI[actionIndex].changeImageTexts.DOColor(_ganreUI[actionIndex].textColor, _activeActionTime).WaitForCompletion();
        yield return new WaitForSeconds(_activeActionTime);
    }

    [Serializable]
    /// <summary>ジャンルが変わる際、影響を受ける変数の構造体</summary>
    struct GanreUI
    {
        public Image changeImage;
        [NonSerialized] public Text changeImageTexts;
        public RectTransform ganreText;
        public Color textColor;
    }
}

public enum ChangeGanreState
{
    RPG = 0,
    Action = 1,
}
