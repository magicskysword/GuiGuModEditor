using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ConfManager
{
    public List<ConfInfo> confInfos = new List<ConfInfo>();
    public Dictionary<string, ConfInfo> confInfosDic = new Dictionary<string, ConfInfo>();

    public List<string> haveConf = new List<string>();
    public Dictionary<string, List<ConfItem>> items = new Dictionary<string, List<ConfItem>>();

    public void Init()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Base/");
        FileInfo[] fileInfos = Utility.GetFiles(filePath);
        foreach (var fileInfo in fileInfos)
        {
            if (fileInfo.Extension.Equals(".json", StringComparison.CurrentCultureIgnoreCase))
            {
                string confName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                using var reader = new StreamReader(fileInfo.FullName);
                string confJson = reader.ReadToEnd();
                var confInfo = ConfInfo.CreateByJson(confName,confJson);
                confInfos.Add(confInfo);
                confInfosDic.Add(confName,confInfo);
            }
        }
    }

    public void AddConf(string confName)
    {
        if(!items.ContainsKey(confName))
        {
            items.Add(confName,new List<ConfItem>());
            haveConf.Add(confName);
            haveConf.Sort();
        }
    }

    public void DeleteConf(string confName)
    {
        if(items.ContainsKey(confName))
        {
            items.Remove(confName);
            haveConf.Remove(confName);
        }
    }

    public void LoadAllConf(string path)
    {
        FileInfo[] fileInfos = Utility.GetFiles(path);
        foreach (var fileInfo in fileInfos)
        {
            if (fileInfo.Extension.Equals(".json", StringComparison.CurrentCultureIgnoreCase))
            {
                string confName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                using var reader = new StreamReader(fileInfo.FullName);
                string confJson = reader.ReadToEnd();
                if (confInfosDic.TryGetValue(confName, out var confInfo))
                {
                    haveConf.Add(confName);
                    items[confName] = ConfItem.CreateListByJson(confInfo, confJson);
                }
            }
        }
    }
    
    public void SaveAllConf(string path)
    {
        foreach (var kvp in items)
        {
            var curConf = confInfosDic[kvp.Key];
            var curPath = Path.Combine(path, $"{kvp.Key}.json");
            using JsonWriter jsonWriter = new JsonTextWriter(new StreamWriter(curPath))
            {
                Formatting = Formatting.Indented,
                Indentation = 2
            };
            jsonWriter.WriteStartArray();
            foreach (var confItem in kvp.Value)
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("id");
                jsonWriter.WriteValue(confItem.id);
                foreach (var kvp2 in curConf.propertyInfo)
                {
                    jsonWriter.WritePropertyName(kvp2.Key);
                    var obj = confItem.properties[kvp2.Key];
                    switch (kvp2.Value)
                    {
                        case "int":
                            jsonWriter.WriteValue((int)obj);
                            break;
                        case "float":
                            jsonWriter.WriteValue((float)obj);
                            break;
                        case "string":
                            jsonWriter.WriteValue((string)obj);
                            break;
                        case "intArray":
                            jsonWriter.WriteStartArray();
                            foreach (var i in obj as List<int>)
                            {
                                jsonWriter.WriteValue(i);
                            }
                            jsonWriter.WriteEndArray();
                            break;
                        case "stringArray":
                            jsonWriter.WriteStartArray();
                            foreach (var s in obj as List<string>)
                            {
                                jsonWriter.WriteValue(s);
                            }
                            jsonWriter.WriteEndArray();
                            break;
                        case "intArrayArray":
                            jsonWriter.WriteStartArray();
                            foreach (var listInt in obj as List<List<int>>)
                            {
                                jsonWriter.WriteStartArray();
                                foreach (var i in listInt)
                                {
                                    jsonWriter.WriteValue(i);
                                }
                                jsonWriter.WriteEndArray();
                            }
                            jsonWriter.WriteEndArray();
                            break;
                        case "floatArrayArray":
                            jsonWriter.WriteStartArray();
                            foreach (var listInt in obj as List<List<float>>)
                            {
                                jsonWriter.WriteStartArray();
                                foreach (var i in listInt)
                                {
                                    jsonWriter.WriteValue(i);
                                }
                                jsonWriter.WriteEndArray();
                            }
                            jsonWriter.WriteEndArray();
                            break;
                        case "stringArrayArray":
                            jsonWriter.WriteStartArray();
                            foreach (var listInt in obj as List<List<string>>)
                            {
                                jsonWriter.WriteStartArray();
                                foreach (var i in listInt)
                                {
                                    jsonWriter.WriteValue(i);
                                }
                                jsonWriter.WriteEndArray();
                            }
                            jsonWriter.WriteEndArray();
                            break;
                        default:
                            Debug.LogWarning($"{kvp.Key} 的 {kvp2.Key} 为无法解析的类型：{kvp2.Value}");
                            jsonWriter.WriteValue(obj);
                            break;
                    }
                }
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
        }
    }
    
    
    
}