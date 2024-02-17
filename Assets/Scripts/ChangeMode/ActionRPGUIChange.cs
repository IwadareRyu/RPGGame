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
    [Header("真ん中に出てくる文字のアニメーション")]
    [SerializeField] Animator _centerTextAnim;
    [SerializeField] bool _initialRPG;
    [SerializeField] bool _initialAction;
    void Awake()
    {
        for (var i = 0; i < _ganreUI.Length; i++)
        {
            _ganreUI[i].changeImageTexts = _ganreUI[i].changeImage.GetComponentInChildren<Text>();
        }
        //デバッグ用にNORPG演出のない場合アニメーションは要らないのでfalseにする。
        if (_initialRPG) { _centerTextAnim.enabled = false; }
        //Start時は、Actionに変わることは隠しておく。
        _nowGanre = _ganreUI[(int)ChangeGanreState.RPG];
        _ganreUI[(int)ChangeGanreState.Action].changeImage.color = _alphaColor;
        _ganreUI[(int)ChangeGanreState.Action].changeImageTexts.color = _alphaColor;
        _changeArrow.color = _alphaColor;
        _ganreUI[(int)ChangeGanreState.Action].ganreText.gameObject.SetActive(false);
        _nowGanre = _ganreUI[(int)ChangeGanreState.RPG];
    }

    /// <summary>UIを指定されたジャンルに変える処理</summary>
    /// <param name="ganre">変えるジャンルのindex</param>
    /// <returns></returns>
    public IEnumerator ChangeGenre(int ganre)
    {
        Debug.Log("チェーンジ！");
        if (_nowGanre.ganreText != _ganreUI[ganre].ganreText)
        {
            if (_changeArrow.color == _alphaColor || _ganreUI[(int)ChangeGanreState.RPG].changeImage.color == _alphaColor)
            {
                _initialAction = true;
                yield return StartCoroutine(InitialAction());
                yield return _changeTrans.DORotate(_changeTrans.rotation.eulerAngles + new Vector3(0, 0, 180), 1f).WaitForCompletion();
            }   //初回呼び出された際、Actionが現れる。
            else if (ganre == 0 && !_initialRPG && _initialAction)
            {
                _initialRPG = true;
                _changeTrans.DORotate(_changeTrans.rotation.eulerAngles + new Vector3(0, 0, 180), 1f);
                yield return StartCoroutine(InitialRPG());
            }
            else
            {
                yield return StartCoroutine(ChangeTime(ganre));
            }
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

    /// <summary>ジャンル変更の際、呼ばれる処理</summary>
    /// <param name="ganre"></param>
    /// <returns></returns>
    IEnumerator ChangeTime(int ganre)
    {
        var ganreText = _ganreUI[ganre].centerGanreText;
        ganreText.color = _ganreUI[ganre].alphaTextColor;
        var tmpscale = ganreText.transform.localScale;
        var seq = DOTween.Sequence();
        seq.Append(_changeTrans.DORotate(_changeTrans.rotation.eulerAngles + new Vector3(0, 0, 180), _ganreChangeTime))
            .Join(ganreText.transform.DOScale(tmpscale * 1.3f, _ganreChangeTime))
            .Join(ganreText.DOFade(1f, _ganreChangeTime / 2))
            .Append(ganreText.DOFade(0f, _ganreChangeTime / 2))
            .OnComplete(() =>
            {
                ganreText.transform.localScale = tmpscale;
            });
        //sequenceが終わるまで待機。
        yield return seq.Play().SetLink(gameObject).WaitForCompletion();

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
        if (_centerTextAnim) { _centerTextAnim.Play("NOAction"); }
    }

    /// <summary>初めて(厳密には2回目)RPGモードに切り替わった際呼ばれるメソッド。</summary>
    /// <returns></returns>
    IEnumerator InitialRPG()
    {
        _centerTextAnim.Play("NORPG");
        yield return null;
        //NORPGのアニメーション時間待つ処理
        var animTime = _centerTextAnim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animTime.length);
        while (true)
        {
            if (Input.GetButtonDown("ActionDecition"))
            {
                break;
            }
            yield return null;
        }   //左クリックが押されるまで待機。
        _centerTextAnim.Play("NORPGEND");
        yield return null;
        //NORPGENDのアニメーション時間待つ処理
        animTime = _centerTextAnim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animTime.length);
        _centerTextAnim.enabled = false;
        _ganreUI[(int)ChangeGanreState.RPG].centerGanreText.transform.localPosition = Vector3.zero;
    }

    [Serializable]
    /// <summary>ジャンルが変わる際、影響を受ける変数の構造体</summary>
    struct GanreUI
    {
        public Image changeImage;
        [NonSerialized] public Text changeImageTexts;
        public RectTransform ganreText;
        public Text centerGanreText;
        public Color textColor;
        public Color alphaTextColor;
    }
}

public enum ChangeGanreState
{
    RPG = 0,
    Action = 1,
}
