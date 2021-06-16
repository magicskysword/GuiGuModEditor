using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirtualList;

public class VirtualSelectList : IListSource
{
    public int Count => names.Count;
    public List<string> names = new List<string>();
    public Action<int> selectEvent = null;
    private int curSelect;
        
    public void SetItem(GameObject view, int index)
    {
        UINodeSelect nodeSelect = view.GetComponent<UINodeSelect>();
        UIMain uiMain = UIMain.Instance;
        nodeSelect.toggle.onValueChanged.RemoveAllListeners();
        nodeSelect.txtShow.text = names[index];
        nodeSelect.toggle.group = nodeSelect.transform.parent.GetComponent<ToggleGroup>();
        if (index == curSelect)
            nodeSelect.toggle.isOn = true;
        else
            nodeSelect.toggle.isOn = false;
        nodeSelect.toggle.onValueChanged.AddListener(trigger =>
        {
            if (trigger)
            {
                selectEvent(index);
                curSelect = index;
            }
        });
    }
}