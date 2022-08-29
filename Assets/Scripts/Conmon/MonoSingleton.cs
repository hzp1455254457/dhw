using UnityEngine;
using System.Collections;
using System;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T), true) as T;
                if (instance == null)
                {
                    instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
                    DontDestroyOnLoad(instance);
                    instance.Init();
                }
                else
                {
                    instance.Init();
                }
                if (instance == null)
                    Debug.Log("Failed to create instance of " + typeof(T).FullName + ".");
            }

            return instance;
        }

    }
    private void Awake()
    {
        //if(instance == null)
        //{
        //    instance = this as T;
        //    instance. Init();
        //}
    }
    public virtual void Init()
    {
        print("Init()+++" + typeof(T).Name);
        //Init();
    }
    private void OnDestroy()
    {
        if (instance != null) instance = null;

    }
    void OnApplicationQuit() { if (instance != null) instance = null; }

 
}
