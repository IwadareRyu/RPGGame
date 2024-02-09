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
    void Start()
    {
        for (var i = 0; i < _ganreUI.Length; i++)
        {
            _ganreUI[i].changeImageTexts = _ganreUI[i].changeImage.GetComponentInChildren<Text>();
        }
        //Start���́AAction�ɕς�邱�Ƃ͉B���Ă����B
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

    /// <summary>UI���w�肳�ꂽ�W�������ɕς��鏈��</summary>
    /// <param name="ganre">�ς���W��������index</param>
    /// <returns></returns>
    public IEnumerator ChangeGenre(int ganre)
    {
        Debug.Log("�`�F�[���W�I");
        if (_changeArrow.color == _alphaColor || _ganreUI[(int)ChangeGanreState.RPG].changeImage.color == _alphaColor)
        {
            yield return StartCoroutine(InitialAction());
        }   //����Ăяo���ꂽ�ہAAction�������B
        if (_nowGanre.ganreText != _ganreUI[ganre].ganreText)
        {
            //��������]����܂őҋ@�B
            yield return _changeTrans.DORotate(_changeTrans.rotation.eulerAngles + new Vector3(0, 0, 180), 1f).WaitForCompletion();
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

    /// <summary>Action��UI��o�ꂳ���郁�\�b�h</summary>
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
    /// <summary>�W���������ς��ہA�e�����󂯂�ϐ��̍\����</summary>
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
