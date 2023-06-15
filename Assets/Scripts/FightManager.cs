using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : SingletonMonovihair<FightManager>
{
    DataBase _database;
    [SerializeField] GameObject _battleFieldPrehab;
    private GameObject _battleField;
    [SerializeField] GameObject _player;
    [SerializeField] Text _winlosetext;
    [SerializeField] Text _pointGetText;
    bool _endFight;
    protected override bool _dontDestroyOnLoad { get { return true; } }

    // Start is called before the first frame update
    void Start()
    {
        _database = DataBase.Instance;
        _winlosetext.gameObject.SetActive(false);
        _pointGetText.gameObject.SetActive(false);
    }

    public void InBattle(GameObject other)
    {
        other.gameObject.SetActive(false);
        _endFight = false;
        _battleField = Instantiate(_battleFieldPrehab, _player.transform.position, _player.transform.localRotation);
        _player.SetActive(false);
        StartCoroutine(EndFightCoroutine(other));
    }

    IEnumerator EndFightCoroutine(GameObject enemy)
    {
        yield return new WaitUntil(() => _endFight == true);
        yield return new WaitForSeconds(30f);
        enemy.gameObject.SetActive(true);
    }

    public void Win(int getpoint)
    {
        _database.GetSkillPoint(getpoint);
        _winlosetext.gameObject.SetActive(true);
        _winlosetext.text = "Win!";
        _pointGetText.gameObject.SetActive(true);
        _pointGetText.text = $"SkillPoint‚ð {getpoint} ptŠl“¾‚µ‚½";
        StartCoroutine(FalseText());
    }

    public void Lose()
    {
        _winlosetext.gameObject.SetActive(true);
        _winlosetext.text = "Lose";
        StartCoroutine(FalseText());
    }

    IEnumerator FalseText()
    {
        yield return new WaitForSeconds(5f);
        _winlosetext.gameObject.SetActive(false);
        _pointGetText.gameObject.SetActive(false);
        _endFight = true;
        _player.SetActive(true);
        Destroy(_battleField);

    }
}
