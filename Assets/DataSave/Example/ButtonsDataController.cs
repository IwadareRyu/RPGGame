using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsDataController : MonoBehaviour
{
    [SerializeField]
    private IntValueSaveTest[] _buttons = default;

    [SerializeField]
    private DataSaveTest _dataSave = default;

    [SerializeField]
    private Button _resetButton;

    /// <summary>
    /// 今回保存するファイルの名前
    /// </summary>
    private const string FILE_NAME = "test";

    /// <summary>
    /// 今回保存するデータ
    /// Serializable属性を持たせないとダメ
    /// </summary>
    [System.Serializable]
    public struct ButtonData
    {
        public int[] nums;
    }

    private void Start()
    {
        DataLoad();

        //リセット処理をリセットボタンに追加
        _resetButton?.onClick.AddListener(DataReset);
    }

    /// <summary>
    /// データのロードを行うクラス
    /// </summary>
    private void DataLoad()
    {
        ButtonData buttonData = _dataSave.Load(FILE_NAME);
        //データを読み込めなかったらデータに0を入れておく
        if (buttonData.nums == null)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Initialize(0);
            }
        }
        //読み込めたらデータをボタンに渡す
        else
        {
            for (int j = 0; j < _buttons.Length; j++)
            {
                _buttons[j].Initialize(buttonData.nums[j]);
            }
        }
    }

    private void OnDisable()
    {
        DataSave();
    }

    /// <summary>
    /// データの保存を行うクラス
    /// </summary>
    private void DataSave()
    {
        var data = new ButtonData
        {
            nums = new int[5]
        };

        for (int k = 0; k < _buttons.Length; k++)
        {
            //それぞれのボタンが持つ値を構造体に渡す
            data.nums[k] = _buttons[k].CurrentValue;
        }

        _dataSave.Save(data, FILE_NAME);
    }

    /// <summary>
    /// データを全て0にリセットする
    /// </summary>
    private void DataReset()
    {
        var data = new ButtonData
        {
            nums = new int[5]
        };

        for (int l = 0; l < _buttons.Length; l++)
        {
            _buttons[l].SetValue(0);
        }

        _dataSave.Save(data, FILE_NAME);
    }
}
