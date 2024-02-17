using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionRPGUIChange : MonoBehaviour
{
    [Header("OperationCanvas�̒��ɂ���UI�̐ݒ�")]
    [SerializeField] Transform _changeTrans;
    [SerializeField] Image _changeArrow;
    [Header("�z��0��RPG�A1��Action")]

    [SerializeField] GanreUI[] _ganreUI;
    [Header("�F�ݒ�")]
    [SerializeField] Color _activeColor;
    [SerializeField] Color _inactiveColor;
    [SerializeField] Color _alphaColor;
    Color _tmpTextColor;
    [Header("�F�ς���Ƃ��̕b���ݒ�")]
    [SerializeField] float _activeActionTime;
    [SerializeField] float _ganreChangeTime;
    [Tooltip("���̃W������")]
    GanreUI _nowGanre;
    [Header("�^�񒆂ɏo�Ă��镶���̃A�j���[�V����")]
    [SerializeField] Animator _centerTextAnim;
    [SerializeField] bool _initialRPG;
    [SerializeField] bool _initialAction;
    void Awake()
    {
        for (var i = 0; i < _ganreUI.Length; i++)
        {
            _ganreUI[i].changeImageTexts = _ganreUI[i].changeImage.GetComponentInChildren<Text>();
        }
        //�f�o�b�O�p��NORPG���o�̂Ȃ��ꍇ�A�j���[�V�����͗v��Ȃ��̂�false�ɂ���B
        if (_initialRPG) { _centerTextAnim.enabled = false; }
        //Start���́AAction�ɕς�邱�Ƃ͉B���Ă����B
        _nowGanre = _ganreUI[(int)ChangeGanreState.RPG];
        _ganreUI[(int)ChangeGanreState.Action].changeImage.color = _alphaColor;
        _ganreUI[(int)ChangeGanreState.Action].changeImageTexts.color = _alphaColor;
        _changeArrow.color = _alphaColor;
        _ganreUI[(int)ChangeGanreState.Action].ganreText.gameObject.SetActive(false);
        _nowGanre = _ganreUI[(int)ChangeGanreState.RPG];
    }

    /// <summary>UI���w�肳�ꂽ�W�������ɕς��鏈��</summary>
    /// <param name="ganre">�ς���W��������index</param>
    /// <returns></returns>
    public IEnumerator ChangeGenre(int ganre)
    {
        Debug.Log("�`�F�[���W�I");
        if (_nowGanre.ganreText != _ganreUI[ganre].ganreText)
        {
            if (_changeArrow.color == _alphaColor || _ganreUI[(int)ChangeGanreState.RPG].changeImage.color == _alphaColor)
            {
                _initialAction = true;
                yield return StartCoroutine(InitialAction());
                yield return _changeTrans.DORotate(_changeTrans.rotation.eulerAngles + new Vector3(0, 0, 180), 1f).WaitForCompletion();
            }   //����Ăяo���ꂽ�ہAAction�������B
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
            //���̃W���������A�N�e�B�u�A���邭����B
            _ganreUI[ganre].changeImage.color = _activeColor;
            _ganreUI[ganre].changeImageTexts.color = _ganreUI[ganre].textColor;
            _ganreUI[ganre].ganreText.gameObject.SetActive(true);
            if (_nowGanre.ganreText != null)
            {
                _nowGanre.changeImage.color = _inactiveColor;
                _nowGanre.changeImageTexts.color = _inactiveColor;
                _nowGanre.ganreText.gameObject.SetActive(false);
            }   //���݂̃W��������UI���Â��A��A�N�e�B�u�ɂ��āA�W��������ς���B
            //���̃W�������ɐ؂�ւ���B
            _nowGanre = _ganreUI[ganre];
        }
        yield return null;
    }

    /// <summary>�W�������ύX�̍ہA�Ă΂�鏈��</summary>
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
        //sequence���I���܂őҋ@�B
        yield return seq.Play().SetLink(gameObject).WaitForCompletion();

    }

    /// <summary>Action��UI��o�ꂳ���郁�\�b�h</summary>
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

    /// <summary>���߂�(�����ɂ�2���)RPG���[�h�ɐ؂�ւ�����یĂ΂�郁�\�b�h�B</summary>
    /// <returns></returns>
    IEnumerator InitialRPG()
    {
        _centerTextAnim.Play("NORPG");
        yield return null;
        //NORPG�̃A�j���[�V�������ԑ҂���
        var animTime = _centerTextAnim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animTime.length);
        while (true)
        {
            if (Input.GetButtonDown("ActionDecition"))
            {
                break;
            }
            yield return null;
        }   //���N���b�N���������܂őҋ@�B
        _centerTextAnim.Play("NORPGEND");
        yield return null;
        //NORPGEND�̃A�j���[�V�������ԑ҂���
        animTime = _centerTextAnim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animTime.length);
        _centerTextAnim.enabled = false;
        _ganreUI[(int)ChangeGanreState.RPG].centerGanreText.transform.localPosition = Vector3.zero;
    }

    [Serializable]
    /// <summary>�W���������ς��ہA�e�����󂯂�ϐ��̍\����</summary>
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
