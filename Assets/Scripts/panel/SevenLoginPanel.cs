using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenLoginPanel : PanelBase
{
   // public Text redText, countText;
    public Button exitBt;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "SevenLoginPanel";
        layer = PanelLayer.Panel;
    }

   
    public SevenLoginItem[] sevenLoginItems;
   
    public GameObject guideGo;
    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        backTf = Global.FindChild<Transform>(skinTrans, "back");
      
        exitBt = Global.FindChild<Button>(skinTrans, "exitBt");
      
     
        exitBt.onClick.AddListener(HideUI);
      
     
        Animation();
        InitUI();
    }


    #endregion

   
   
    public  void Hide()
    {
        Close();
      
      
    }
  
  
    public void HideUI()
    {
        
        AndroidHelper.Instance.ShowTableVideo("0");
        AndroidHelper.Instance.CloseBanner();
        Hide();
    }
  

    private void InitUI()
    {
        sevenLoginItems = GetComponentsInChildren<SevenLoginItem>();
        int i = 0;
        foreach (var item in sevenLoginItems)
        {
            item.index = i;
           item.sevenLoginData = ConfigData.DataManager.Instance.LoginData.sevenLoginDatas[i++];
            item.Init();
        }
    }
 public void   ShowGuide(bool value)
    {

    }

   public bool IsGet
    {
        set {
            ConfigData.DataManager.Instance.LoginData.IsGet = value;
            UnityActionManager.Instance.DispatchEvent("RefreshSevenIcon");
           
        }
        get { return ConfigData.DataManager.Instance.LoginData.IsGet; }
    }

    public int lastCount { get {
          return  ConfigData.DataManager.Instance.LoginData.lastCount;
        } }
    public int currentCount { get {
            return  ConfigData.DataManager.Instance.LoginData.currentCount;
        }
   
    }
  
 
 
   
  
}
