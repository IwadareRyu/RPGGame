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
    [SerializeField] Text _tutorialText;
    [SerializeField] bool _inFight;
    [SerializeField] bool _actionbool;
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

    public void InBattle(GameObject other)
    {
        other.gameObject.SetActive(false);
        _tutorialText.enabled = false;
        _inFight = true;
        _battleField = Instantiate(_battleFieldPrehab, _player.transform.position, _player.transform.localRotation);
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
        _pointGetText.text = $"SkillPoint‚ð {getpoint} ptŠl“¾‚µ‚½";
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
        OnExitBattle.Invoke();
        _winlosetext?.gameObject.SetActive(false);
        _pointGetText?.gameObject.SetActive(false);
        _tutorialText.enabled = true;
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
