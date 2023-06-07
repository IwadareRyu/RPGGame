using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeSetChangeScripts : MonoBehaviour
{
    [SerializeField] GameObject _treePanel;
    [SerializeField] GameObject _skillSetPanel;
    [SerializeField] Text _selectText;
    SkillSetScripts _skillSetScript;

    // Start is called before the first frame update
    void Start()
    {
        _skillSetPanel.SetActive(true);
        _treePanel.SetActive(false);
    }

    public void TreeChange()
    {
        _skillSetPanel.SetActive(false);
        _treePanel.SetActive(true);
        _selectText.text = "AttackMagic";
    }

    public void SkillSetChange()
    {
        _skillSetPanel.SetActive(true);
        _treePanel.SetActive(false);
        _skillSetScript = GameObject.FindAnyObjectByType<SkillSetScripts>();
        _selectText.text = _skillSetScript.SkillType.ToString();
        
    }
}
