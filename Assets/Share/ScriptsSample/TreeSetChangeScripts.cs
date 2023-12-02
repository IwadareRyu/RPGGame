using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeSetChangeScripts : MonoBehaviour
{
    
    [SerializeField] 
    TreeScript _treePanel;
    [SerializeField] 
    GameObject _skillSetPanel;
    [SerializeField] 
    Text _selectText;
    SkillSetScripts _skillSetScript;
    [SerializeField]
    bool _setbool;
    [SerializeField]
    string _treeText = "スキルツリー";
    [SerializeField]
    string _setText = "スキルセット";

    // Start is called before the first frame update
    void Start()
    {
        _skillSetPanel.SetActive(true);
        _treePanel.gameObject.SetActive(false);
    }

    /// <summary>スキルセットメニューからスキルツリーメニューに変えるときの処理。</summary>
    public void TreeChange()
    {
        _skillSetPanel.SetActive(false);
        _treePanel.DisplaySkillPoint();
        _treePanel.gameObject.SetActive(true);
        _selectText.text = _treeText;
    }

    /// <summary>スキルツリーメニューからスキルセットメニューに変えるときの処理。</summary>
    public void SkillSetChange()
    {
        _skillSetPanel.SetActive(true);
        _treePanel.gameObject.SetActive(false);
        _skillSetScript = GameObject.FindAnyObjectByType<SkillSetScripts>();
        if (_setbool == true)
        {
            _selectText.text = _skillSetScript.SkillType.ToString();
        }
        else
        {
            _selectText.text = _setText;
        }
        
    }
}
