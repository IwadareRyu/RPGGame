using MasterData;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    [SerializeField] bool IsVersionUpFlag = false;
    [SerializeField] bool _resetLoadData = false;
    //[Header("アタックスキル")]
    //[SerializeField] AttackSkill[] _attackData;
    //public AttackSkill[] AttackSkillData => _attackData;
    public bool[] _attackSkillbool;
    public int[] _attackSkillSetNo = new int[3] { 0, 0, 0 };
    private const string _attackSkillFileName = "AttackSkill";

    MasterData.MasterDataClass<MasterData.Skill> attackSkillMaster;
    static public MasterData.Skill[] AttackSkills => Instance.attackSkillMaster.Data;

    delegate void LoadMasterDataCallback<T>(T data);

    [Header("ブロックスキル")]
    //[SerializeField] BlockSkill[] _blockData;
    //public BlockSkill[] BlockSkillData => _blockData;
    public bool[] _blockSkillbool;
    public int[] _blockSkillSetNo = new int[1] {0};
    private const string _blockSkillFileName = "BlockSkill";

    MasterData.MasterDataClass<MasterData.Skill> blockSkillMaster;
    static public MasterData.Skill[] BlockSkills => Instance.blockSkillMaster.Data;

    [Header("攻撃魔法")]
    //[SerializeField] AttackMagic[] _attackMagicData;
    //public AttackMagic[] AttackMagicData => _attackMagicData;
    public bool[] _attackMagicbool;
    public int[] _attackMagicSetNo = new int[2] {0,0};
    private const string _attackMagicFileName = "AttackMagic";

    MasterData.MasterDataClass<MasterData.Skill> attackMagicMaster;
    static public MasterData.Skill[] AttackMagics => Instance.attackMagicMaster.Data;

    [Header("防御魔法")]
    //[SerializeField] BlockMagic[] _blockMagicData;
    //public BlockMagic[] BlockMagicData => _blockMagicData;
    public bool[] _blockMagicbool;
    public int[] _blockMagicSetNo = new int[2] {0,0};
    private const string _blockMagicFileName = "BlockMagic";

    MasterData.MasterDataClass<MasterData.Skill> blockMagicMaster;
    static public MasterData.Skill[] BlockMagics => Instance.blockMagicMaster.Data;

    [System.Serializable]
    public struct SkillData
    {
        public bool[] _getSkill;
        public int[] _SetNo;
    }

    [System.Serializable]
    public struct SkillPt
    {
        public int _skillPt;
    }

    SkillPt _pt;

    SkillData _skillData;
    SkillData _attackSkillData;
    SkillData _blockSkillData;
    SkillData _blockMagicSkillData;
    SkillData _attackMagicSkillData;

    [SerializeField]
    DataSaveTest _dataSave;

    static DataBase instance = null;
    public static DataBase Instance => instance;

    int LoadingCount = 0;
    int IsInit = 0;
    
    [Header("今持っているスキルポイント"),Tooltip("今持っているスキルポイント"),SerializeField]
    int _skillPoint = 5;
    public int SkillPoint => _skillPoint;
    private const string _skillPointFile = "SkillPoint";
    public void GetSkillPoint(int i)
    {
        _skillPoint += i;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadMasterData("AttackSkill",(MasterData.MasterDataClass<MasterData.Skill> data)=> attackSkillMaster = data, "https://script.google.com/macros/s/AKfycbxD-VLj76crR54K8pQymLk2j-9pU9lTxrJZGAJPiNsKVEPYMGBMI4PHhEZuWp-QLlbQxQ/exec?sheet=");
        LoadMasterData("BlockSkill", (MasterData.MasterDataClass<MasterData.Skill> data) => blockSkillMaster = data, "https://script.google.com/macros/s/AKfycbzdgyB-ovKFBPe0LEmKBX0z7A6Mg4_naJ0IND9vvXRPDoebCZ2i7DW8RmL7TENvc8rQ/exec?sheet=");
        LoadMasterData("AttackMagic", (MasterData.MasterDataClass<MasterData.Skill> data) => attackMagicMaster = data, "https://script.google.com/macros/s/AKfycby-oXlJZ_LgQmqaoUsc1_t2Ka19HdpepnrPaLqZ3gT5Dwl6TrUiREEbpaw5QFIdMiue-w/exec?sheet=");
        LoadMasterData("BlockMagic", (MasterData.MasterDataClass<MasterData.Skill> data) => blockMagicMaster = data, "https://script.google.com/macros/s/AKfycbylB-h568JHNdda_Am68zPZBYzUZ5sHJnFcd-ib_CmUJECn201GBrrLCPHyBMALoNUzqA/exec?sheet=");

    }
    private void OnEnable()
    {
        //セーブデータロード処理
        if (_resetLoadData)
        {
            _attackMagicSkillData = SkillDataLoad(_attackMagicFileName);
            _attackMagicSetNo = SetNoLoad(_attackMagicSetNo, _attackMagicSkillData);
            _attackMagicbool = GetBoolLoad(_attackMagicbool, _attackMagicSkillData);
            _blockSkillData = SkillDataLoad(_blockSkillFileName);
            _blockSkillSetNo = SetNoLoad(_blockSkillSetNo, _blockSkillData);
            _attackSkillData = SkillDataLoad(_attackSkillFileName);
            _attackSkillSetNo = SetNoLoad(_attackSkillSetNo, _attackSkillData);
            _blockMagicSkillData = SkillDataLoad(_blockMagicFileName);
            _blockMagicSetNo = SetNoLoad(_blockMagicSetNo, _blockMagicSkillData);
            _skillPoint = SkillPointLoad(_skillPointFile);
        }
        //_skillPoint = 0;
    }

    private void Update()
    {
        if (LoadingCount == 0 && IsInit == 0)
        {
            IsInit = 1;
            for (var i = 0;i < attackSkillMaster.Data.Length;i++)
            {
                var attackSkill = attackSkillMaster.Data[i];
                Debug.Log($"{attackSkill.ID} {attackSkill.SkillName} {attackSkill.AttackValue} {attackSkill.RequaireAttack} {attackSkill.SkillType}");
            }

            for(var i = 0;i < blockSkillMaster.Data.Length;i++)
            {
                var blockSkill = blockSkillMaster.Data[i];
                Debug.Log($"{blockSkill.ID} {blockSkill.SkillName} {blockSkill.AttackValue} {blockSkill.OffencePower} {blockSkill.DiffencePower} {blockSkill.SkillType}");
            }
        }
        else if (IsInit == 0)
        {
            Debug.Log("読み込み失敗");
        }
    }

    private void OnDisable()
    {
        //セーブデータセーブ処理
        SkillDataSave(_attackMagicSetNo, _attackMagicbool, _attackMagicFileName);
        SkillDataSave(_attackSkillSetNo, _attackSkillbool, _attackSkillFileName);
        SkillDataSave(_blockSkillSetNo, _blockSkillbool, _blockSkillFileName);
        SkillDataSave(_blockMagicSetNo, _blockMagicbool, _blockMagicFileName);
        PtDataSave(_skillPoint,_skillPointFile);
    }

    private void LoadMasterData<T>(string file, LoadMasterDataCallback<T> callback,string url)
    {
        var data = LocalData.Load<T>(file);
        if (data == null || IsVersionUpFlag)
        {
            LoadingCount++;
            Network.WebRequest.Request<Network.WebRequest.GetString>(url + file, 
                Network.WebRequest.ResultType.String, 
                (string json) =>
            {
                var dldata = JsonUtility.FromJson<T>(json);
                LocalData.Save<T>(file, dldata);
                callback(dldata);
                Debug.Log($"NetWork download : {file} / {json} / {dldata}");
                --LoadingCount;
            });
        }
        else
        {
            Debug.Log($"Local load : {file} / {data}");
            callback(data);
        }
    }


    void SkillDataSave(int[] setNo, bool[] getSkillbool,string fileName)
    {
        _skillData._SetNo = setNo;
        _skillData._getSkill = getSkillbool;
        _dataSave.SaveSkill(_skillData, fileName);
    }

    void PtDataSave(int skillPt,string skillPtFile)
    {
        _pt._skillPt = skillPt;
        _dataSave.SaveSkillPoint(_pt, skillPtFile);
    }

    SkillData SkillDataLoad(string fileName)
    {
        return _dataSave.LoadSkill(fileName);
    }

    int[] SetNoLoad(int[] setNo,SkillData skillData)
    {
        if (skillData._SetNo.Length != 0 && setNo.Length == skillData._SetNo.Length)
        {
            return skillData._SetNo;
        }
        return setNo;
    }

    bool[] GetBoolLoad(bool[] getBool,SkillData skillData)
    {
        if(skillData._getSkill.Length != 0)
        {
            return skillData._getSkill;
        }
        return getBool;
    }

    int SkillPointLoad(string fileName)
    {
        _pt = _dataSave.LoadPoint(fileName);
        if(_pt._skillPt != null)
        {
            return _pt._skillPt;
        }
        return _skillPoint;
    }
}
