using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaCallUnity : MonoBehaviour
{
    public static JavaCallUnity Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RedCount = 0;
        ECPM = 50;
        TableECPM = 10;
    }
    private void Start()
    {
        AndroidHelper.Instance.unityReady();
    }
    public int RedCount { set; get; }
    bool isFirst=true;
    public void SetRedValue(string value)
    {
     
        int value1= int.Parse(value);
        LogHelper.DebugLog("���ú����ֵ" + value1);
        LogHelper.DebugLog("��ǰ�����ֵ" + RedCount);
     
        if (value1>= RedCount)
        {
           
            if (isFirst)
            {
                UnityActionManager.Instance.DispatchEvent<int, float>("ˢ�º�˺��������", value1, 0);
                isFirst = false;
            }
            else
            {
                //if (MainUI.Instance != null)
                //{
                //    MainUI.Instance.redValue = value1;

                //}
                 UnityActionManager.Instance.DispatchEvent<int, float>("ˢ�º�˺��������", value1, 1f);
            }
        }
        else
        {
            LogHelper.DebugLog("���ú����ֵ�۳�");
           
            UnityActionManager.Instance.DispatchEvent<int, float>("ˢ�º�˺��������", value1, 0);
        }
        RedCount = value1;
     
        
    }
  
    public float GetAwardRedCount()
    {
     return  (float) ECPM * 100 * 0.3f;
    }
    public float GetTableRedCount()
    {
        return (float)TableECPM * 100 * 0.3f;
    }
 double ECPM { set; get; }
    double TableECPM { set; get; }
    public void SendAwardMessageEvent(string value)
    {
      
        ECPM = double.Parse(value);
        Debug.Log("ecpm:" + JavaCallUnity.Instance.ECPM);
        ConfigData.DataManager.Instance.LoginData.AddCount();
        StartCoroutine(Global.Delay(0.01F, () =>
        {
            UnityActionManager.Instance.DispatchOnceEvent("������Ƶ�ص�");
        }));
       
    }

    public void SendTableMessageEvent(string value)
    {
        TableECPM = double.Parse(value);
        Debug.Log("ecpm:" + JavaCallUnity.Instance.TableECPM);
        StartCoroutine(Global.Delay(0.1F, () =>
        {
            UnityActionManager.Instance.DispatchOnceEvent("�����ص�");
        }));
       
    }
    public void SendFeedMessageEvent(string value)
    {
       
        UnityActionManager.Instance.DispatchOnceEvent("��Ϣ���ص�");
    }
    public void SendBannerMessageEvent(string value)
    {

        UnityActionManager.Instance.DispatchOnceEvent("banner�ص�");
    }
}