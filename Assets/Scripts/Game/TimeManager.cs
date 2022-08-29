using UnityEngine;
using System.Collections;
using System;

public class TimeManager :MonoBehaviour
{

    public static DateTime UtcNow { get { return DateTime.UtcNow.AddSeconds(DeltaTime); } }
    //public static DateTime Now { get { return DateTime.Now.AddSeconds(DeltaTime); } }

    private static double DeltaTime = 0;
    private static double ServerTime = 0;
    private static double ValidStartGameTime = 0;

    //同步服务器时间
    public static void Sync(long time)
    {
        ValidStartGameTime = Time.realtimeSinceStartup;
        DateTime dt =new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddMilliseconds((double)time);
       DeltaTime = (dt - DateTime.UtcNow).TotalSeconds;
        ServerTime = time;
    }

    //剩余时间
    public static TimeSpan GetLeftTime(long validTime)
    {
        TimeSpan ts = (new DateTime(1970, 1, 1).AddSeconds((double)validTime)).Subtract(UtcNow);
        return ts;
    }

    //当前服务器时间
    public static DateTime GetSystemTime()
    {
        DateTime dateTime = (new DateTime(1970, 1, 1).AddMilliseconds((double)ServerTime)).AddSeconds((double)(Time.realtimeSinceStartup - ValidStartGameTime)).AddHours(8);
        //Debug.Log("unity获取时间：" +dateTime);
        return dateTime;
    }
}