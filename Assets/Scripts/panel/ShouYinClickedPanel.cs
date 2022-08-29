using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShouYinClickedPanel : PanelBase
{
    public Text redText, countText;
    public Button exitBt, getBt;
    #region 生命周期
    //初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "ShouYinClickedPanel";
        layer = PanelLayer.Panel;
    }
    RectTransform rectTransform;
    float time = 0.8f;
    public void SetTransForm()
    {
        if (gameObject.activeInHierarchy)
        {
            if (rectTransform == null)
            {
                rectTransform = backTf.GetComponent<RectTransform>();
            }
            rectTransform.DOAnchorMax(new Vector2(0.5f, 1), time);
            rectTransform.DOAnchorMin(new Vector2(0.5f, 1), time);
            rectTransform.DOAnchorPosY(-20, time);
            rectTransform.DOPivotY(1, time);
          
        }
    }
    private void ShowFeed()
    {
        AndroidHelper.Instance.ShowFeed( SetTransForm);
    }
    public void RecoverTransForm()
    {
        if (rectTransform == null)
        {
            rectTransform = backTf.GetComponent<RectTransform>();
        }
        rectTransform.anchorMin = new Vector2(0.5f, 0f);
        rectTransform.anchorMax = new Vector2(0.5f, 0f);
        rectTransform.pivot = new Vector2(0.5f, 0f);
        rectTransform.anchoredPosition = new Vector2(0, 400);
        // rectTransform.DOAnchorPosY(-365, 1.3f);
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
        //getsmallBt = Global.FindChild<Button>(skinTrans, "getsmallBt");
        exitBt.onClick.AddListener(CloseClick);
        getBt.onClick.AddListener(OnVideoClick);
        //getsmallBt.onClick.AddListener(OnSmallClick);
        redText.text = ((float)(args[0])).ToString("f2") + "元";
        countText.text= "+"+((int)(args[1])).ToString()+"个" ;
      Animation(ShowFeed);
        AudioManager.Instance.PlaySound("finish_redpacket");
    }

  
    #endregion
    public void CloseClick()
    {
        GerAward();
    }

    private void GerAward()
    {
        MainUI.Instance.AddRed((int)(JavaCallUnity.Instance.GetAwardRedCount()));
        MainUI.Instance.AddGold((int)(args[1]));
        TipsShowBase.Instance.Show("TipsShow3", MainUI.Instance.bornTf, MainUI.Instance.targetTf, new Sprite[] { ResourceManager.Instance.GetSprite("金币"),
        ResourceManager.Instance.GetSprite("红包")
        }, null, null, 1.5f, "+"+ ((int)(args[1])), "+"+ ((float)(args[0])).ToString("f2") + "元");
 
         AndroidHelper.Instance.CloseFeed();
        if (GuideManager.Instance.isFirstGame)
        {
            GuideManager.Instance.AchieveGuide();
        }
        else
        {
            AndroidHelper.Instance.UploadDataEvent("finish_shouyintai_hb");
        }
        Close();
    }

    public void OnVideoClick()
    {


        GerAward();
    


        }
  


}
