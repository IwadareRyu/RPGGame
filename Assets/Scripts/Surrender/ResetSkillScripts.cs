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
                _cost += _dataBase.AttackMagicData[i].SkillPoint;
            }
        }

        _menuCostText.text = _cost.ToString();
    }

    public void YesReset()
    {
        _resetComfimation.SetActive(false);
        _dataBase.GetSkillPoint(_cost);
        for(var i = 0;i < _dataBase._attackMagicSetNo.Length;i++)
        {
            _dataBase._attackMagicSetNo[i] = 0;
        }
        for(var i = 1;i < _dataBase._attackMagicbool.Length;i++)
        {
            _dataBase._attackMagicbool[i] = false;
        }
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
