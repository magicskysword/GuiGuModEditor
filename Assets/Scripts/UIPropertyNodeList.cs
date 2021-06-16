using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPropertyNodeList : UIPropertyNodeBase
{
    public List<UIPropertyNodeBase> list = new List<UIPropertyNodeBase>();
    public UIPropertyType itemType;
    public InputField inputNum;
    public Button btnChangeNum;
    public object bindData;

    private void Start()
    {
        btnChangeNum.onClick.AddListener(OnClickChangeNum);
    }

    private void OnClickChangeNum()
    {
        var num = Convert.ToInt32(inputNum.text);
        ChangeItemNum(num);
        OnChangeListNum(num);
        baseProperty.ResetProperty();
    }

    public void ChangeItemNum(int num)
    {
        inputNum.text = num.ToString();
        var count = list.Count;
        if (count < num)
        {
            var listCount = num - count;
            for (int i = 0; i < listCount; i++)
            {
                var obj
                    = Instantiate(UIConfMainEditor.Instance.GetPrefab(itemType), transform, false);
                var uiPropertyNodeBase = obj.GetComponent<UIPropertyNodeBase>();
                list.Add(uiPropertyNodeBase);
                uiPropertyNodeBase.baseProperty = baseProperty;
                var curIndex = count + i;
                uiPropertyNodeBase.onGetValue = o => OnValueChangeInList(curIndex, o);
                switch (uiPropertyNodeBase)
                {
                    case UIPropertyNodeInput uiPropertyNodeInput:
                        uiPropertyNodeInput.InputType = itemType;
                        break;
                    case UIPropertyNodeList uiPropertyNodeList:
                        switch (itemType)
                        {
                            case UIPropertyType.IntArray:
                                uiPropertyNodeList.itemType = UIPropertyType.Int;
                                break;
                            case UIPropertyType.FloatArray:
                                uiPropertyNodeList.itemType = UIPropertyType.Float;
                                break;
                            case UIPropertyType.StringArray:
                                uiPropertyNodeList.itemType = UIPropertyType.String;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                }
            }
        }
        else if(count > num)
        {
            for (int i = count - 1; i >= num; i--)
            {
                Destroy(list[i].gameObject);
            }
            list.RemoveRange(num,count-num);
        }
    }
    
    public void OnValueChangeInList(int index,object value)
    {
        IList list = bindData as IList;
        list[index] = value;
        onGetValue?.Invoke(list);
    }
    
    public void OnChangeListNum(int num)
    {
        IList list = bindData as IList;
        var count = list.Count;
        if (num > list.Count)
        {
            var listCount = num - count;
            for (int i = 0; i < listCount; i++)
            {
                switch (itemType)
                {
                    case UIPropertyType.Int:
                        list.Add(0);
                        break;
                    case UIPropertyType.Float:
                        list.Add(0f);
                        break;
                    case UIPropertyType.String:
                        list.Add("");
                        break;
                    case UIPropertyType.IntArray:
                        list.Add(new List<int>());
                        break;
                    case UIPropertyType.FloatArray:
                        list.Add(new List<float>());
                        break;
                    case UIPropertyType.StringArray:
                        list.Add(new List<string>());
                        break;
                }
            }
        }
        else
        {
            for (int i = count - 1; i >= num; i--)
            {
                list.RemoveAt(i);
            }
        }
        onGetValue?.Invoke(list);
    }
}