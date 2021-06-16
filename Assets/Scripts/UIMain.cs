using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFB;
using UnityEngine;
using UnityEngine.UI;
using VirtualList;

public class UIMain : SingletonMonoBehaviour<UIMain>
{
    public Button btnNew;
    public Button btnLoad;
    public Button btnSave;

    public Text txtCurPath;
    

    
    public ConfManager confManager;


    private string curPath;

    public string CurPath
    {
        get => curPath;
        set
        {
            curPath = value;
            txtCurPath.text = $"当前路径：{curPath}";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetConfManager();

        btnNew.onClick.AddListener(OnClickNew);
        btnLoad.onClick.AddListener(OnClickLoad);
        btnSave.onClick.AddListener(OnClickSave);
    }
    
    public void ResetConfManager()
    {
        confManager = new ConfManager();
        confManager.Init();
    }

    private void OnClickNew()
    {
        string[] dirPath =
            StandaloneFileBrowser.OpenFolderPanel("选择文件夹", "", false);
        if (dirPath.Length > 0)
        {
            CurPath = dirPath[0];
            ResetConfManager();
        }

        UIConfMainEditor.Instance.RefreshAll();
    }

    private void OnClickLoad()
    {
        string[] dirPath =
            StandaloneFileBrowser.OpenFolderPanel("选择文件夹", "", false);
        if (dirPath.Length > 0)
        {
            CurPath = dirPath[0];
            ResetConfManager();
            confManager.LoadAllConf(CurPath);
        }

        UIConfMainEditor.Instance.RefreshAll();
    }


    private void OnClickSave()
    {
        confManager.SaveAllConf(CurPath);
    }



    
}