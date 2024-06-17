using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSetScripts : MonoBehaviour
{
    [SerializeField]
    GameObject[] _skillPanel;

    [SerializeField]
    GameObject _skillbottom;

    [SerializeField]
    GameObject _skillSetPoint;

    private DataBase _dataBase;

    [SerializeField]
    SkillType _skilltype;

    public SkillType SkillType => _skilltype;

    [SerializeField]
    Text _tutorialText;

    [SerializeField]
    Text _skillText;

    [SerializeField]
    Text _coolTimeText;

    public int _tmp = -1;

    private void Awake()
    {
        for (var i = 0; i < _skillPanel.Length; i++)
        {
            var num = i;
            var button = _skillPanel[i].GetComponent<Button>();
            button.onClick.AddListener(() => MoveSkillChoice(num));
        }
    }

    private void Start()
    {
        _dataBase = DataBase.Instance;
        if (_skillPanel.Length != 0)
        {
            for (var i = 0; i < _skillPanel.Length; i++)
            {
                var text = _skillPanel[i].GetComponentInChildren<Text>();
                if (_skilltype == SkillType.BlockSkill)
                {
                    text.text = DataBase.Instance.BlockSkillSelectData.SkillInfomation[DataBase.Instance._blockSkillSetNo[i]]._skillName;
                }
                else if (_skilltype == SkillType.AttackSkill)
                {
                    text.text = DataBase.Instance.AttackSkillSelectData.SkillInfomation[DataBase.Instance._attackSkillSetNo[i]]._skillName;
                }
                else if (_skilltype == SkillType.AttackMagic)
                {
                    text.text = DataBase.Instance.AttackMagicSelectData.SkillInfomation[DataBase.Instance._attackMagicSetNo[i]]._skillName;
                }
                else
                {
                    text.text = DataBase.Instance.BlockMagicSelectData.SkillInfomation[DataBase.Instance._blockMagicSetNo[i]]._skillName;
                }
            }
            if (_dataBase)
            {
                if (_skilltype == SkillType.BlockSkill)
                {
                    SkillSet(_dataBase._blockSkillbool, DataBase.Instance.BlockSkillSelectData);
                }
                else if (_skilltype == SkillType.AttackSkill)
                {
                    SkillSet(_dataBase._attackSkillbool, DataBase.Instance.AttackSkillSelectData);
                }
                else if (_skilltype == SkillType.AttackMagic)
                {
                    SkillSet(_dataBase._attackMagicbool, DataBase.Instance.AttackMagicSelectData);
                }
                else
                {
                    SkillSet(_dataBase._blockMagicbool, DataBase.Instance.BlockMagicSelectData);
                }
            }
        }
    }
    public void SkillSet(bool[] skillbool, SelectorSkillObjects skillObjs)
    {
        if (_skillSetPoint.gameObject.transform.childCount != 0)
        {
            foreach (Transform trans in _skillSetPoint.gameObject.transform)
            {
                Destroy(trans.gameObject);
            }
        }
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
                    if (text) text.text = skillObjs.SkillInfomation[i]._skillName;
                    //ボタンにスキルの要素数を持っておく。
                    var click = button.GetComponent<Button>();
                    var num = i;
                    click.onClick.AddListener(() => SelectSkill(num));
                }
            }
        }
    }

    public void SelectSkill(int i)
    {
        _tmp = i;
        if (_skilltype == SkillType.BlockSkill)
        {
            SkillDis(DataBase.Instance.BlockSkillSelectData.SkillInfomation[i]);
        }
        else if (_skilltype == SkillType.AttackSkill)
        {
            SkillDis(DataBase.Instance.AttackSkillSelectData.SkillInfomation[i]);
        }
        else if (_skilltype == SkillType.AttackMagic)
        {
            SkillDis(DataBase.Instance.AttackMagicSelectData.SkillInfomation[i]);
        }
        else
        {
            SkillDis(DataBase.Instance.BlockMagicSelectData.SkillInfomation[i]);
        }
    }

    public void SelectSkillReset()
    {
        _tutorialText.text = "";
        _skillText.text = "";
        _coolTimeText.text = "";
        _tmp = -1;
    }

    void SkillDis(SkillInfomation skillobj)
    {
        AudioManager.Instance.SEPlay(SE.Click);
        _tutorialText.text = skillobj._description;
        _skillText.text = $"{skillobj._skillName} を選択中";
        _coolTimeText.text = skillobj._chastingTime.ToString("0.0");
    }

    public void MoveSkillChoice(int i)
    {
        if (_tmp == -1) { return; }
        
        if (_skilltype == SkillType.BlockSkill)
        {
            MoveSkill(i, _dataBase._blockSkillSetNo, DataBase.Instance.BlockSkillSelectData.SkillInfomation);
        }
        else if (_skilltype == SkillType.AttackSkill)
        {
            MoveSkill(i, _dataBase._attackSkillSetNo, DataBase.Instance.AttackSkillSelectData.SkillInfomation);
        }
        else if (_skilltype == SkillType.AttackMagic)
        {
            MoveSkill(i, _dataBase._attackMagicSetNo, DataBase.Instance.AttackMagicSelectData.SkillInfomation);
        }
        else
        {
            MoveSkill(i, _dataBase._blockMagicSetNo, DataBase.Instance.BlockMagicSelectData.SkillInfomation);
        }
    }

    public void MoveSkill(int i,int[] SetNo,SkillInfomation[] _skill)
    {
        var text = _skillPanel[i].GetComponentInChildren<Text>();
        text.text = _skill[_tmp]._skillName;
        SetNo[i] = _tmp;
        AudioManager.Instance.SEPlay(SE.Equip);
    }
}
