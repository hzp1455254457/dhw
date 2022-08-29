using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidHelper : MonoSingleton<AndroidHelper>, IAdInterface
{
    AndroidJavaObject adObject;
    AndroidJavaClass adClass;
    AndroidJavaClass AndroidUnityHelper;
  public bool isEdit { set; get; }
    public override void Init()
    {
        base.Init();
#if UNITY_EDITOR
        isEdit = true;

#else
isEdit = false;

#endif
       isEdit = true;//不走安卓得逻辑
        if (!isEdit)
        {
            adClass = new AndroidJavaClass("com.unity3d.player.ADHelper");
            AndroidUnityHelper = new AndroidJavaClass("com.unity3d.player.AndroidUnityHelper");


        }
        isShowTabled = true;
    }
    public void unityReady()
    {
        if (!isEdit)
        {
            AndroidUnityHelper.CallStatic("unityReady");
        }
    }
    public void CloseBanner()
    {
        if (!isEdit)
        {
            adClass.CallStatic("doBannerContainer",false);
        }
    }
    int day = 10;
    //public void OnGUI()
    //{
    //    if (GUILayout.Button("展示banner"))
    //    {
    //        ShowBanner();
    //    }
    //    if (GUILayout.Button("关闭banner"))
    //    {
    //        CloseBanner();
    //    }
   
    //    if (GUILayout.Button("设置天数"))
    //    {
    //        ConfigData.DataManager.Instance.SetSevenLoginDate(day++);
    //    }
    //}
    public void CloseFeed()
    {
        if (!isEdit)
        {
            adClass.CallStatic("dismissFeed");
        }
    }

    public void CloseFullVideo()
    {
       
    }

    public void CloseRewardVideo()
    {
      
    }

    public void CloseTableVideo()
    {
       
    }

    public void ShowBanner()
    {
        if (isEdit)
        {
            //UnityActionManager.Instance.DispatchOnceEvent("激励视频回调");
        }
        else
        {
            adClass.CallStatic("doBannerContainer",true);
        }
       
    }

    public void ShowFeed(Action callback = null)
    {
        if (callback != null)
            UnityActionManager.Instance.AddOnceAction("信息流回调", callback);
        if (isEdit)
        {
            JavaCallUnity.Instance.SendFeedMessageEvent("");
            //UnityActionManager.Instance.DispatchOnceEvent("信息流回调");
        }
        else
        {
            adClass.CallStatic("showFeedAd");
        }
    }

    public void ShowFullVideo(Action callback = null)
    {
      
    }

    public void ShowRewardVideo(string tag, Action callback = null)
    {
        if (callback != null)
            UnityActionManager.Instance.AddOnceAction("激励视频回调", callback);
        if (isEdit)
        {
            JavaCallUnity.Instance.SendAwardMessageEvent("100");
        }
        else
        {
            adClass.CallStatic("showRewardFromUnity", "JavaCallUnity", "SendAwardMessageEvent");
        }
        
    }
    public static bool isShowTabled { get; set; }
    public bool TableIsLoaded()
    {
        if (isEdit)
        {
            return true;
        }
        else
        {
            
         bool value=   adClass.CallStatic<bool>("tableVideoReady");
            Debug.Log("TableIsLoaded:" + value);
            return value;
        }
        return false;
    }
    public void ShowTableVideo(string tag,Action callback = null)
    {
       
        if (callback!=null)
        UnityActionManager.Instance.AddOnceAction("插屏回调", callback);
        if (isEdit)
        {
            JavaCallUnity.Instance.SendTableMessageEvent("20");
        }
        else
        {
            adClass.CallStatic("showTableVideo", tag);
        }
    }

    public void showWithDrawDialog()
    {
        AudioManager.Instance.PlaySound("提现列表");
        if (isEdit)
        {
            Debug.Log("打开提现页面");
        }
        else
        {
            AndroidUnityHelper.CallStatic("showWithDrawDialog");
        }
    }
    public void requestAddScoreHBQ(int score)
    {
        if (isEdit)
        {
            Debug.Log("增加红包卷");
        }
        else
        {
            AndroidUnityHelper.CallStatic("requestAddScoreHBQ",score);
        }
    }
    public void UploadDataEvent(string eventName)
    {
        if (isEdit)
        {
            LogHelper.DebugLog("埋点" + eventName);
        }
        else
        {
            AndroidUnityHelper.CallStatic("uploadDataEvent", eventName);
        }
          
        
    }
    public bool RewardRPState()
    {
        if (isEdit)
        {
            return false;
        }
        else
        {
         bool value=   AndroidUnityHelper.CallStatic<bool>("rewardRPState");
            LogHelper.DebugLog("RewardRPState" + value);
            return value;
        }
    }
    public  void getCourseRewardRP()
    {
        if (isEdit)
        {
            LogHelper.DebugLog("教程红包奖励");
        }
        else
        {
            AndroidUnityHelper.CallStatic("getCourseRewardRP");
            
           
        }
    }
}


