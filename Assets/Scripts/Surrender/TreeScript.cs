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

    [Header("�X�L���c���[�̃{�^��(0���珇��)"),Tooltip("�X�L���c���[�̃{�^��"),SerializeField] 
    Button[] _attackMagicTreeButtom;

    [Tooltip("�אڃ��X�g")]
    List<int[]> _adjacentList = new List<int[]>();

    [Tooltip("�k�胊�X�g�̔z��")]
    List<int>[] _backList;

    [Tooltip("�o�H���X�g")]
    List<int> _ansList = new List<int>();

    [Header("�אڂ��钸�_�������Ă���txt�e�L�X�g"),Tooltip("�אڒ��_��txt"),SerializeField]
    TextAsset _textFile;

    [Header("�I�������X�L���̖��O��\������e�L�X�g"), Tooltip("�I�������X�L���̖��O��\������e�L�X�g"), SerializeField]
    Text _skillNameText;

    [Header("�I���̊m�F�̍ۃX�L���|�C���g�������e�L�X�g"), Tooltip("�I���̊m�F�̍ۃX�L���|�C���g�������e�L�X�g"), SerializeField]
    Text _skillPointText;

    [Header("�I���̊m�F�̍ۃX�L���̐����������e�L�X�g"), Tooltip("�I���̊m�F�̍ۃX�L���̐����������e�L�X�g"), SerializeField]
    Text _tutorialText;

    [Header("���̃X�L����I�����Ă��邩��\������e�L�X�g"), Tooltip("���̃X�L����I�����Ă��邩��\������e�L�X�g"), SerializeField]
    Text _skillText;

    [Header("�X�L���|�C���g��\������e�L�X�g"), Tooltip("�X�L���|�C���g��\������e�L�X�g"), SerializeField]
    Text _menuSkillPtText;

    [Header("�X�L���擾�̊m�F��ʂ�\������E�B���h�E"), Tooltip("�X�L���擾�̊m�F��ʂ�\������E�B���h�E"), SerializeField] 
    GameObject _confirmation;

    [Header("�X�L�����擾�ł��Ȃ��Ƃ��ɕ\������E�B���h�E"), Tooltip("�X�L�����擾�ł��Ȃ��Ƃ��ɕ\������E�B���h�E"), SerializeField] 
    GameObject _falseComfimation;

    [Header("�I���̊m�F���󂯓����{�^��"), Tooltip("�I���̊m�F���󂯓����{�^��"), SerializeField] 
    Button _yes;

    [Header("�I���̊m�F���󂯓���Ȃ��{�^��"), Tooltip("�I���̊m�F���󂯓���Ȃ��{�^��"), SerializeField] 
    Button _no;

    [Tooltip("�f�[�^�x�[�X")]
    private DataBase _database;

    [Tooltip("�擾����X�L���̍��v�R�X�g")]
    int _cost;

    [Tooltip("�o�H�����������ɁA�T���𔲂���bool")]
    bool _answerbool;

    [Tooltip("�K������X�L���̌�")]
    int _skillCount;

    [SerializeField] bool _tmpNameBool = true;

    [SerializeField] SkillSetScripts _skillSet;

    private void Awake()
    {
        _database = DataBase.Instance;
    }

    private void OnEnable()
    {
        //�X�L���|�C���g�̕\��(�Q�[�����ɒl���ς��\��������̂�OnEnable)
        _menuSkillPtText.text = _database.SkillPoint.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        //�k�胊�X�g�̔z��̐���錾����B(�X�L���c���[�̃{�^���̐��ƈꏏ)
        _backList = new List<int>[_attackMagicTreeButtom.Length];
        for(var i = 0;i < _attackMagicTreeButtom.Length;i++)
        {
            //�k�胊�X�g�̔z��̏��������ς܂��Ă����B
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
        }   //�{�^����onClick��ǉ����A�X�L���̖��O���S�����e�L�X�g�ɏ�������A���łɎ擾���Ă���X�L���̃{�^���������Ȃ��悤�ɂ���B

        //txt�t�@�C���̓��e��ǂݍ��ށB
        StringReader reader = new StringReader(_textFile.text);

        while(reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            _adjacentList.Add(Array.ConvertAll(line.Split(" "), int.Parse));
        }   //�t�@�C���̓��e����s�����X�g�ɒǉ��B

        //���D��T���ők�胊�X�g�����B
        InitialBFS();

        //yes��no��onClick��ǉ�������A�m�F��ʂ�false�ɂ���B
        _yes.onClick.AddListener(YesConfirmation);
        _no.onClick.AddListener(NoConfirmation);
        _confirmation.SetActive(false);
        _falseComfimation.SetActive(false);
        _skillSet.SkillSet(DataBase.Instance._attackMagicbool, DataBase.Instance.AttackMagicSelectData);
    }

    /// <summary>�X�L�����I�����ꂽ�Ƃ��ɌĂ΂�鏈���B</summary>
    /// <param name="end"></param>
    void DFSSkillTree(int end)
    {
        Debug.Log($"{end}���I������܂����B");
        _ansList.Add(0);
        //���X�g�ɍŏ��̒��_��ǉ�������A�[���D��T���̊J�n�B
        DFS(0, end);
        Debug.Log($"�o�H��{string.Join("��", _ansList)}�ł��B");

        foreach (var i in _ansList)
        {
            if (!_database._attackMagicbool[i])
            {
                if(DataBase.Instance.AttackMagicSelectData.SkillInfomation[i]._selectSkill is AttackMagicSelect attackMagic)
                _cost += attackMagic.SkillPoint;
            }
        }   //�f�[�^�̗v�f������X�L���f�[�^������Ă��āA�X�L���f�[�^�̃X�L���|�C���g�����v�R�X�g�ɉ��Z�B

        //�X�L���̐����A���̃X�L����I�����Ă��邩��\�����āA�m�F��ʂ��o���B
        _tutorialText.text = DataBase.Instance.AttackMagicSelectData.SkillInfomation[end]._description;
        _skillText.text = $"{DataBase.Instance.AttackMagicSelectData.SkillInfomation[end]._skillName} ��I��";
        _confirmation.SetActive(true);
        _skillNameText.text = DataBase.Instance.AttackMagicSelectData.SkillInfomation[_ansList[_ansList.Count - 1]]._skillName;
        _skillPointText.text = _cost.ToString();
    }

    /// <summary>�[���D��T��</summary>
    /// <param name="start">���݂̒��_</param>
    /// <param name="end">�ŏI�n�_</param>
    void DFS(int start, int end)
    {
        if (start == end)
        {
            return;
        }   //���łɌ��݂̒��_���ŏI�n�_�Ȃ�T���I���B
        else
        {
            //���݂̒��_�ɗאڂ��Ă��钸�_�����ԂɌ��Ă����B
            foreach (int i in _adjacentList[start])
            {
                //�אڒ��_�����X�g�Ɋ܂�ł��Ȃ���΃��X�g�ɒǉ��B
                if (!_ansList.Contains(i))
                {
                    _ansList.Add(i);
                    if (i == end)
                    {
                        _answerbool = true;
                        return;
                    }   //�אڒ��_���ŏI�n�_�̏ꍇ�A�T�����I��������B
                    else
                    {
                        DFS(i, end);
                        if (_answerbool) { return; }
                    }   //�����𖞂����Ă��Ȃ��ꍇ�A�אڒ��_�����݂̒��_�Ƃ��čċA����B
                }
            }
            _ansList.RemoveAt(_ansList.Count - 1);
        }   //���݂̒��_�̒T�����I������烊�X�g����폜����B
    }

    /// <summary>�������p���D��T��</summary>
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

    /// <summary>�X�L�����I�����ꂽ�Ƃ��ɌĂ΂�鏈��</summary>
    /// <param name="num"></param>
    void BFSSkillTree(int choiceNumber)
    {
        Debug.Log($"{choiceNumber}���I������܂����B");
        _ansList.Add(choiceNumber);
        //���X�g�ɑI�������ԍ���ǉ�������A���D��T���̊J�n�B
        BFS(choiceNumber);

        Debug.Log($"�K������X�L���� {string.Join(" ", _ansList)} �� {_ansList.Count} ��ł��B");

        foreach (var i in _ansList)
        {
            if(DataBase.Instance.AttackMagicSelectData.SkillInfomation[i]._selectSkill is AttackMagicSelect attackMagic)
            _cost += attackMagic.SkillPoint;
        }   //�f�[�^�̗v�f������X�L���f�[�^������Ă��āA�X�L���f�[�^�̃X�L���|�C���g�����v�R�X�g�ɉ��Z�B

        //�X�L���̐����A���̃X�L����I�����Ă��邩��\�����āA�m�F��ʂ��o���B
        _tutorialText.text = DataBase.Instance.AttackMagicSelectData.SkillInfomation[choiceNumber]._description;
        _skillText.text = $"{DataBase.Instance.AttackMagicSelectData.SkillInfomation[choiceNumber]._skillName} ��I��";
        _confirmation.SetActive(true);
        _skillNameText.text = $"{DataBase.Instance.AttackMagicSelectData.SkillInfomation[_ansList[0]]._skillName}\n���܂�{_ansList.Count}��̃X�L��";
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

    /// <summary>�m�F���󂯓��ꂽ���̏����B</summary>
    void YesConfirmation()
    {
        _confirmation.SetActive(false);
        if (_cost > _database.SkillPoint)
        {
            Debug.Log("�X�L���|�C���g������Ȃ��̂Ń|�C���g��������A�������I�����܂��B");
            _falseComfimation.SetActive(true);
        }   //�X�L���|�C���g������Ȃ������瑫��Ȃ��Ƃ��̃E�B���h�E���o���B
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
        }   //�X�L���|�C���g������Ȃ�A���v�R�X�g���A�|�C���g������āA�擾�����X�L���̃{�^���������Ȃ�����B
        AnswerReset();
    }

    public void ResetButton()
    {
        for (var i = 1; i < _attackMagicTreeButtom.Length; i++)
        {
            _attackMagicTreeButtom[i].interactable = true;
        }
    }



    /// <summary>�m�F���󂯓���Ȃ��������̏���</summary>
    void NoConfirmation()
    {
        _confirmation.SetActive(false);
        AnswerReset();
    }

    /// <summary>�T���������̏��������������鏈��</summary>
    void AnswerReset()
    {
        _answerbool = false;
        _ansList.Clear();
        _cost = 0;
    }
}
