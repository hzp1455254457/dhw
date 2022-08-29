using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShouYinPanel : PanelBase
{
    public Text redText;
    public Button exitBt, getBt, getsmallBt;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "ShouYinPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        backTf = Global.FindChild<Transform>(skinTrans, "back");
        redText= Global.FindChild<Text>(skinTrans, "redText");
      
        exitBt = Global.FindChild<Button>(skinTrans, "exitBt");
        getBt = Global.FindChild<Button>(skinTrans, "getBt");
        getsmallBt = Global.FindChild<Button>(skinTrans, "getsmallBt");
        exitBt.onClick.AddListener(CloseClick);
        getBt.onClick.AddListener(OnVideoClick);
        getsmallBt.onClick.AddListener(OnSmallClick);
        getsmallBt.gameObject.SetActive(false);
        float redValue = Random.Range(30f, 50f);
       redText.text = redValue.ToString("f2") + "元";
        if (GuideManager.Instance.isFirstGame)
        {
            AndroidHelper.Instance.UploadDataEvent("jiaocheng_5");
        }
        else
        {
            AndroidHelper.Instance.UploadDataEvent("show_shouyintai_hb");
        }
        AudioManager.Instance.PlaySound("show_redpacket");
        Animation();
    }
    #endregion
    public void CloseClick()
    {
        Close();
        if (GuideManager.Instance.isFirstGame)
        {
            GuideManager.Instance.AchieveGuide();
            AndroidHelper.Instance.UploadDataEvent("jiaocheng_hb_close");
        }
        else
        {
            AndroidHelper.Instance.UploadDataEvent("click_shouyintai_hb_close");
        }
    }
    public void OnVideoClick()
    {
        if (GuideManager.Instance.isFirstGame)
        {
            AndroidHelper.Instance.UploadDataEvent("jiaocheng_6_clicklingqu");
        }
        else
        {
            AndroidHelper.Instance.UploadDataEvent("start_shouyintai_hb_get");
        }
        AndroidHelper.Instance.ShowRewardVideo("收银台", ()=> {
          
            //MainUI.Instance.AddRed((int)(JavaCallUnity.Instance.ECPM * 100 * 0.3f));
            //MainUI.Instance.AddGold(MainUI.Instance.shouYinGold);
            PanelMgr.Instance.OpenPanel<ShouYinClickedPanel>("ShouYinClickedPanel", (JavaCallUnity.Instance.GetAwardRedCount()*MainUI.Instance.redScale), Random.Range(100,1000));
          
            MainUI.Instance.shouYinGold = 0;
            MainUI.Instance.shouyinCount = 0;
            PeopleManager.Instance.SetZero();
            Close();
        });

    }
    public void OnSmallClick()
    {
        if (GuideManager.Instance.isFirstGame)
        {
            AndroidHelper.Instance.UploadDataEvent("jiaocheng_hb_0.1");
            GuideManager.Instance.AchieveGuide();
        }
        else
        {
            AndroidHelper.Instance.UploadDataEvent("click_shouyintai_hb_001");
        }
        MainUI.Instance.AddGold(100);
        MainUI.Instance.shouYinGold = 0;
        MainUI.Instance.shouyinCount = 0;
        PeopleManager.Instance.SetZero();
        Close();
    }
  


}
