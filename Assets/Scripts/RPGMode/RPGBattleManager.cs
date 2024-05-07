using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPGBattle
{
    public class RPGBattleManager : MonoBehaviour
    {
        [SerializeField] Image _blackPanel;
        [SerializeField] Image _ruleImage;
        [SerializeField] Color _fadeColor = Color.white;
        Color _defaultColor = Color.white;
        [SerializeField] float _fadeTime = 1f;

        /// <summary>一斉制御するときに使うかもしれないUnityEvent</summary>
        public static event UnityAction OnEnterAction;
        public static event UnityAction OnEnterRPG;
        public static event UnityAction OnStartBattle;
        public static event UnityAction OnEndBattle;

        BattleState _battleState;
        public BattleState BattleState => _battleState;

        private static RPGBattleManager instance;
        public static RPGBattleManager Instance => instance;

        void Awake()
            => instance = this;

        void Start()
            => StartCoroutine(StartBattle());

        IEnumerator StartBattle()
        {
            BattleEnter();
            _blackPanel.enabled = true;
            _defaultColor = _ruleImage.color;
            _ruleImage.color = _fadeColor;
            FadeChildText(0, 1);
            yield return _ruleImage.DOColor(_defaultColor, _fadeTime).SetLink(_ruleImage.gameObject).WaitForCompletion();
            yield return new WaitUntil(() => Input.anyKeyDown);
            FadeChildText(1, 0);
            yield return _ruleImage.DOColor(_fadeColor, _fadeTime).SetLink(_ruleImage.gameObject).WaitForCompletion();
            _blackPanel.enabled = false;
            StartRPG();
        }

        void FadeChildText(float start, float end)
        {
            foreach (Transform child in _ruleImage.transform)
            {
                if (child.TryGetComponent<Text>(out var text))
                {
                    var textColor = text.color;
                    textColor.a = start;
                    text.color = textColor;
                    text.DOFade(end, _fadeTime);
                }
            }
        }

        /// <summary>BattleStart時に行うメソッド</summary>
        public void BattleEnter()
        {

            _battleState = BattleState.BattleStart;
        }

        public void BattleStop()
        {
            _battleState = BattleState.BattleStop;
        }

        public void BattleEnd()
        {
            _battleState = BattleState.BattleEnd;
        }


        /// <summary>RPGが始まった時に動作するメソッド</summary>
        public void StartRPG()
        {
            _battleState = BattleState.RPGBattle;
        }
    }

    public enum BattleState
    {
        BattleStart,
        BattleStop,
        RPGBattle,
        BattleEnd,
    }
}
