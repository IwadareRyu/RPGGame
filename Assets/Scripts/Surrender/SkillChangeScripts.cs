using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChangeScripts : MonoBehaviour
{
    [SerializeField] GameObject[] _skillChangeBottom;
    [SerializeField] GameObject[] _skillChangeUI;
    [SerializeField] Text _selectText;
    int _setNo;
    [SerializeField] int _setChange = 0;

    private void Start()
    {
        for(var i = 0;i < _skillChangeUI.Length;i++)
        {
            if(i != _setChange)
            {
                _skillChangeUI[i].SetActive(false);
            }
            else
            {
                _skillChangeBottom[i].SetActive(false);
                _setNo = i;
                _selectText.text = ((SkillType)i).ToString();
            }
        }
    }

    public void ChangeSkill(int i)
    {
        _skillChangeBottom[_setNo].SetActive(true);
        _skillChangeUI[_setNo].SetActive(false);
        _skillChangeUI[i].SetActive(true);
        _skillChangeBottom[i].SetActive(false);
        _selectText.text = ((SkillType)i).ToString();
        GameObject.FindObjectOfType<SkillSetScripts>().SelectSkillReset();
        _setNo = i;
    }

    public void ChangeMamu()
    {

    }
}
