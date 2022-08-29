using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UnityActionManager 
{
    private static UnityActionManager _Instance;

    public static UnityActionManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new UnityActionManager();
            }
            return _Instance;
        }
    }
    private UnityActionManager()
    {

    }
    private Dictionary<string,List< Delegate>> dic = new Dictionary<string, List<Delegate>>();

    public void AddAction(string key,Action unityAction)
    {
        if (dic.ContainsKey(key))
        {
             dic[key].Add(unityAction);
        }
        else
        {
            var dele = new List<Delegate>();
            dic.Add(key, dele);
            dic[key].Add(unityAction);
        }
    }
    public void AddOnceAction(string key, Action unityAction)
    {
        if (dic.ContainsKey(key))
        {

            dic[key].Clear();
            dic[key].Add(unityAction);
        }
        else
        {
            var dele = new List<Delegate>();
            dic.Add(key, dele);
            dic[key].Add(unityAction);
        }
    }
    public void DispatchOnceEvent(string eventName)
    {
        if (dic.ContainsKey(eventName))
        {
            foreach (var item in dic[eventName])
            {
                item.DynamicInvoke();
            }
           CleanKey(eventName);
        }
        else
        {
            Debug.Log("UnityActionManager不存在" + eventName + "事件");
        }
    }
    public void AddAction<T>(string key, Action <T>unityAction)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(unityAction);
        }
        else
        {
            var dele = new List<Delegate>();
            dic.Add(key, dele);
            dic[key].Add(unityAction);
        }
    }
    public void AddAction<T,F>(string key, Action<T,F> unityAction)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(unityAction);
        }
        else
        {
            var dele = new List<Delegate>();
            dic.Add(key, dele);
            dic[key].Add(unityAction);
        }
    }
    public void AddAction<T,F,G>(string key, Action<T,F,G> unityAction)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(unityAction);
        }
        else
        {
            var dele = new List<Delegate>();
            dic.Add(key, dele);
            dic[key].Add(unityAction);
        }
    }
    public bool HaveAction<T>(string key)
    {
        if (dic.ContainsKey(key))
        {
            return true;
        }
        else { return false; }
    }
    public void RemoveAction(string key, Action callback)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Remove(callback);
            if(dic[key].Count<=0)
            dic.Remove(key);
        }
        else
        {
            Debug.Log("UnityActionManager不存在" + key + "事件");
        }

    }
    public void RemoveAction<T>(string key, Action<T> callback)
    {
        
        if (dic.ContainsKey(key))
        {
            dic[key].Remove(callback);
            if (dic[key].Count <= 0)
                dic.Remove(key);
        }
        else
        {
            Debug.Log("UnityActionManager不存在" + key + "事件");
        }

    }
    public void RemoveAction<T,K>(string key, Action<T,K> callback)
    {

        if (dic.ContainsKey(key))
        {
            dic[key].Remove(callback);
            if (dic[key].Count <= 0)
                dic.Remove(key);
        }
        else
        {
            Debug.Log("UnityActionManager不存在" + key + "事件");
        }

    }
    public void DispatchEvent(string eventName)
    {
        if (dic.ContainsKey(eventName))
        {
            foreach (var item in dic[eventName])
            {
                item.DynamicInvoke();
            }
            //if (once)
            //{
            //    RemoveAction(eventName);
            //}
        }
        else
        {
            //Debug.LogError("UnityActionManager不存在" + eventName + "事件");
        }
    }
    public void DispatchEvent<T>(string eventName, T arg)
    {
        if (dic.ContainsKey(eventName))
        {
            foreach (var item in dic[eventName])
            {
                item.DynamicInvoke(arg);
            }
            //if (once)
            //{
            //    RemoveAction(eventName);
            //}
        }
        else
        {
          //  Debug.LogError("UnityActionManager不存在" + eventName + "事件");
        }
    }
    public void DispatchEvent<T,F>(string eventName, T arg,F f)
    {
        if (dic.ContainsKey(eventName))
        {
            foreach (var item in dic[eventName])
            {
                item.DynamicInvoke(arg,f);
            }
            //if (once)
            //{
            //    RemoveAction(eventName);
            //}
        }
        else
        {
           // Debug.LogError("UnityActionManager不存在" + eventName + "事件");
        }
    }
    public void CleanKey(string key)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Clear();
        }
    }
    public void DispatchEvent<T, F, G>(string eventName, T arg,F f,G g)
    {
        if (dic.ContainsKey(eventName))
        {
            foreach (var item in dic[eventName])
            {
                item.DynamicInvoke(arg, f,g);
            }
            //if (once)
            //{
            //    RemoveAction(eventName);
            //}
        }
        else
        {
            //Debug.Log("UnityActionManager不存在" + eventName + "事件");
        }
    }
}
