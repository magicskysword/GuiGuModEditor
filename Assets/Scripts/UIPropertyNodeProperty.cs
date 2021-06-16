using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPropertyNodeProperty : UIPropertyNodeBase
{
    public string propertyType;
    public string propertyID;
    public object propertyValue;
    public UIPropertyNodeBase bindNode;
    public Text txtLabel;
    public ConfItem tagConfItem;

    public void SetAllProperty()
    {
        txtLabel.text = propertyID;
        switch (propertyType)
        {
            case "int":
            case "float":
            case "string":
                var nodeInput = bindNode as UIPropertyNodeInput;
                nodeInput.StringValue = propertyValue.ToString();
                nodeInput.onGetValue = OnGetValue;
                break;
            case "intArray":
            case "stringArray":
                var nodeList = bindNode as UIPropertyNodeList;
                var valueList = propertyValue as IList;
                switch (propertyType)
                {
                    case "intArray":
                        nodeList.itemType = UIPropertyType.Int;
                        break;
                    case "stringArray":
                        nodeList.itemType = UIPropertyType.String;
                        break;
                }
                nodeList.ChangeItemNum(valueList.Count);
                nodeList.bindData = propertyValue;
                for (int i = 0; i < valueList.Count; i++)
                {
                    var nodeInput2 = nodeList.list[i] as UIPropertyNodeInput;
                    nodeInput2.StringValue = valueList[i].ToString();
                }
                break;
            case "intArrayArray":
            case "floatArrayArray":
            case "stringArrayArray":
                var nodeList2 = bindNode as UIPropertyNodeList;
                var valueList2 = propertyValue as IList;
                switch (propertyType)
                {
                    case "intArrayArray":
                        nodeList2.itemType = UIPropertyType.IntArray;
                        break;
                    case "floatArrayArray":
                        nodeList2.itemType = UIPropertyType.FloatArray;
                        break;
                    case "stringArrayArray":
                        nodeList2.itemType = UIPropertyType.StringArray;
                        break;
                }
                nodeList2.ChangeItemNum(valueList2.Count);
                nodeList2.bindData = propertyValue;
                for (int i = 0; i < valueList2.Count; i++)
                {
                    var nodeList3 = nodeList2.list[i] as UIPropertyNodeList;
                    nodeList3.baseProperty = this;
                    switch (propertyType)
                    {
                        case "intArrayArray":
                            nodeList3.itemType = UIPropertyType.Int;
                            break;
                        case "floatArrayArray":
                            nodeList3.itemType = UIPropertyType.Float;
                            break;
                        case "stringArrayArray":
                            nodeList3.itemType = UIPropertyType.String;
                            break;
                    }

                    IList nodeList3BindData = valueList2[i] as IList;
                    nodeList3.ChangeItemNum(nodeList3BindData.Count);
                    nodeList3.bindData = nodeList3BindData;
                    for (int j = 0; j < nodeList3BindData.Count; j++)
                    {
                        var nodeInput2 = nodeList3.list[j] as UIPropertyNodeInput;
                        nodeInput2.StringValue = nodeList3BindData[j].ToString();
                    }
                }
                break;
            default:
                Debug.LogWarning($"{propertyID} 为无法解析的类型：{propertyType}");
                break;
        }
    }

    public void ResetProperty()
    {
        UIConfMainEditor.Instance.OnPropertyChange();
    }

    private void OnGetValue(object obj)
    {
        propertyValue = obj;
        SaveCurItemProperty(propertyID,propertyValue);
    }
    
    public void SaveCurItemProperty(string propertyID,object propertyValue)
    {
        if (propertyID == "id")
        {
            tagConfItem.id = Convert.ToInt32(propertyValue);
            UIConfMainEditor.Instance.RefreshConf();
        }
        else
        {
            tagConfItem.properties[propertyID] = propertyValue;
        }
    }
}