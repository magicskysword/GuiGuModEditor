using System;
using System.Collections.Generic;
using LitJson;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ConfItem
{
    public int id;
    public Dictionary<string, object> properties = new Dictionary<string, object>();

    public ConfItem()
    {
        
    }

    public ConfItem(ConfInfo confInfo)
    {
        id = 0;
        foreach (var kvp in confInfo.propertyInfo)
        {
            var propertyID = kvp.Key;
            switch (kvp.Value)
            {
                case "int":
                    properties[propertyID] = 0;
                    break;
                case "float":
                    properties[propertyID] = 0f;
                    break;
                case "string":
                    properties[propertyID] = "";
                    break;
                case "intArray":
                    properties[propertyID] = new List<int>();
                    break;
                case "stringArray":
                    properties[propertyID] = new List<float>();
                    break;
                case "intArrayArray":
                    properties[propertyID] = new List<List<int>>();
                    break;
                case "floatArrayArray":
                    properties[propertyID] = new List<List<float>>();
                    break;
                case "stringArrayArray":
                    properties[propertyID] = new List<List<string>>();
                    break;
            }
        }
    }

    public static List<ConfItem> CreateListByJson(ConfInfo confInfo, string jsonData)
    {
        List<ConfItem> items = new List<ConfItem>();
        JArray array = null;
        try
        {
            array = JArray.Parse(jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
        foreach (var token in array)
        {
            if (token.Type != JTokenType.Object)
            {
                Debug.LogError($"Json {confInfo.confName} 数据出错。");
            }
            ConfItem confItem = new ConfItem();
            confItem.ProcessObject(confInfo,token.Value<JObject>());
            items.Add(confItem);
        }
        return items;
    }
    
    private void ProcessObject(ConfInfo confInfo,JObject obj)
    {
        foreach (var kvp in obj)
        {
            string propertyID = kvp.Key;
            if (propertyID == "id")
            {
                id = kvp.Value.Value<int>();
                continue;
            }
            string objType = confInfo.GetPropertyType(propertyID);
            if (objType != null)
            {
                switch (objType)
                {
                    case "int":
                        properties[propertyID] = kvp.Value.Value<int>();
                        break;
                    case "float":
                        properties[propertyID] = kvp.Value.Value<float>();
                        break;
                    case "string":
                        properties[propertyID] = kvp.Value.Value<string>();
                        break;
                    case "intArray":
                        List<int> listInt = new List<int>();
                        properties[propertyID] = listInt;
                        foreach (var value in kvp.Value.Values())
                        {
                            listInt.Add(value.Value<int>());
                        }
                        break;
                    case "stringArray":
                        List<string> listStr = new List<string>();
                        properties[propertyID] = listStr;
                        foreach (var value in kvp.Value.Values())
                        {
                            listStr.Add(value.Value<string>());
                        }
                        break;
                    case "intArrayArray":
                        List<List<int>> listInt2 = new List<List<int>>();
                        properties[propertyID] = listInt2;
                        foreach (var value in kvp.Value.Children())
                        {
                            List<int> list2_1 = new List<int>();
                            listInt2.Add(list2_1);
                            foreach (var fValue in value.Values())
                            {
                                list2_1.Add(fValue.Value<int>());
                            }
                        }
                        break;
                    case "floatArrayArray":
                        List<List<float>> listFloat2 = new List<List<float>>();
                        properties[propertyID] = listFloat2;
                        foreach (var value in kvp.Value.Values())
                        {
                            List<float> list2_1 = new List<float>();
                            listFloat2.Add(list2_1);
                            foreach (var fValue in value.Values())
                            {
                                list2_1.Add(fValue.Value<float>());
                            }
                        }
                        break;
                    case "stringArrayArray":
                        List<List<string>> listStr2 = new List<List<string>>();
                        properties[propertyID] = listStr2;
                        foreach (var value in kvp.Value.Values())
                        {
                            List<string> list2_1 = new List<string>();
                            listStr2.Add(list2_1);
                            foreach (var fValue in value.Values())
                            {
                                list2_1.Add(fValue.Value<string>());
                            }
                        }
                        break;
                    default:
                        Debug.LogWarning($"{confInfo.confName} 中含有无法解析的类型：{objType}");
                        break;
                }
            }
            else
            {
                Debug.LogError($"{kvp.Key} 在 {confInfo.confName} 中未找到对应值。");
            }
        }
    }
}