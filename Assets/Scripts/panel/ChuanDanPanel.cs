using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChuanDanPanel : PanelBase
{
    public Text redText, countText;
    public Button exitBt, getBt, getsmallBt;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "ChuanDanPanel";
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
        getsmallBt = Global.FindChild<Button>(skinTrans, "getsmallBt");
        exitBt.onClick.AddListener(CloseClick);
        getBt.onClick.AddListener(OnVideoClick);
        getsmallBt.onClick.AddListener(OnSmallClick);
        float redValue = Random.Range(30f, 50f);
        redText.text= redValue.ToString("f2")+"元";
        Animation();
        if ((bool)(args[0]))
        {
            getsmallBt.gameObject.SetActive(false);
        }
        AudioManager.Instance.PlaySound("show_redpacket");
        AndroidHelper.Instance.UploadDataEvent("show_chuandan_hb");
        
    }
    #endregion
    public void CloseClick()
    {
        AndroidHelper.Instance.UploadDataEvent("close_chuandan_hb");
        Close();
    }
    public void OnVideoClick()
    {
        AndroidHelper.Instance.UploadDataEvent("click_chuandan_hb");
        AndroidHelper.Instance.ShowRewardVideo("传单", ()=> {

         
            int count = Random.Range(1, 100);
            PanelMgr.Instance.OpenPanel<ChuanDanClickedPanel>("ChuanDanClickedPanel", JavaCallUnity.Instance.GetAwardRedCount() * MainUI.Instance.redScale, count);
            Close();
           
        });

    }
    public void OnSmallClick()
    {
        MainUI.Instance.AddRed(100);
        MainUI.Instance.SetGetReded();
        Close();
    }
  


}
