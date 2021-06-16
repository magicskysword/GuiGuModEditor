using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ConfInfo
{
    public string confName;
    public Dictionary<string, string> propertyInfo = new Dictionary<string, string>();

    public string GetPropertyType(string propertyID)
    {
        if (propertyInfo.TryGetValue(propertyID, out string propertyType))
        {
            return propertyType;
        }

        return null;
    }
    
    public static ConfInfo CreateByJson(string confName,string jsonData)
    {
        ConfInfo confInfo = new ConfInfo() {confName = confName};
        JObject jObject = JObject.Parse(jsonData);
        foreach (var kvp in jObject)
        {
            string propertyName = kvp.Key;
            string propertyType = kvp.Value.Value<string>();
            confInfo.propertyInfo.Add(propertyName,propertyType);
        }
        return confInfo;
    }
}