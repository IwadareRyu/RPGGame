using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveTest : MonoBehaviour
{
    /// <summary>
    /// シングルトンのデータセーブクラスのセーブの処理を行う
    /// 使用する型をここで決めておいてもいいかも
    /// </summary>
    public void Save(ButtonsDataController.ButtonData saveObj, string fileName)
    {
        DataSaveManager.Instance.Save(saveObj, fileName);
    }

    public void SaveSkill(DataBase.SkillData skillData,string fileName)
    {
        DataSaveManager.Instance.Save(skillData, fileName);
    }

    /// <summary>
    /// シングルトンのデータセーブクラスのロード処理を行う
    /// </summary>
    public ButtonsDataController.ButtonData Load(string fileName)
    {
        return DataSaveManager.Instance.Load<ButtonsDataController.ButtonData>(fileName);   
    }

    public DataBase.SkillData LoadSkill(string fileName)
    {
        return DataSaveManager.Instance.Load<DataBase.SkillData>(fileName);
    }
}
