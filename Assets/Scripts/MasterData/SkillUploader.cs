using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;
using UnityEngine.Networking;
using System;

public class SkillUploader : MonoBehaviour
{
    [SerializeField] AttackMagic[] _attackMagics;
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField]string _attackMagicURL = "https://script.google.com/macros/s/AKfycbzJZAN4noJWn_pMrDJpqtWlSzaQpnCm2I6fPVIUtiWpRxiVnMGF8BDG5d-fr2des8ZzXA/exec";
    [SerializeField] BlockMagic[] _blockMagics;
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField]string _blockMagicURL = "https://script.google.com/macros/s/AKfycbylB-h568JHNdda_Am68zPZBYzUZ5sHJnFcd-ib_CmUJECn201GBrrLCPHyBMALoNUzqA/exec";
    [SerializeField] AttackSkill[] _attackSkills;
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField]string _attackSkillURL = "https://script.google.com/macros/s/AKfycbxD-VLj76crR54K8pQymLk2j-9pU9lTxrJZGAJPiNsKVEPYMGBMI4PHhEZuWp-QLlbQxQ/exec";
    [SerializeField] BlockSkill[] _blockSkills;
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField] string _blockSkillURL = "https://script.google.com/macros/s/AKfycbzdgyB-ovKFBPe0LEmKBX0z7A6Mg4_naJ0IND9vvXRPDoebCZ2i7DW8RmL7TENvc8rQ/exec";
    [SerializeField] bool IsVersionUpFlag = false;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(LoadMasterData(SkillType.AttackSkill, _attackSkillURL));
        StartCoroutine(LoadMasterData(SkillType.BlockSkill, _blockSkillURL));
        StartCoroutine(LoadMasterData(SkillType.AttackMagic, _attackMagicURL));
        StartCoroutine(LoadMasterData(SkillType.BlockMagic, _blockMagicURL));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUp()
    {
        StartCoroutine(LoadMasterData(SkillType.AttackSkill,_attackSkillURL));
        StartCoroutine(LoadMasterData(SkillType.BlockSkill,_blockSkillURL));
        StartCoroutine(LoadMasterData(SkillType.AttackMagic,_attackMagicURL));
        StartCoroutine(LoadMasterData(SkillType.BlockMagic,_blockMagicURL));
    }

    IEnumerator LoadMasterData(SkillType type,string url)
    {
        if(IsVersionUpFlag)
        {
            UnityWebRequest request = UnityWebRequest.Get($"{url}?sheet=");
            yield return request.SendWebRequest();
            Debug.Log("Žó‚¯Žæ‚èŠ®—¹");
            string s = request.downloadHandler.text;
            Debug.Log(s);
            MasterDataClass<Skill> data = JsonUtility.FromJson<MasterDataClass<Skill>>(s);
            switch (type) 
            {
                case SkillType.AttackSkill:
                    for(int i = 0;i < _attackSkills.Length;i++)
                    {
                        _attackSkills[i].AttackSkillLoad(data.Data[i]);
                    }
                    break;
                case SkillType.BlockSkill:
                    for(int i = 0;i < _blockSkills.Length;i++)
                    {
                        _blockSkills[i].BlockSkillLoad(data.Data[i]);
                    }
                    break;
                case SkillType.AttackMagic:
                    for (int i = 0; i < _attackMagics.Length; i++)
                    {
                        _attackMagics[i].AttackMagicLoad(data.Data[i]);
                    }
                    break;
                case SkillType.BlockMagic:
                    for (int i = 0; i < _blockMagics.Length; i++)
                    {
                        _blockMagics[i].BlockMagicLoad(data.Data[i]);
                    }
                    break;
            }
        }
    }
}
