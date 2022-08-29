using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TanweiDaiJieSuoRedPanel : PanelBase
{
    public Text redText;
    public Button exitBt, getBt;
    public Image TanWeiImag;
    ConfigData.Tanwei tanwei;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "TanweiDaiJieSuoRedPanel";
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
        TanWeiImag=Global.FindChild<Image>(skinTrans, "TanWeiImag");
     
        tanwei = (ConfigData.Tanwei)(args[0]);
      
     int red=   tanwei.GetShopInfo().redCount;
        redText.text = string.Format("+{0}元", (red*MainUI.Instance.redScale).ToString("f2"));
;        //getsmallBt = Global.FindChild<Button>(skinTrans, "getsmallBt");
        exitBt.onClick.AddListener(CloseClick);
        getBt.onClick.AddListener(OnVideoClick);
        //getsmallBt.onClick.AddListener(OnSmallClick);
        Animation();
        AndroidHelper.Instance.UploadDataEvent("show_kaiye_hb");
    }
    #endregion
    public void CloseClick()
    {
        AndroidHelper.Instance.UploadDataEvent("close_kaiye_hb");
        Close();
    }
    public void OnVideoClick()
    {
        AndroidHelper.Instance.UploadDataEvent("click_kaiye_hb");
        AndroidHelper.Instance.ShowRewardVideo("摊位解锁", () => {

            tanwei.JieSuo();
            MainUI.Instance.AddRed((int)JavaCallUnity.Instance.GetAwardRedCount());
            MainUI.Instance.ShowPiaoChuan(()=> {


                MainUI.Instance.ShowPiaoChuan(null, (Sprite)args[1]
              , tanwei.GetShopInfo().shop_name+"已开业");


            }, "红包", "开业大红包 +" + (JavaCallUnity.Instance.GetAwardRedCount() * MainUI.Instance.redScale).ToString("f2") + "元");

            AndroidHelper.Instance.UploadDataEvent("finish_kaiye_hb");
            Close();

        });

    }
  
  


}
