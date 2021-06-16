using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
    public static FileInfo[] GetFiles(string filePath)
    {
        DirectoryInfo theFolder = new DirectoryInfo(filePath);
        return theFolder.GetFiles();
    }
    
    public static void Clear(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }
    
    public static void ForceRebuildLayoutImmediate(this RectTransform rectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}