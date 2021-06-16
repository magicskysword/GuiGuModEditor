using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIPropertyNodeInput : UIPropertyNodeBase
{
    public InputField inputField;

    public string StringValue
    {
        get => inputField.text;
        set => inputField.SetTextWithoutNotify(value);
    }

    public int IntValue => Convert.ToInt32(inputField.text);
    public float FloatValue => Convert.ToSingle(inputField.text);

    private UIPropertyType inputType;
    public UIPropertyType InputType
    {
        get => inputType;
        set
        {
            inputType = value;
            switch (value)
            {
                case UIPropertyType.Int:
                    inputField.contentType = InputField.ContentType.IntegerNumber;
                    break;
                case UIPropertyType.Float:
                    inputField.contentType = InputField.ContentType.DecimalNumber;
                    break;
                case UIPropertyType.String:
                default:
                    inputField.contentType = InputField.ContentType.Standard;
                    break;
            };
        }
    }

    private void Awake()
    {
        inputField.onValueChanged.AddListener(OnValueChange);
    }

    public void OnValueChange(string value)
    {
        object lastValue;
        switch (InputType)
        {
            case UIPropertyType.Int:
                if (!string.IsNullOrEmpty(value))
                {
                    var val = Math.Min(Convert.ToInt64(inputField.text), int.MaxValue);
                    lastValue = (int)Math.Max(val, int.MinValue);
                }
                else
                    lastValue = 0;
                break;
            case UIPropertyType.Float:
                if (!string.IsNullOrEmpty(value))
                {
                    var val = Math.Min(Convert.ToDouble(inputField.text), float.MaxValue);
                    lastValue = (float)Math.Max(val, float.MinValue);
                }
                else
                    lastValue = 0f;
                break;
            default:
                lastValue = inputField.text;
                break;
        }
        inputField.text = lastValue.ToString();
        onGetValue?.Invoke(lastValue);
    }
}