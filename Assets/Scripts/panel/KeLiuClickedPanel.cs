using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeLiuClickedPanel : PanelBase
{
    public Text redText, countText;
    public Button exitBt, getBt;
    public Image image1;
    Tweener tweener1;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "KeLiuClickedPanel";
        layer = PanelLayer.Panel;
    }
   
  
  
    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        backTf = Global.FindChild<Transform>(skinTrans, "back");
        redText= Global.FindChild<Text>(skinTrans, "redText");
        countText = Global.FindChild<Text>(skinTrans, "countText");
        exitBt = Global.FindChild<Button>(skinTrans, "exitBt");
        getBt = Global.FindChild<Button>(skinTrans, "getBt");
        image1= Global.FindChild<Image>(skinTrans, "image1");  
        UnityActionManager.Instance.AddAction<int>("Refreshkeliu", RefreshStutes);
        RefreshStutes(ConfigData.DataManager.Instance.gameUserDataConfig.achivePeopleCount);
        SetImageValue(ConfigData.DataManager.Instance.gameUserDataConfig.achivePeopleCount, false);
        //getsmallBt = Global.FindChild<Button>(skinTrans, "getsmallBt");
        exitBt.onClick.AddListener(CloseClick);
        getBt.onClick.AddListener(OnVideoClick);
      Animation();
        
    }
    private void SetImageValue(int obj,bool isAnim)
    {
        float time = isAnim == true ? 0.5f : 0;
        if (tweener1 != null)
        {
            tweener1.Kill();
        }
      
        if (obj >= 301 && obj <= 1000)
        {
            tweener1 = image1.DOFillAmount((obj - 300) / 700f*(0.64f-0.36f)+0.36f, time);
           
        }
        else if (obj <= 300)
        {
            tweener1 = image1.DOFillAmount((obj) / 300f * 0.36f, time);


        }
        else if (obj > 1000)
        {
            tweener1 = image1.DOFillAmount((obj-1000) / 4000f*(1-0.64f)+0.64f, time);
        }
    }
    private void RefreshStutes(int obj)
    {
        countText.text = obj + "个";
        if (obj >= 301 && obj <= 1000)
        {
            redText.text = "红包加成600%";

        }
        else if (obj <= 300)
        {
            redText.text = "红包加成300%";
        }
        else if(obj > 1000)
        {
            redText.text = "红包加成900%";
        }
        SetImageValue(obj, true);
    }


    #endregion
    public void CloseClick()
    {
        GerAward();
    }

    private void GerAward()
    {
        UnityActionManager.Instance.RemoveAction<int>("Refreshkeliu", RefreshStutes);
        Close();
    }

    public void OnVideoClick()
    {


        GerAward();
    


        }
  


}
