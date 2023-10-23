using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    public static T _instance;
    public static T Instance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
            _instance = (T)this;
        else
            Destroy(this);
    }

    private void OnDestory()
    {
        if (_instance = (T)this)
            _instance = null;
    }


}
