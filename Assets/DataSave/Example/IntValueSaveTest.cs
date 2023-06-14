using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntValueSaveTest : MonoBehaviour
{
    private Button _button = default;
    private Text _text = default;

    private int _currentValue = 0;
    public int CurrentValue => _currentValue;

    /// <summary>
    /// ボタンに値を渡してそれを表示する
    /// </summary>
    /// <param name="value"></param>
    public void Initialize(int value)
    {
        _button = GetComponent<Button>();
        _text = gameObject.transform.GetChild(0).GetComponent<Text>();

        _currentValue = value;
        SetValue(value);

        _button.onClick.AddListener(AddValue);
    }

    public void SetValue(int value)
    {
        _text.text = value.ToString();
        _currentValue = value;
    }

    public void AddValue()
    {
        _currentValue++;
        SetValue(_currentValue);
    }
}
