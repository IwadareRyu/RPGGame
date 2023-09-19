using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LocalData
{
    static public T Load<T>(string file)
    {
        //ファイルなかったらnullを返す。
        if(!File.Exists(Application.persistentDataPath + "/" + file))
        {
            return default(T);
        }

        var arr = File.ReadAllBytes(Application.persistentDataPath + "/" + file);

        string json = Encoding.UTF8.GetString(arr);
        return JsonUtility.FromJson<T>(json);
    }

    static public void Save<T>(string file,T data)
    {
        var json = JsonUtility.ToJson(data);
        byte[] arr = Encoding.UTF8.GetBytes(json);

        File.WriteAllBytes(Application.persistentDataPath + "/" + file, arr);
    }
}
