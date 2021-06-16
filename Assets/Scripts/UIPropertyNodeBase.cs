using System;
using UnityEngine;

public class UIPropertyNodeBase : MonoBehaviour
{
    public Action<object> onGetValue;
    public UIPropertyNodeProperty baseProperty;
}