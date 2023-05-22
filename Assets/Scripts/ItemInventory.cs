using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    [SerializeField]
    GameObject[] _skillPanel;

    [SerializeField]
    GameObject _skillbottom;

    [SerializeField]
    GameObject _skillSetPoint;

    public static ItemInventory instance;

    private DataBase _dataBase;

    [SerializeField]
    SkillTypeEnum.SkillType _skilltype;

    [SerializeField]
    Text _tutorialText;

    [SerializeField]
    Text _skillText;

    public int tmp = -1;

    private void Awake()
    {
        _dataBase = GameObject.FindAnyObjectByType<DataBase>();

        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {

        for(var i = 0; i < _skillPanel.Length;i++)
        {
            var text = _skillPanel[i].GetComponentInChildren<Text>();
            if (_skilltype == SkillTypeEnum.SkillType.AttackMagic)
            {
                text.text = _dataBase.AttackMagicData[_dataBase._attackMagicSetNo[i]].SkillName;
            }
            if (_skilltype == SkillTypeEnum.SkillType.BlockMagic)
            {
                text.text = _dataBase.BlockMagicData[_dataBase._blockMagicSetNo[i]].SkillName;
            }
            if (_skilltype == SkillTypeEnum.SkillType.AttackSkill)
            {
                text.text = _dataBase.AttackSkillData[_dataBase._attackSkillSetNo[i]].SkillName;
            }
            if (_skilltype == SkillTypeEnum.SkillType.BlockSkill)
            {
                text.text = _dataBase.BlockSkillData[_dataBase._blockSkillSetNo[i]].SkillName;
            }
        }

        if (_dataBase)
        {
            if (_skilltype == SkillTypeEnum.SkillType.AttackMagic)
            {
                SkillSet(_dataBase._attackMagicbool, _dataBase.AttackMagicData);
            }
            else if (_skilltype == SkillTypeEnum.SkillType.BlockMagic)
            {
                SkillSet(_dataBase._blockMagicbool, _dataBase.BlockMagicData);
            }
            else if (_skilltype == SkillTypeEnum.SkillType.BlockSkill)
            {
                SkillSet(_dataBase._blockSkillbool, _dataBase.BlockSkillData);
            }
            else if (_skilltype == SkillTypeEnum.SkillType.AttackSkill)
            {
                SkillSet(_dataBase._attackSkillbool, _dataBase.AttackSkillData);
            }
        }
    }

    void SkillSet(bool[] skillbool,SkillObjects[] skillObjs)
    {
        for (var i = 0; i < skillbool.Length; i++)
        {
            if (skillbool[i])
            {
                //ボタンを生成してボタンの変数を宣言
                var bottom = Instantiate(_skillbottom, transform.position, Quaternion.identity);
                if (bottom)
                {
                    //ボタンを_skillSetPoint(Scrollview/Viewport/Content)の直下に配置する。
                    bottom.transform.SetParent(_skillSetPoint.transform);
                    //ボタンのテキストにデータベースに登録されているスキルの名前を入力。
                    var text = bottom.GetComponentInChildren<Text>();
                    if (text) text.text = skillObjs[i].SkillName;
                    //ボタンにスキルの要素数を持っておく。
                    var bottomScript = bottom.GetComponent<BottomScript>();
                    if (bottomScript) bottomScript._skillNo = i;
                }
            }
        }
    }

    public void SelectSkill(int i)
    {
        if (_skilltype == SkillTypeEnum.SkillType.AttackMagic)
        {
            SkillDis(i, _dataBase.AttackMagicData);
        }
        else if (_skilltype == SkillTypeEnum.SkillType.BlockMagic)
        {
            SkillDis(i, _dataBase.BlockMagicData);
        }
        else if (_skilltype == SkillTypeEnum.SkillType.BlockSkill)
        {
            SkillDis(i, _dataBase.BlockSkillData);
        }
        else if (_skilltype == SkillTypeEnum.SkillType.AttackSkill)
        {
            SkillDis(i, _dataBase.AttackSkillData);
        }
    }

    void SkillDis(int i,SkillObjects[] skillobj)
    {
        tmp = i;
        _tutorialText.text = skillobj[i]._description;
        _skillText.text = $"{skillobj[i].SkillName} を選択中";
    }

    public void MoveSkillChoice(int i)
    {
        if (_skilltype == SkillTypeEnum.SkillType.AttackMagic)
        {
            
        }
    }
}
