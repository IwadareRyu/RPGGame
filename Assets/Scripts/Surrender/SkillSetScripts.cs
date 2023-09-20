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

    public int _tmp = -1;

    private void Awake()
    {
        _dataBase = DataBase.Instance;
        for (var i = 0; i < _skillPanel.Length; i++)
        {
            var num = i;
            var button = _skillPanel[i].GetComponent<Button>();
            button.onClick.AddListener(() => MoveSkillChoice(num));
        }
    }

    private void OnEnable()
    {
        if (_skillPanel.Length != 0)
        {
            for (var i = 0; i < _skillPanel.Length; i++)
            {
                var text = _skillPanel[i].GetComponentInChildren<Text>();
                if (_skilltype == SkillType.BlockSkill)
                {
                    text.text = DataBase.BlockSkills[_dataBase._blockSkillSetNo[i]].SkillName;
                }
                else if (_skilltype == SkillType.AttackSkill)
                {
                    text.text = DataBase.AttackSkills[_dataBase._attackSkillSetNo[i]].SkillName;
                }
                else if (_skilltype == SkillType.AttackMagic)
                {
                    text.text = DataBase.AttackMagics[_dataBase._attackMagicSetNo[i]].SkillName;
                }
                else
                {
                    text.text = DataBase.BlockMagics[_dataBase._blockMagicSetNo[i]].SkillName;
                }
            }
        }
        foreach (Transform trans in _skillSetPoint.gameObject.transform)
        {
            Destroy(trans.gameObject);
        }
        if (_dataBase)
        {
            if (_skilltype == SkillType.BlockSkill)
            {
                SkillSet(_dataBase._blockSkillbool, DataBase.BlockSkills);
            }
            else if (_skilltype == SkillType.AttackSkill)
            {
                SkillSet(_dataBase._attackSkillbool, DataBase.AttackSkills);
            }
            else if (_skilltype == SkillType.AttackMagic)
            {
                SkillSet(_dataBase._attackMagicbool, DataBase.AttackMagics);
            }
            else
            {
                SkillSet(_dataBase._blockMagicbool, DataBase.BlockMagics);
            }
        }
    }

    public void SkillSet(bool[] skillbool, MasterData.Skill[] skillObjs)
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

    public void SelectSkill(int i)
    {
        if (_skilltype == SkillType.BlockSkill)
        {
            SkillDis(i, DataBase.BlockSkills);
        }
        else if (_skilltype == SkillType.AttackSkill)
        {
            SkillDis(i, DataBase.AttackSkills);
        }
        else if (_skilltype == SkillType.AttackMagic)
        {
            SkillDis(i, DataBase.AttackMagics);
        }
        else
        {
            SkillDis(i, DataBase.BlockMagics);
        }
    }

    public void SelectSkillReset()
    {
        _tutorialText.text = "";
        _skillText.text = "";
        _tmp = -1;
    }

    void SkillDis(int i, MasterData.Skill[] skillobj)
    {
        _tmp = i;
        _tutorialText.text = skillobj[i].Description;
        _skillText.text = $"{skillobj[i].SkillName} を選択中";
    }

    public void MoveSkillChoice(int i)
    {
        if (_tmp == -1) { return; }
        if (_skilltype == SkillType.BlockSkill)
        {
            MoveSkill(i, _dataBase._blockSkillSetNo, DataBase.BlockSkills[_tmp]);
        }
        else if (_skilltype == SkillType.AttackSkill)
        {
            MoveSkill(i, _dataBase._attackSkillSetNo, DataBase.AttackSkills[_tmp]);
        }
        else if (_skilltype == SkillType.AttackMagic)
        {
            MoveSkill(i, _dataBase._attackMagicSetNo, DataBase.AttackMagics[_tmp]);
        }
        else
        {
            MoveSkill(i, _dataBase._blockMagicSetNo, DataBase.BlockMagics[_tmp]);
        }
    }

    public void MoveSkill(int i,int[] SetNo,MasterData.Skill _skill)
    {
        var text = _skillPanel[i].GetComponentInChildren<Text>();
        text.text = _skill.SkillName;
        SetNo[i] = _tmp;
    }
}
