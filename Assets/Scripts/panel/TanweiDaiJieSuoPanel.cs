using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TanweiDaiJieSuoPanel : PanelBase
{
    public Text redText,nameText, jiesuoTimeText, goldText;
    public Button exitBt, getBt;
    public Image TanWeiImag;
    ConfigData.Tanwei tanwei;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "TanweiDaiJieSuoPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        backTf = Global.FindChild<Transform>(skinTrans, "back");
        redText= Global.FindChild<Text>(skinTrans, "redText");
        nameText= Global.FindChild<Text>(skinTrans, "nameText");
        jiesuoTimeText= Global.FindChild<Text>(skinTrans, "jiesuoTimeText");
        goldText = Global.FindChild<Text>(skinTrans, "goldText");
        exitBt = Global.FindChild<Button>(skinTrans, "exitBt");
        getBt = Global.FindChild<Button>(skinTrans, "getBt");
        TanWeiImag=Global.FindChild<Image>(skinTrans, "TanWeiImag");
        TanWeiImag.sprite = (Sprite)args[0];
        nameText.text = args[1].ToString();
        jiesuoTimeText.text ="解锁所需时间:"+ args[2].ToString()+"秒";
        goldText.text = string.Format("{0}金币解锁", (int)(args[3]));
        tanwei = (ConfigData.Tanwei)(args[4]);
        int red= Random.Range(20000, 50000);
        tanwei.GetShopInfo().redCount = red;
        redText.text = string.Format("解锁后 +{0}元", (red*MainUI.Instance.redScale).ToString("f2"));
;        //getsmallBt = Global.FindChild<Button>(skinTrans, "getsmallBt");
        exitBt.onClick.AddListener(CloseClick);
        getBt.onClick.AddListener(OnVideoClick);
        //getsmallBt.onClick.AddListener(OnSmallClick);
        Animation();
    }
    #endregion
    public void CloseClick()
    {
        AndroidHelper.Instance.UploadDataEvent("close_daibaitan");
        Close();
    }
    public void OnVideoClick()
    {
    
        if (MainUI.Instance.goldValue >= tanwei.GetShopInfo().unlock_money)
        { int value = MainUI.Instance.goldValue;
          int value1=  value - tanwei.GetShopInfo().unlock_money;
            tanwei.UnLockTanWei();
            MainUI.Instance.RefreshGold(value1,0.1f);
            AndroidHelper.Instance.UploadDataEvent("finish_daibaitan");
            Close();
        }
      

    }
  
  


}
