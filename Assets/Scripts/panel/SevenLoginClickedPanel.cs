using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenLoginClickedPanel : PanelBase
{
    public Text redText, countText;
    public Button exitBt, getBt;
    public Image iconImg;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "SevenLoginClickedPanel";
        layer = PanelLayer.Panel;
    }
    //RectTransform rectTransform;
   // float time = 0.8f;
    //public void SetTransForm()
    //{
    //    if (gameObject.activeInHierarchy)
    //    {
    //        if (rectTransform == null)
    //        {
    //            rectTransform = backTf.GetComponent<RectTransform>();
    //        }
    //        rectTransform.DOAnchorMax(new Vector2(0.5f, 1), time);
    //        rectTransform.DOAnchorMin(new Vector2(0.5f, 1), time);
    //        rectTransform.DOAnchorPosY(-20, time);
    //        rectTransform.DOPivotY(1, time);
          
    //    }
    //}
    private void ShowFeed()
    {
        AndroidHelper.Instance.ShowFeed( );
    }
    //public void RecoverTransForm()
    //{
    //    if (rectTransform == null)
    //    {
    //        rectTransform = backTf.GetComponent<RectTransform>();
    //    }
    //    rectTransform.anchorMin = new Vector2(0.5f, 0f);
    //    rectTransform.anchorMax = new Vector2(0.5f, 0f);
    //    rectTransform.pivot = new Vector2(0.5f, 0f);
    //    rectTransform.anchoredPosition = new Vector2(0, 400);
    //    // rectTransform.DOAnchorPosY(-365, 1.3f);
    //}
    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        backTf = Global.FindChild<Transform>(skinTrans, "back");
        redText= Global.FindChild<Text>(skinTrans, "redText");
        countText = Global.FindChild<Text>(skinTrans, "countText");
        exitBt = Global.FindChild<Button>(skinTrans, "exitBt");
        getBt = Global.FindChild<Button>(skinTrans, "getBt");
        iconImg= Global.FindChild<Image>(skinTrans, "iconImg");
        //getsmallBt = Global.FindChild<Button>(skinTrans, "getsmallBt");
        exitBt.onClick.AddListener(CloseClick);
        getBt.onClick.AddListener(OnVideoClick);
        //getsmallBt.onClick.AddListener(OnSmallClick);
        redText.text = ((int)(args[0])).ToString() + "元";
        iconImg.sprite = ResourceManager.Instance.GetSprite("红包-"+ (int)(args[1]));
        iconImg.SetNativeSize();
        countText.text = string.Format("昨日累积观看了<size=70><color=#FF2E47>{0}</color></size>次广告", (int)(args[0]));
      Animation();
        ShowFeed();
    }

  
    #endregion
    public void CloseClick()
    {
        GerAward();
    }

    private void GerAward()
    {
        MainUI.Instance.AddRed((int)((int)(args[0])/MainUI.Instance.redScale));
        // MainUI.Instance.AddGold((int)(args[1]));
        MainUI.Instance.ShowPiaoChuan(null, "红包", string.Format("+{0}元", (int)(args[0])));
        AndroidHelper.Instance.CloseFeed();

        Close();
    }

    public void OnVideoClick()
    {


        GerAward();
    


        }
  


}
