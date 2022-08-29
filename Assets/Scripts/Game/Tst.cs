using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using LitJson;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Tst : MonoBehaviour
{
    // Start is called before the first frame update
    public int year, mouth, day, hour, min, sec;

     string timeURL = "http://api.m.taobao.com/rest/api3.do?api=mtop.common.getTimestamp";
    public Slider slider;
    
    void Start()
    {
        StartCoroutine(LoadGameScene());
        StartCoroutine(GetTime());
        Animation();
    }
    public void Animation()
    {
        slider.DOValue(1, 1.5f).onComplete=()=> {
            Debug.LogError("进度加载完成");
            LoadGame();
        };
    }
    bool isAchive = false;
   
    IEnumerator GetTime()
    {
       
        //Debug.LogError("当前时间:" + Time.realtimeSinceStartup);
        UnityWebRequest www = UnityWebRequest.Get(timeURL);
        //WWW www = new WWW(timeURL);
     
            yield return www.SendWebRequest();
        
    
        if (www.error != null)
        {
            Debug.LogError(www.error);
            Debug.LogError(www.downloadHandler.text);
            // StartCoroutine(GetTime());
            TimeManager.Sync((DateTime.Now.Ticks- new DateTime(1970, 1, 1, 8, 0, 0).Ticks)/10000);
            Debug.LogError("当前时间:" + TimeManager.GetSystemTime());
            //Debug.LogError("当前时间:" + Time.realtimeSinceStartup);
            
        }
        else
        {
            Debug.LogError(www.downloadHandler.text);
            JsonData jsonData = JsonMapper.ToObject<JsonData>(www.downloadHandler.text);
            long value = long.Parse(jsonData["data"]["t"].ToString());
            TimeManager.Sync(value);
            Debug.LogError("当前时间:" + TimeManager.GetSystemTime());
            //Debug.LogError("当前时间:" + Time.realtimeSinceStartup);
            ConfigData.DataManager.Instance.CheckLoginDayCount();
            //SplitTime(www.text);
        }
    
        //Debug.LogError(www.text);
        //SplitTime(www.text);
        LoadGame();
    }
    AsyncOperation asyncOperation;
    private void LoadGame()
    {
        if (!isAchive)
        {
            isAchive = true;
           
        }
        else
        {
            Debug.LogError("进度加载完成跳转场景");
            //SceneManager.LoadScene("Game");
            asyncOperation.allowSceneActivation = true;
        }
    }
    private IEnumerator LoadGameScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync("Game");
        asyncOperation.allowSceneActivation = false;
        //yield return asyncOperation;
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        


    }
    

}
