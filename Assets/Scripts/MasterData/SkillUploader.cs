using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting;
using UnityEditor;

public class SkillUploader : MonoBehaviour
{
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField]string _attackMagicURL = "https://script.google.com/macros/s/AKfycbzJZAN4noJWn_pMrDJpqtWlSzaQpnCm2I6fPVIUtiWpRxiVnMGF8BDG5d-fr2des8ZzXA/exec";
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField]string _blockMagicURL = "https://script.google.com/macros/s/AKfycbylB-h568JHNdda_Am68zPZBYzUZ5sHJnFcd-ib_CmUJECn201GBrrLCPHyBMALoNUzqA/exec";
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField]string _attackSkillURL = "https://script.google.com/macros/s/AKfycbxD-VLj76crR54K8pQymLk2j-9pU9lTxrJZGAJPiNsKVEPYMGBMI4PHhEZuWp-QLlbQxQ/exec";
    [ContextMenuItem("SetUp", "SetUp")]
    [SerializeField] string _blockSkillURL = "https://script.google.com/macros/s/AKfycbzdgyB-ovKFBPe0LEmKBX0z7A6Mg4_naJ0IND9vvXRPDoebCZ2i7DW8RmL7TENvc8rQ/exec";
    [SerializeField] bool IsVersionUpFlag = false;

    [SerializeField]SelectorSkillObjects _attackSkillsSelect;
    [SerializeField] SelectorSkillObjects _blockSkillsSelect;
    [SerializeField] SelectorSkillObjects _attackMagicsSelect;
    [SerializeField] SelectorSkillObjects _blockMagicsSelect;

    void SetUp()
    {
        StartCoroutine(LoadMasterData(_attackSkillsSelect,_attackSkillURL));
        StartCoroutine(LoadMasterData(_blockSkillsSelect,_blockSkillURL));
        StartCoroutine(LoadMasterData(_attackMagicsSelect,_attackMagicURL));
        StartCoroutine(LoadMasterData(_blockMagicsSelect,_blockMagicURL));
    }

    IEnumerator LoadMasterData(SelectorSkillObjects selectorSkill,string url)
    {
        if(IsVersionUpFlag)
        {
            UnityWebRequest request = UnityWebRequest.Get($"{url}?sheet=");
            yield return request.SendWebRequest();
            Debug.Log("受け取り完了");
            string s = request.downloadHandler.text;
            Debug.Log(s);
            MasterDataClass<Skill> data = JsonUtility.FromJson<MasterDataClass<Skill>>(s);
            selectorSkill.AttributeSkillLoad(ref data);
#if UNITY_EDITOR
            EditorUtility.SetDirty(selectorSkill);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
