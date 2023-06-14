using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveTest : MonoBehaviour
{
    /// <summary>
    /// �V���O���g���̃f�[�^�Z�[�u�N���X�̃Z�[�u�̏������s��
    /// �g�p����^�������Ō��߂Ă����Ă���������
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
    /// �V���O���g���̃f�[�^�Z�[�u�N���X�̃��[�h�������s��
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
