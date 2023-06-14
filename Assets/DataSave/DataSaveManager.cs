using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class DataSaveManager
{
    private static DataSaveManager _instance = new ();
    public static DataSaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"Error! Please correct!");
            }
            return _instance;
        }
    }
    private DataSaveManager() { }

    /// <summary>
    /// データのセーブを行うクラス
    /// </summary>
    /// <typeparam name="T">保存するデータの型</typeparam>
    /// <param name="targetObj">保存するデータ（必ず System.Serializable 属性をつける）</param>
    /// <param name="fileName">保存するファイルの名前</param>
    public void Save<T>(T saveObj, string fileName)
    {
        StreamWriter writer;
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        writer = new StreamWriter(path, false);

        string saveData = JsonUtility.ToJson(saveObj);

        writer.Write(saveData);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// データのロードを行うクラス
    /// </summary>
    /// <typeparam name="T">読み込むデータの型</typeparam>
    /// <param name="fileName">読み込むデータのファイル名（セーブした時と同じもの）</param>
    /// <returns>読み込んだデータ</returns>
    public T Load<T> (string fileName)
    {
        try
        {
            StreamReader reader;
            reader = new StreamReader(Application.persistentDataPath + "/" + fileName + ".json");
            string data = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<T>(data);
        }
        catch (FileNotFoundException)
        {
            return default;
        }
    }
}
