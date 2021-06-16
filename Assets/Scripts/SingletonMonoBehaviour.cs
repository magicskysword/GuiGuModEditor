using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T instance;

    public static T Instance => instance;

    private void Awake()
    {
        instance = this as T;
    }
}