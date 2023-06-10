using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSetSample : MonoBehaviour
{
    [Header("Skillをセットするボタンのオブジェクト"),Tooltip("Skillをセットするボタンのオブジェクト"),SerializeField]
    GameObject[] _skillPanel;

    [Header("ボタンのPrehab"),Tooltip("ボタンのPrehab"),SerializeField]
    GameObject _skillbottom;

    [Header("取得しているスキルを並べる場所"),Tooltip("取得しているスキルを並べる場所"),SerializeField]
    GameObject _skillSetPoint;

    private DataBaseSample _dataBase;

    [Header("スキルの説明を書く場所"), Tooltip("スキルの説明を書く場所"), SerializeField]
    Text _tutorialText;

    [Header("現在何を選択しているかを書く場所"), Tooltip("現在何を選択しているかを書く場所"), SerializeField]
    Text _skillText;

    [Header("選択中のスキルのNo(スキルデータの要素数)"), Tooltip("選択中のスキルのNo(スキルデータの要素数)"), SerializeField]
    public int _tmp = -1;

    private void Awake()
    {
        _dataBase = DataBaseSample.Instance;
        for (var i = 0; i < _skillPanel.Length; i++)
        {
            var num = i;
            var button = _skillPanel[i].GetComponent<Button>();
            button.onClick.AddListener(() => MoveSkillChoice(num));
        }   //SkillをセットするボタンにonClickを設定する。
        if (_skillPanel.Length != 0)
        {
            for (var i = 0; i < _skillPanel.Length; i++)
            {
                var text = _skillPanel[i].GetComponentInChildren<Text>();
                text.text = _dataBase.SkillData[_dataBase._skillSetNo[i]].SkillName;
            }   //現在セットしているスキルの名前を書く。
        }
    }

    private void OnEnable()
    {
        foreach (Transform trans in _skillSetPoint.gameObject.transform)
        {
            Destroy(trans.gameObject);
        }   //並べたスキルをすべて削除

        //スキルを並べる処理
        SkillSet(_dataBase._skillbool, _dataBase.SkillData);
    }

    /// <summary>取得したスキルを並べるメソッド</summary>
    /// <param name="skillbool">取得しているスキルを確認するboolの配列</param>
    /// <param name="skillObjs">スキル一覧の情報</param>
    public void SkillSet(bool[] skillbool, SkillObjectSample[] skillObjs)
    {
        for (var i = 0; i < skillbool.Length; i++)
        {
            if (skillbool[i])
            {
                //ボタンを生成してボタンの変数を宣言
                var button = Instantiate(_skillbottom, transform.position, Quaternion.identity);
                if (button)
                {
                    //ボタンを_skillSetPoint(Scrollview/Viewport/Content)の直下に配置する。
                    button.transform.SetParent(_skillSetPoint.transform);
                    //ボタンのテキストにデータベースに登録されているスキルの名前を入力。
                    var text = button.GetComponentInChildren<Text>();
                    if (text) text.text = skillObjs[i].SkillName;
                    //ボタンにスキルの要素数を持っておく。
                    var click = button.GetComponent<Button>();
                    var num = i;
                    click.onClick.AddListener(() => SelectSkill(num));
                }
            }
        }
    }

    /// <summary>スキルを選択したときに呼ばれるメソッド</summary>
    /// <param name="i">選択したスキルのNo</param>
    public void SelectSkill(int i)
    {
        //スキルの情報の表示
        SkillDis(i, _dataBase.SkillData);
    }

    /// <summary>選択したスキルの情報のリセット</summary>
    public void SelectSkillReset()
    {
        _tutorialText.text = "";
        _skillText.text = "";
        _tmp = -1;
    }

    /// <summary>スキルの情報を表示するメソッド</summary>
    /// <param name="i">選択したスキルのNo</param>
    /// <param name="skillobj">スキル一覧の情報</param>
    void SkillDis(int i, SkillObjectSample[] skillobj)
    {
        _tmp = i;
        _tutorialText.text = skillobj[i].Description;
        _skillText.text = $"{skillobj[i].SkillName} を選択中";
    }

    /// <summary>セットするスキルのボタンを押した時の処理</summary>
    /// <param name="i">選択したスキルをセットする場所のNo</param>
    public void MoveSkillChoice(int i)
    {
        if (_tmp == -1) { return; }
        MoveSkill(i, _dataBase._skillSetNo, _dataBase.SkillData[_tmp]);
    }

    /// <summary>スキルをセットするメソッド</summary>
    /// <param name="i">選択したスキルをセットする場所のNo</param>
    /// <param name="SetNo">スキルをセットする場所の情報</param>
    /// <param name="_skill">セットするスキルの情報</param>
    public void MoveSkill(int i, int[] SetNo, SkillObjectSample _skill)
    {
        var text = _skillPanel[i].GetComponentInChildren<Text>();
        text.text = _skill.SkillName;
        SetNo[i] = _tmp;
    }
}
