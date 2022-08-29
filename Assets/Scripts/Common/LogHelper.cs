using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogHelper 
{
    public static bool IsShowLog = true;
    public static void DebugLog(object message)
    {if(IsShowLog)
        Debug.Log("hzp:" + message);
    }
}
