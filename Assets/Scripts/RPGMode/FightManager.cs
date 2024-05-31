using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RPGBattle;

public class FightManager : SingletonMonovihair<FightManager>
{
    DataBase _database;
    [Header("テストプレイでもアタッチ必要")]
    [SerializeField] GameObject _battleFieldPrehab;
    private GameObject _battleField;
    private RPGBattleManager _rpgBattleManager;
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
    [Header("テストプレイ用")]
    [SerializeField] EncountEnemy _testEnemyPrefab;

    protected override bool _dontDestroyOnLoad { get { return true; } }

    // Start is called before the first frame update
    void Start()
    {
        _database = DataBase.Instance;
        if (_winlosetext != null)_winlosetext.enabled = false;
        if(_pointGetText != null)_pointGetText.enabled = false;
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
    public IEnumerator InBattle(EncountEnemy other)
    {
        if (_tutorialCanvas != null) { _tutorialCanvas.enabled = false; }
        if (_changeGanreCanvas != null) { _changeGanreCanvas.enabled = false; }
        _inFight = true;
        if (_player != null)
        {
            yield return _battleField =
                Instantiate(_battleFieldPrehab, other.transform.position, _player.transform.localRotation);
        }
        else
        {
            yield return _battleField =
                Instantiate(_battleFieldPrehab, other.transform.position, other.transform.localRotation);
        }

        _rpgBattleManager = _battleField.GetComponent<RPGBattleManager>();
        other.gameObject.SetActive(false);
        if (_player != null) { _player.SetActive(false); }
        StartCoroutine(EndFightCoroutine(other.gameObject));
        AudioManager.Instance.BGMPlay(BGM.RPGBattle);
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
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.SEPlay(SE.Win);
        _database.GetSkillPoint(getpoint);
        
        if (_winlosetext != null) 
        {
            _winlosetext.enabled = true ; 
            _winlosetext.text = "Win!";
        }
        if (_pointGetText != null) 
        {
            _pointGetText.enabled = true;
            _pointGetText.text = $"SkillPointを {getpoint} pt獲得した"; 
        }
        StartCoroutine(EndBattle());
    }

    /// <summary>負けた時に呼ばれるメソッド</summary>
    public void Lose()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.SEPlay(SE.Lose);
        if (_winlosetext != null)
        {
            _winlosetext.enabled = true;
            _winlosetext.text = "Lose";
        }
        StartCoroutine(EndBattle());
    }

    /// <summary>バトルが終わった時に動作するメソッド</summary>
    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(5f);
        //OnExitBattle.Invoke();
        if (_winlosetext != null) _winlosetext.enabled = false;
        if (_pointGetText != null) _pointGetText.enabled = false;
        if (_tutorialCanvas != null) { _tutorialCanvas.enabled = true; }
        if (_changeGanreCanvas != null) { _changeGanreCanvas.enabled = true; }
        _inFight = false;
        if (_player != null) { _player.SetActive(true); }
        if (_battleField != null) { Destroy(_battleField); }
        AudioManager.Instance.BGMPlay(BGM.RPGPart);
    }
}
