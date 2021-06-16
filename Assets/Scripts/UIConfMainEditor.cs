using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VirtualList;

public class UIConfMainEditor : SingletonMonoBehaviour<UIConfMainEditor>
{
    public Dropdown dropdown;
    public Button btnConfAdd;
    public Button btnConfDelete;
    public Button btnItemAdd;
    public Button btnItemDelete;
    public ToggleGroup confContent;
    public ToggleGroup itemContent;
    public GameObject dataContent;
    [Space]
    public GameObject nodeSelectPrefab;
    public GameObject nodePropertyPrefab;
    public GameObject nodeInputPrefab;
    public GameObject nodeListPrefab;
    
    public void RefreshAll()
    {
        curConfInfo = null;
        curConfItem = null;
        RefreshContent();
        RefreshConf();
        OnPropertyChange();
        dropdown.AddOptions(UIMain.Instance.confManager.confInfos.Select(info => info.confName).ToList());
    }

    
    private ConfInfo curConfInfo = null;
    private ConfItem curConfItem = null;

    private void Start()
    {
        btnConfAdd.onClick.AddListener(OnClickConfAdd);
        btnConfDelete.onClick.AddListener(OnClickConfDelete);
        btnItemAdd.onClick.AddListener(OnClickItemAdd);
        btnItemDelete.onClick.AddListener(OnClickItemDelete);
    }
    
    public GameObject GetPrefab(UIPropertyType propertyType)
    {
        switch (propertyType)
        {
            case UIPropertyType.Int:
            case UIPropertyType.Float:
            case UIPropertyType.String:
                return nodeInputPrefab;
            case UIPropertyType.IntArray:
            case UIPropertyType.FloatArray:
            case UIPropertyType.StringArray:
                return nodeListPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(propertyType), propertyType, null);
        }
    }
    
    private void OnClickConfAdd()
    {
        string getConfName = dropdown.options[dropdown.value].text;
        UIMain.Instance.confManager.AddConf(getConfName);
        RefreshAll();
    }

    private void OnClickConfDelete()
    {
        string getConfName = dropdown.options[dropdown.value].text;
        UIMain.Instance.confManager.DeleteConf(getConfName);
        RefreshAll();
    }

    private void OnClickItemAdd()
    {
        if (curConfInfo != null)
        {
            UIMain.Instance.confManager.items[curConfInfo.confName].Add(new ConfItem(curConfInfo));
        }

        RefreshContent();
        RefreshConf();
    }

    private void OnClickItemDelete()
    {
        if (curConfInfo != null)
        {
            UIMain.Instance.confManager.items[curConfInfo.confName].Remove(curConfItem);
        }

        RefreshConf();
        RefreshContent();
        OnPropertyChange();
    }
    
    private void RefreshContent()
    {
        confContent.transform.Clear();
        AbstractVirtualList virtualList = confContent.GetComponent<AbstractVirtualList>();
        virtualList.Clear();
        VirtualSelectList virtualSelectList = new VirtualSelectList();
        var curList = UIMain.Instance.confManager.haveConf
            .Select(confName => $"{confName}({UIMain.Instance.confManager.items[confName].Count})").ToList();
        virtualSelectList.names.AddRange(curList);
        virtualSelectList.selectEvent = SelectConf;
        virtualList.SetSource(virtualSelectList);
    }

    public void SelectConf(int index)
    {
        itemContent.transform.Clear();
        string confName = UIMain.Instance.confManager.haveConf[index];
        curConfInfo = UIMain.Instance.confManager.confInfosDic[confName];
        curConfItem = null;
        RefreshConf();
        OnPropertyChange();
    }

    public void RefreshConf()
    {
        AbstractVirtualList virtualList = itemContent.GetComponent<AbstractVirtualList>();
        virtualList.Clear();
        if (curConfInfo == null)
            return;
        VirtualSelectList virtualSelectList = new VirtualSelectList();
        var curList = UIMain.Instance.confManager.items[curConfInfo.confName].Select(conf => conf.id.ToString()).ToList();
        virtualSelectList.names.AddRange(curList);
        virtualSelectList.selectEvent = SelectItem;
        virtualList.SetSource(virtualSelectList);
    }

    private void SelectItem(int index)
    {
        curConfItem = UIMain.Instance.confManager.items[curConfInfo.confName][index];
        OnPropertyChange();
    }

    public void OnPropertyChange()
    {
        dataContent.transform.Clear();
        if (curConfItem == null)
            return;
        ShowItemProperty(dataContent.transform, curConfItem, "id", "int", curConfItem.id);
        foreach (var kvp in curConfInfo.propertyInfo)
        {
            ShowItemProperty(dataContent.transform, curConfItem, kvp.Key,
                kvp.Value, curConfItem.properties[kvp.Key]);
        }
    }

    private void ShowItemProperty(Transform parent, ConfItem confItem, string propertyID, string propertyType,
        object propertyValue)
    {
        var nodeProperty = Instantiate(nodePropertyPrefab).GetComponent<UIPropertyNodeProperty>();
        nodeProperty.transform.SetParent(parent, false);
        nodeProperty.propertyID = propertyID;
        nodeProperty.propertyType = propertyType;
        nodeProperty.propertyValue = propertyValue;
        nodeProperty.tagConfItem = confItem;
        nodeProperty.baseProperty = nodeProperty;
        switch (propertyType)
        {
            case "int":
                var nodeInputInt = Instantiate(nodeInputPrefab).GetComponent<UIPropertyNodeInput>();
                nodeInputInt.transform.SetParent(nodeProperty.transform, false);
                nodeInputInt.InputType = UIPropertyType.Int;
                nodeInputInt.baseProperty = nodeProperty;

                nodeProperty.bindNode = nodeInputInt;
                break;
            case "float":
                var nodeInputFloat = Instantiate(nodeInputPrefab).GetComponent<UIPropertyNodeInput>();
                nodeInputFloat.transform.SetParent(nodeProperty.transform, false);
                nodeInputFloat.InputType = UIPropertyType.Float;
                nodeInputFloat.baseProperty = nodeProperty;

                nodeProperty.bindNode = nodeInputFloat;
                break;
            case "string":
                var nodeInputString = Instantiate(nodeInputPrefab).GetComponent<UIPropertyNodeInput>();
                nodeInputString.transform.SetParent(nodeProperty.transform, false);
                nodeInputString.InputType = UIPropertyType.String;
                nodeInputString.baseProperty = nodeProperty;

                nodeProperty.bindNode = nodeInputString;
                break;
            case "intArray":
                var nodeListInt = Instantiate(nodeListPrefab).GetComponent<UIPropertyNodeList>();
                nodeListInt.itemType = UIPropertyType.Int;
                nodeListInt.transform.SetParent(nodeProperty.transform, false);
                nodeListInt.baseProperty = nodeProperty;
                
                nodeProperty.bindNode = nodeListInt;
                break;
            case "stringArray":
                var nodeListString = Instantiate(nodeListPrefab).GetComponent<UIPropertyNodeList>();
                nodeListString.itemType = UIPropertyType.String;
                nodeListString.transform.SetParent(nodeProperty.transform, false);
                nodeListString.baseProperty = nodeProperty;

                nodeProperty.bindNode = nodeListString;
                break;
            case "intArrayArray":
                var nodeListInt2 = Instantiate(nodeListPrefab).GetComponent<UIPropertyNodeList>();
                nodeListInt2.itemType = UIPropertyType.IntArray;
                nodeListInt2.transform.SetParent(nodeProperty.transform, false);
                nodeListInt2.baseProperty = nodeProperty;
                
                nodeProperty.bindNode = nodeListInt2;
                break;
            case "floatArrayArray":
                var nodeListFloat2 = Instantiate(nodeListPrefab).GetComponent<UIPropertyNodeList>();
                nodeListFloat2.itemType = UIPropertyType.FloatArray;
                nodeListFloat2.transform.SetParent(nodeProperty.transform, false);
                nodeListFloat2.baseProperty = nodeProperty;

                nodeProperty.bindNode = nodeListFloat2;
                break;
            case "stringArrayArray":
                var nodeListString2 = Instantiate(nodeListPrefab).GetComponent<UIPropertyNodeList>();
                nodeListString2.itemType = UIPropertyType.StringArray;
                nodeListString2.transform.SetParent(nodeProperty.transform, false);
                nodeListString2.baseProperty = nodeProperty;
                
                nodeProperty.bindNode = nodeListString2;
                break;
            default:
                Debug.LogWarning($"{propertyID} 为无法解析的类型：{propertyType}");
                break;
        }

        nodeProperty.SetAllProperty();
    }
}