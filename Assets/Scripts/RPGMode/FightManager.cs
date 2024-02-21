using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FightManager : SingletonMonovihair<FightManager>
{
    DataBase _database;
    [SerializeField] GameObject _battleFieldPrehab;
    private GameObject _battleField;
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

    protected override bool _dontDestroyOnLoad { get { return true; } }

    // Start is called before the first frame update
    void Start()
    {
        _database = DataBase.Instance;
        _winlosetext?.gameObject.SetActive(false);
        _pointGetText?.gameObject.SetActive(false);
    }

    public void CanvasDisplay()
    {
        _changeGanreCanvas.enabled = !_changeGanreCanvas.enabled;
        _tutorialCanvas.enabled = !_tutorialCanvas.enabled;
    }

    public IEnumerator InBattle(GameObject other)
    {
        _tutorialCanvas.enabled = false;
        _changeGanreCanvas.enabled = false;
        _inFight = true;
        yield return _battleField = Instantiate(_battleFieldPrehab, other.transform.position, _player.transform.localRotation);
        other.gameObject.SetActive(false);
        _player.SetActive(false);
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

    IEnumerator EndFightCoroutine(GameObject enemy)
    {
        yield return new WaitUntil(() => _inFight == false);
        yield return new WaitForSeconds(30f);
        enemy?.gameObject.SetActive(true);
    }

    public void Win(int getpoint)
    {
        _database.GetSkillPoint(getpoint);
        _winlosetext?.gameObject.SetActive(true);
        _winlosetext.text = "Win!";
        _pointGetText?.gameObject.SetActive(true);
        _pointGetText.text = $"SkillPointを {getpoint} pt獲得した";
        StartCoroutine(FalseText());
    }

    public void Lose()
    {
        _winlosetext?.gameObject.SetActive(true);
        _winlosetext.text = "Lose";
        StartCoroutine(FalseText());
    }

    public void RPGEnter()
    {
        OnEnterRPG.Invoke();
        _battleState = BattleState.BattleStop;
    }

    public void StartRPG()
    {
        _battleState = BattleState.RPGBattle;
    }

    public void ActionEnter()
    {
        OnEnterAction.Invoke();
        _battleState = BattleState.ActionBattle;
    }

    IEnumerator FalseText()
    {
        yield return new WaitForSeconds(5f);
        _battleState = BattleState.BattleEnd;
        //OnExitBattle.Invoke();
        _winlosetext?.gameObject.SetActive(false);
        _pointGetText?.gameObject.SetActive(false);
        _tutorialCanvas.enabled = true;
        _changeGanreCanvas.enabled = true;
        _inFight = false;
        _player.SetActive(true);
        Destroy(_battleField);

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
