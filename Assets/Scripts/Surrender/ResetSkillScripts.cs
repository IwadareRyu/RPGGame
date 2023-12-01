using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ResetSkillScripts : MonoBehaviour
{
    [SerializeField] GameObject _resetComfimation;

    [SerializeField] Button _resetSkill;

    [SerializeField] Button _yesReset;

    [SerializeField] Button _noReset;

    [SerializeField] Text _menuCostText;

    [SerializeField] Text _costText;

    [SerializeField] TreeScript _treeScript;

    [SerializeField] SkillSetScripts _selectSkillSet;

    DataBase _dataBase;

    int _cost = 0;

    // Start is called before the first frame update
    void Start()
    {
        _dataBase = DataBase.Instance;
        _resetSkill.onClick.AddListener(ResetSkill);
        _yesReset.onClick.AddListener(YesReset);
        _noReset.onClick.AddListener(NoReset);
        _resetComfimation.SetActive(false);
    }

    public void ResetSkill()
    {
        _resetComfimation.SetActive(true);

        for (var i = 0; i < _dataBase._attackMagicbool.Length; i++)
        {
            if (_dataBase._attackMagicbool[i])
            {
                if (DataBase.Instance.AttackMagicSelectData.SkillInfomation[i]._selectSkill is AttackMagicSelect attackMagic)
                {
                    _cost += attackMagic.SkillPoint;
                }
            }
        }

        _menuCostText.text = _cost.ToString();
    }

    /// <summary>スキル習得状況をリセットするとき処理(現在AttackMagicのみの処理)</summary>
    public void YesReset()
    {
        _resetComfimation.SetActive(false);
        _dataBase.GetSkillPoint(_cost);
        for(var i = 0;i < _dataBase._attackMagicSetNo.Length;i++)
        {
            _dataBase._attackMagicSetNo[i] = 0;
            _selectSkillSet.SelectSkill(0);
            _selectSkillSet.MoveSkillChoice(i);
            _selectSkillSet.SelectSkillReset();
        }
        for(var i = 1;i < _dataBase._attackMagicbool.Length;i++)
        {
            _dataBase._attackMagicbool[i] = false;
        }
        _selectSkillSet.SkillSet(_dataBase._attackMagicbool, DataBase.Instance.AttackMagicSelectData);
        _treeScript.ResetButton();
        _costText.text = _dataBase.SkillPoint.ToString();
        CostReset();
    }

    public void NoReset()
    {
        _resetComfimation.SetActive(false);
        CostReset();
    }

    void CostReset()
    {
        _cost = 0;
    }
}
