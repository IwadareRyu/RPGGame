using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TreeScript : MonoBehaviour
{

    [Header("スキルツリーのボタン(0から順に)"),Tooltip("スキルツリーのボタン"),SerializeField] 
    Button[] _attackMagicTreeButtom;

    [Tooltip("隣接リスト")]
    List<int[]> _adjacentList = new List<int[]>();

    [Tooltip("遡りリストの配列")]
    List<int>[] _backList;

    [Tooltip("経路リスト")]
    List<int> _ansList = new List<int>();

    [Header("隣接する頂点が書いてあるtxtテキスト"),Tooltip("隣接頂点のtxt"),SerializeField]
    TextAsset _textFile;

    [Header("選択したスキルの名前を表示するテキスト"), Tooltip("選択したスキルの名前を表示するテキスト"), SerializeField]
    Text _skillNameText;

    [Header("選択の確認の際スキルポイントを書くテキスト"), Tooltip("選択の確認の際スキルポイントを書くテキスト"), SerializeField]
    Text _skillPointText;

    [Header("選択の確認の際スキルの説明を書くテキスト"), Tooltip("選択の確認の際スキルの説明を書くテキスト"), SerializeField]
    Text _tutorialText;

    [Header("何のスキルを選択しているかを表示するテキスト"), Tooltip("何のスキルを選択しているかを表示するテキスト"), SerializeField]
    Text _skillText;

    [Header("スキルポイントを表示するテキスト"), Tooltip("スキルポイントを表示するテキスト"), SerializeField]
    Text _menuSkillPtText;

    [Header("スキル取得の確認画面を表示するウィンドウ"), Tooltip("スキル取得の確認画面を表示するウィンドウ"), SerializeField] 
    GameObject _confirmation;

    [Header("スキルが取得できないときに表示するウィンドウ"), Tooltip("スキルが取得できないときに表示するウィンドウ"), SerializeField] 
    GameObject _falseComfimation;

    [Header("選択の確認を受け入れるボタン"), Tooltip("選択の確認を受け入れるボタン"), SerializeField] 
    Button _yes;

    [Header("選択の確認を受け入れないボタン"), Tooltip("選択の確認を受け入れないボタン"), SerializeField] 
    Button _no;

    [Tooltip("データベース")]
    private DataBase _database;

    [Tooltip("取得するスキルの合計コスト")]
    int _cost;

    [Tooltip("経路を見つけた時に、探索を抜けるbool")]
    bool _answerbool;

    [Tooltip("習得するスキルの個数")]
    int _skillCount;

    [SerializeField] bool _tmpNameBool = true;

    [SerializeField] SkillSetScripts _skillSet;

    private void Awake()
    {
        _database = DataBase.Instance;
    }

    private void OnEnable()
    {
        //スキルポイントの表示(ゲーム中に値が変わる可能性があるのでOnEnable)
        _menuSkillPtText.text = _database.SkillPoint.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        //遡りリストの配列の数を宣言する。(スキルツリーのボタンの数と一緒)
        _backList = new List<int>[_attackMagicTreeButtom.Length];
        for(var i = 0;i < _attackMagicTreeButtom.Length;i++)
        {
            //遡りリストの配列の初期化を済ませておく。
            _backList[i] = new List<int>();
            var num = i;
            _attackMagicTreeButtom[i].onClick.AddListener(() => BFSSkillTree(num));
            var text = _attackMagicTreeButtom[i].GetComponentInChildren<Text>();
            var skillName = DataBase.Instance.AttackMagicSelectData.SkillInfomation[i]._skillName;
            text.text = skillName.Substring(0,4);
            if(_database._attackMagicbool[i])
            {
                _attackMagicTreeButtom[i].interactable = false;
            }
        }   //ボタンにonClickを追加し、スキルの名前を４文字テキストに書いた後、すでに取得しているスキルのボタンをさわれないようにする。

        //txtファイルの内容を読み込む。
        StringReader reader = new StringReader(_textFile.text);

        while(reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            _adjacentList.Add(Array.ConvertAll(line.Split(" "), int.Parse));
        }   //ファイルの内容を一行ずつリストに追加。

        //幅優先探索で遡りリストを作る。
        InitialBFS();

        //yesとnoにonClickを追加した後、確認画面をfalseにする。
        _yes.onClick.AddListener(YesConfirmation);
        _no.onClick.AddListener(NoConfirmation);
        _confirmation.SetActive(false);
        _falseComfimation.SetActive(false);
        _skillSet.SkillSet(DataBase.Instance._attackMagicbool, DataBase.Instance.AttackMagicSelectData);
    }

    /// <summary>スキルが選択されたときに呼ばれる処理。</summary>
    /// <param name="end"></param>
    void DFSSkillTree(int end)
    {
        Debug.Log($"{end}が選択されました。");
        _ansList.Add(0);
        //リストに最初の頂点を追加した後、深さ優先探索の開始。
        DFS(0, end);
        Debug.Log($"経路は{string.Join("→", _ansList)}です。");

        foreach (var i in _ansList)
        {
            if (!_database._attackMagicbool[i])
            {
                if(DataBase.Instance.AttackMagicSelectData.SkillInfomation[i]._selectSkill is AttackMagicSelect attackMagic)
                _cost += attackMagic.SkillPoint;
            }
        }   //データの要素数からスキルデータを取ってきて、スキルデータのスキルポイントを合計コストに加算。

        //スキルの説明、何のスキルを選択しているかを表示して、確認画面を出す。
        _tutorialText.text = DataBase.Instance.AttackMagicSelectData.SkillInfomation[end]._description;
        _skillText.text = $"{DataBase.Instance.AttackMagicSelectData.SkillInfomation[end]._skillName} を選択中";
        _confirmation.SetActive(true);
        _skillNameText.text = DataBase.Instance.AttackMagicSelectData.SkillInfomation[_ansList[_ansList.Count - 1]]._skillName;
        _skillPointText.text = _cost.ToString();
    }

    /// <summary>深さ優先探索</summary>
    /// <param name="start">現在の頂点</param>
    /// <param name="end">最終地点</param>
    void DFS(int start, int end)
    {
        if (start == end)
        {
            return;
        }   //すでに現在の頂点が最終地点なら探索終了。
        else
        {
            //現在の頂点に隣接している頂点を順番に見ていく。
            foreach (int i in _adjacentList[start])
            {
                //隣接頂点がリストに含んでいなければリストに追加。
                if (!_ansList.Contains(i))
                {
                    _ansList.Add(i);
                    if (i == end)
                    {
                        _answerbool = true;
                        return;
                    }   //隣接頂点が最終地点の場合、探索を終了させる。
                    else
                    {
                        DFS(i, end);
                        if (_answerbool) { return; }
                    }   //条件を満たしていない場合、隣接頂点を現在の頂点として再帰する。
                }
            }
            _ansList.RemoveAt(_ansList.Count - 1);
        }   //現在の頂点の探索が終わったらリストから削除する。
    }

    /// <summary>初期化用幅優先探索</summary>
    void InitialBFS()
    {
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(0);

        while (queue.Count > 0)
        {
            var currentSkillNumber = queue.Dequeue();
            foreach (var adjacentSkillNumber in _adjacentList[currentSkillNumber])
            {
                if (adjacentSkillNumber != -1)
                {
                    if (!_backList[adjacentSkillNumber].Contains(currentSkillNumber))
                    {
                        _backList[adjacentSkillNumber].Add(currentSkillNumber);
                        queue.Enqueue(adjacentSkillNumber);
                    }
                }
            }
        }
    }

    /// <summary>スキルが選択されたときに呼ばれる処理</summary>
    /// <param name="num"></param>
    void BFSSkillTree(int choiceNumber)
    {
        Debug.Log($"{choiceNumber}が選択されました。");
        _ansList.Add(choiceNumber);
        //リストに選択した番号を追加した後、幅優先探索の開始。
        BFS(choiceNumber);

        Debug.Log($"習得するスキルは {string.Join(" ", _ansList)} の {_ansList.Count} 種です。");

        foreach (var i in _ansList)
        {
            if(DataBase.Instance.AttackMagicSelectData.SkillInfomation[i]._selectSkill is AttackMagicSelect attackMagic)
            _cost += attackMagic.SkillPoint;
        }   //データの要素数からスキルデータを取ってきて、スキルデータのスキルポイントを合計コストに加算。

        //スキルの説明、何のスキルを選択しているかを表示して、確認画面を出す。
        _tutorialText.text = DataBase.Instance.AttackMagicSelectData.SkillInfomation[choiceNumber]._description;
        _skillText.text = $"{DataBase.Instance.AttackMagicSelectData.SkillInfomation[choiceNumber]._skillName} を選択中";
        _confirmation.SetActive(true);
        _skillNameText.text = $"{DataBase.Instance.AttackMagicSelectData.SkillInfomation[_ansList[0]]._skillName}\nを含む{_ansList.Count}種のスキル";
        _skillPointText.text = _cost.ToString();
    }

    void BFS(int choiceNumber)
    {
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(choiceNumber);

        while (queue.Count > 0)
        {
            var currentSkillNumber = queue.Dequeue();
            foreach (var backSkillNumber in _backList[currentSkillNumber])
            {
                if (!_ansList.Contains(backSkillNumber) && !_database._attackMagicbool[backSkillNumber])
                {
                    _ansList.Add(backSkillNumber);
                    queue.Enqueue(backSkillNumber);
                }
            }
        }
    }

    /// <summary>確認を受け入れた時の処理。</summary>
    void YesConfirmation()
    {
        _confirmation.SetActive(false);
        if (_cost > _database.SkillPoint)
        {
            Debug.Log("スキルポイントが足りないのでポイントを消費せず、処理を終了します。");
            _falseComfimation.SetActive(true);
        }   //スキルポイントが足りなかったら足りないときのウィンドウを出す。
        else
        {
            _database.GetSkillPoint(-_cost);
            _menuSkillPtText.text = _database.SkillPoint.ToString();
            foreach (var i in _ansList)
            {
                _database._attackMagicbool[i] = true;
                _attackMagicTreeButtom[i].interactable = false;
            }
            _skillSet.SkillSet(DataBase.Instance._attackMagicbool, DataBase.Instance.AttackMagicSelectData);
        }   //スキルポイントがあるなら、合計コスト分、ポイントを消費して、取得したスキルのボタンを押せなくする。
        AnswerReset();
    }

    public void ResetButton()
    {
        for (var i = 1; i < _attackMagicTreeButtom.Length; i++)
        {
            _attackMagicTreeButtom[i].interactable = true;
        }
    }



    /// <summary>確認を受け入れなかった時の処理</summary>
    void NoConfirmation()
    {
        _confirmation.SetActive(false);
        AnswerReset();
    }

    /// <summary>探索した時の処理を初期化する処理</summary>
    void AnswerReset()
    {
        _answerbool = false;
        _ansList.Clear();
        _cost = 0;
    }
}
