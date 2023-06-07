using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TreeScript : MonoBehaviour
{
    [SerializeField] Button[] _attackMagicTreeButtom;
    List<int[]> _adjacentList = new List<int[]>();
    List<int> _ansList = new List<int>();
    [SerializeField] TextAsset _textFile;
    [SerializeField] Text _skillNameText;
    [SerializeField] Text _skillPointText;
    [SerializeField] Text _tutorialText;
    [SerializeField] Text _skillText;
    [SerializeField] GameObject _confirmation;
    [SerializeField] Button _yes;
    [SerializeField] Button _no;

    int _cost;
    bool _answerbool;

    // Start is called before the first frame update
    void Awake()
    {
        for(var i = 0;i < _attackMagicTreeButtom.Length;i++)
        {
            var num = i;
            _attackMagicTreeButtom[i].onClick.AddListener(() => DFSSkillTree(num));
            var text = _attackMagicTreeButtom[i].GetComponentInChildren<Text>();
            var tutorialtext = DataBase.Instance.AttackMagicData[i].SkillName;
            text.text = tutorialtext.Substring(0,4);
            if(DataBase.Instance._attackMagicbool[i])
            {
                _attackMagicTreeButtom[i].interactable = false;
            }
        }
        StringReader reader = new StringReader(_textFile.text);

        while(reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            _adjacentList.Add(Array.ConvertAll(line.Split(" "), int.Parse));
        }

        _confirmation.SetActive(false);
        _yes.onClick.AddListener(YesConfirmation);
        _no.onClick.AddListener(NoConfirmation);
    }

    void DFSSkillTree(int end)
    {
        Debug.Log($"{end}が選択されました。");
        _ansList.Add(0);
        DFS(0,end);
        Debug.Log($"経路は{string.Join("→", _ansList)}です。");

        foreach(var i in _ansList)
        {
            if (!DataBase.Instance._attackMagicbool[i])
            {
                _cost += DataBase.Instance.AttackMagicData[i].SkillPoint;
            }
        }
        _tutorialText.text = DataBase.Instance.AttackMagicData[end]._description;
        _skillText.text = $"{DataBase.Instance.AttackMagicData[end].SkillName} を選択中";
        _confirmation.SetActive(true);
        _skillNameText.text = DataBase.Instance.AttackMagicData[_ansList[_ansList.Count - 1]].SkillName;
        _skillPointText.text = _cost.ToString();
    }

    void DFS(int start,int end)
    {
        if(start == end)
        {
            return;
        }
        else
        {
            foreach(int i in _adjacentList[start])
            {
                if(!_ansList.Contains(i))
                {
                    _ansList.Add(i);
                    if (i == end)
                    {
                        _answerbool = true;
                        return;
                    }
                    else
                    {
                        DFS(i, end);
                        if (_answerbool) { return; }
                    }
                }
            }
            _ansList.RemoveAt(_ansList.Count - 1);
        }
    }

    void YesConfirmation()
    {
        _confirmation.SetActive(false);
        //ここにスキルコストを減らす処理を書く。//



        //////////////////////////////////////////
        foreach(var i in _ansList)
        {
            DataBase.Instance._attackMagicbool[i] = true;
            _attackMagicTreeButtom[i].interactable = false;
        }
        AnswerReset();
    }

    void NoConfirmation()
    {
        _confirmation.SetActive(false);
        AnswerReset();
    }

    void AnswerReset()
    {
        _answerbool = false;
        _ansList.Clear();
        _cost = 0;
    }

    public void SelectSkillReset()
    {
        _tutorialText.text = "";
        _skillText.text = "";
    }
}
