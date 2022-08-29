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
       isEdit = true;//���߰�׿���߼�
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
    //    if (GUILayout.Button("չʾbanner"))
    //    {
    //        ShowBanner();
    //    }
    //    if (GUILayout.Button("�ر�banner"))
    //    {
    //        CloseBanner();
    //    }
   
    //    if (GUILayout.Button("��������"))
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
            //UnityActionManager.Instance.DispatchOnceEvent("������Ƶ�ص�");
        }
        else
        {
            adClass.CallStatic("doBannerContainer",true);
        }
       
    }

    public void ShowFeed(Action callback = null)
    {
        if (callback != null)
            UnityActionManager.Instance.AddOnceAction("��Ϣ���ص�", callback);
        if (isEdit)
        {
            JavaCallUnity.Instance.SendFeedMessageEvent("");
            //UnityActionManager.Instance.DispatchOnceEvent("��Ϣ���ص�");
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
            UnityActionManager.Instance.AddOnceAction("������Ƶ�ص�", callback);
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
        UnityActionManager.Instance.AddOnceAction("�����ص�", callback);
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
        AudioManager.Instance.PlaySound("�����б�");
        if (isEdit)
        {
            Debug.Log("������ҳ��");
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
            Debug.Log("���Ӻ����");
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
            LogHelper.DebugLog("���" + eventName);
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
            LogHelper.DebugLog("�̳̺������");
        }
        else
        {
            AndroidUnityHelper.CallStatic("getCourseRewardRP");
            
           
        }
    }
}


