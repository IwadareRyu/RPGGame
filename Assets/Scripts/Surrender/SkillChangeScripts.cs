using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChangeScripts : MonoBehaviour
{
    [SerializeField] GameObject[] _skillChangeBottom;
    [SerializeField] Canvas[] _skillChangeCanvas;
    [SerializeField] SkillSetScripts[] _skillSetScripts;
    [SerializeField] Text _selectText;
    int _setNo;
    [SerializeField] int _setChange = 0;

    private void Start()
    {
        for(var i = 0;i < _skillChangeCanvas.Length;i++)
        {
            if(i != _setChange)
            {
               _skillChangeCanvas[i].enabled = false;
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
        _skillChangeCanvas[_setNo].enabled = false;
        _skillChangeCanvas[i].enabled = true;
        _skillChangeBottom[i].SetActive(false);
        _selectText.text = ((SkillType)i).ToString();
        _skillSetScripts[i].SelectSkillReset();
        _setNo = i;
    }

    public void ChangeMamu()
    {

    }
}
