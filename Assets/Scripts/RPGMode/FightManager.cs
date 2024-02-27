using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FightManager : SingletonMonovihair<FightManager>
{
    DataBase _database;
    [Header("テストプレイでもアタッチ必要")]
    [SerializeField] GameObject _battleFieldPrehab;
    private GameObject _battleField;
    [Header("テストプレイ時アタッチ不要")]
    [SerializeField] GameObject _player;
    [SerializeField] Text _winlosetext;
    [SerializeField] Text _pointGetText;
    [SerializeField] Canvas _tutorialCanvas;
    [SerializeField] Canvas _changeGanreCanvas;
    [SerializeField] bool _inFight;
    [SerializeField] bool _actionbool;
    [SerializeField] bool _isFalseCanvas;
    public bool InFight => _inFight;

    public static event UnityAction OnEnterAction;
    public static event UnityAction OnEnterRPG;
    public static event UnityAction OnEnterBattle;
    public static event UnityAction OnExitBattle;

    BattleState _battleState;
    public BattleState BattleState => _battleState;
    [Header("テストプレイ用")]
    [SerializeField] GameObject _testEnemyPrefab;

    protected override bool _dontDestroyOnLoad { get { return true; } }

    // Start is called before the first frame update
    void Start()
    {
        _database = DataBase.Instance;
        _winlosetext?.gameObject.SetActive(false);
        _pointGetText?.gameObject.SetActive(false);
    }

    /// <summary>Canvasの表示非表示を切り替えるメソッド</summary>
    public void CanvasDisplay()
    {
        _changeGanreCanvas.enabled = !_changeGanreCanvas.enabled;
        _tutorialCanvas.enabled = !_tutorialCanvas.enabled;
    }

    /// <summary>テストプレイ用バトルスタート</summary>
    public void TestBattlePlay()
    {
        StartCoroutine(InBattle(_testEnemyPrefab));
    }

    /// <summary>RPGのバトル開始時呼ばれるメソッド</summary>
    /// <param name="other">当たった敵</param>
    public IEnumerator InBattle(GameObject other)
    {
        if (_tutorialCanvas != null) { _tutorialCanvas.enabled = false; }
        if (_changeGanreCanvas != null) { _changeGanreCanvas.enabled = false; }
        _inFight = true;
        if (_player != null)
        {
            yield return _battleField =
                Instantiate
                (
                    _battleFieldPrehab, other.transform.position, _player.transform.localRotation
                );
        }
        else
        {
            yield return _battleField =
                Instantiate
                (
                    _battleFieldPrehab, other.transform.position, other.transform.localRotation
                );

        }
        other.gameObject.SetActive(false);
        if (_player != null) { _player.SetActive(false); }
        StartCoroutine(EndFightCoroutine(other));
        //OnEnterBattle.Invoke();
        if (!_actionbool)
        {
            _battleState = BattleState.RPGBattle;
        }
        else
        {
            _battleState = BattleState.ActionBattle;
            OnEnterAction.Invoke();
        }
    }

    /// <summary>敵を再出現させるメソッド</summary>
    /// <param name="enemy">非表示させた敵</param>
    IEnumerator EndFightCoroutine(GameObject enemy)
    {
        yield return new WaitUntil(() => _inFight == false);
        yield return new WaitForSeconds(30f);
        enemy?.gameObject.SetActive(true);
    }

    /// <summary>勝った時に呼ばれるメソッド</summary>
    /// <param name="getpoint"></param>
    public void Win(int getpoint)
    {
        _battleState = BattleState.BattleEnd;
        _database.GetSkillPoint(getpoint);
        _winlosetext?.gameObject.SetActive(true);
        if (_winlosetext != null) { _winlosetext.text = "Win!"; }
        _pointGetText?.gameObject.SetActive(true);
        if (_pointGetText != null) { _pointGetText.text = $"SkillPointを {getpoint} pt獲得した"; }
        StartCoroutine(EndBattle());
    }

    /// <summary>負けた時に呼ばれるメソッド</summary>
    public void Lose()
    {
        _battleState = BattleState.BattleEnd;
        _winlosetext?.gameObject.SetActive(true);
        if (_winlosetext != null) { _winlosetext.text = "Lose"; }
        StartCoroutine(EndBattle());
    }

    /// <summary>RPG導入時に操作するメソッド</summary>
    public void RPGEnter()
    {
        OnEnterRPG.Invoke();
        _battleState = BattleState.BattleStop;
    }

    /// <summary>RPGが始まった時に動作するメソッド</summary>
    public void StartRPG()
    {
        _battleState = BattleState.RPGBattle;
    }

    /// <summary>アクションモードが始まった時に動作するメソッド(今の所没)</summary>
    public void ActionEnter()
    {
        OnEnterAction.Invoke();
        _battleState = BattleState.ActionBattle;
    }

    /// <summary>バトルが終わった時に動作するメソッド</summary>
    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(5f);
        //OnExitBattle.Invoke();
        _winlosetext?.gameObject.SetActive(false);
        _pointGetText?.gameObject.SetActive(false);
        if (_tutorialCanvas != null) { _tutorialCanvas.enabled = true; }
        if (_changeGanreCanvas != null) { _changeGanreCanvas.enabled = true; }
        _inFight = false;
        if (_player != null) { _player.SetActive(true); }
        if (_battleField != null) { Destroy(_battleField); }
    }
}


public enum BattleState
{
    BattleStart,
    BattleStop,
    RPGBattle,
    ActionBattle,
    BattleEnd,
}
