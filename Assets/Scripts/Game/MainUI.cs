using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConfigData;
using DG.Tweening;
using Spine.Unity;

public class MainUI : UIBase
{
    public static MainUI Instance;
    #region 顶部ui
    public GameObject redGo;//红包元素
    public Button tixianBt;//提现按钮
    public Text goldText,redText;
     NumberEffect gold;
    NumberEffect1 red;
    public int goldValue;
   public int redValue { get; set; }
    public float redScale = 0.001f;
    public int shouYinGold { set; get; }
    public bool IsRedShow { get { return IsShowRed; } }
   public bool isGetReded { get; set; }//是否获取过红包卷
    public Transform bornTf;//出生点
    public Transform targetTf;//目标点
    public void AddChuanDanCount(int value)
    {
        if (value + ChuanDan > countMax)
        {
            ChuanDan = countMax;
        }
        else
        {
            ChuanDan += value;
        }
    }
    public void StopTiming()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            TimedAction();
        }
    }
    public void SetGetReded()
    {
        isGetReded = true;
    }
    public void AddRed(int value)
    {
        if (!IsShowRed) return;
        AndroidHelper.Instance.requestAddScoreHBQ(value);
        if (AndroidHelper.Instance.isEdit)
        {
           StartCoroutine( Global.Delay(0.1f, () => { RefreshRed(redValue + value, 1f); })); }

    }

    public void RefreshRed(int value, float time)
    {
        if (!IsShowRed) return;
        red.Animation(value * redScale, "", "元", time, redValue * redScale);
        redValue = value;
        
    }
   
    public void AddGold(int value)
    {
        RefreshGold(goldValue+ value, 0.5f);
       
    }
    public void RefreshGold(int value, float time)
    {
        gold.Animation(value, "", "", time, goldValue);
        goldValue = value;
        DataManager.Instance.coinNumber =  value;
    }
    #endregion
    #region 中部ui
    public Text Timetext;//传单计时
    public Text countText;//传单数量
    public GameObject chuandanGO;//传单UI
    public GameObject timeGO;//计时ui

    private void RefreshChuanDanText(int value, int max)
    {
        countText.text = string.Format("{0}/{1}", value, max);
    }

    bool isTime = false;
  public  int time { set { Time = value;
            DataManager.Instance.gameUserDataConfig.chuanTime = value;
        } get { return Time; } }
    int Time;
     int countMax = 100;
     int timeMax = 100;
    private int chuanDan;
    public int ChuanDan
    {
        set
        {
            DataManager.Instance.chuanDanNumber = value;
            chuanDan = value;
            UnityActionManager.Instance.DispatchEvent<int, int>("刷新传单数量", value, countMax);
            if (value <= 0)
            {
                StartTime();
            }
        }
        get { return chuanDan; }
    }
    Coroutine coroutine;
    public void StartTime()
    {
        if (!isTime)
        {

            chuandanGO.SetActive(false);
            
            timeGO.SetActive(true);
            isTime = true;
            int times = time > 0 ? time : timeMax;
            coroutine = StartCoroutine(Timing(Timetext, () =>
            {
                ChuanDan = countMax;
                TimedAction();
              
            }, times));

        }
    }

    public  IEnumerator Timing(Text text, UnityEngine.Events.UnityAction action, int time1 = 180)
    {
        time = time1;
        text.text = string.Format("{0}",Global. GetMinuteTime(time));
        while (time >= 0)
        {
            yield return new WaitForSeconds(1f);
            time--;
            text.text = string.Format("{0}", Global.GetMinuteTime(time));
            
        }
        time = 0;
        text.text = string.Format("{0}", Global.GetMinuteTime(time));
        if (action != null)
        {
            action();
        }
    }
    private void TimedAction()
    {
        isTime = false;
        chuandanGO.SetActive(true);
     
        timeGO.SetActive(false);
        coroutine = null;
        time = timeMax;
        isGetReded = false;
    }

    #endregion
    #region 底部ui
   // public Button shouyinBt;
    public Slider shouyinSlider1, shouyinSlider2;
   public Button shouYinBt, shouYinBt1;
    public SkeletonGraphic shouyinAnim;
    Tweener shouyinTweener1;
    Tweener shouyinTweener2;
    public CanvasGroup shouyinTipscanvasGroup;
    public int shouyinCount { set {
            ShouyinCount = value;
            AddShouYinValue();
            ConfigData.DataManager.Instance.gameUserDataConfig.shouYinCount=value;
        } get { return ShouyinCount; } }
    int ShouyinCount;
    public int shouyinMax { set; get; }
    bool isAniming = false;//收银台提示动画

    public void SetZeroShouYin()
    {

        if (isAniming) return;
    
        if (IsShowRed)
        {
            PanelMgr.Instance.OpenPanel<ShouYinPanel>("ShouYinPanel", shouYinGold);
         
           
        }
        else
        {
           
            AddGold(100);
            shouYinGold = 0;
            MainUI.Instance.shouyinCount = 0;
            PeopleManager.Instance.SetZero();
        }
    }
    private void EventString(bool botton)
    {if(botton)
        AndroidHelper.Instance.UploadDataEvent("click_shouyintai_icon_btn");
        else
        {
            AndroidHelper.Instance.UploadDataEvent("click_shouyintai_btn_with_exit");
        }
    }
    bool isInit = true;
    public void AddShouYinValue()
    {

       float value= shouyinCount / (float)shouyinMax;

        if (value >= 1)
        {

            shouyinAnim.AnimationState.SetAnimation(0, "manle", true);
            isAniming = true;
            shouyinTipscanvasGroup.DOFade(1, 0.5f).onComplete = () => { isAniming = false; };
            ShouYinTips();

        }
        else if (value == 0)
        {
            shouyinAnim.AnimationState.SetAnimation(0, "daiji", false);
            shouyinTipscanvasGroup.alpha = 0;
        }
        else
        {
            shouyinAnim.AnimationState.SetAnimation(0, "zengjiajindushi", false);
        }
        if (shouyinTweener1 != null)
        {
            shouyinTweener1.Kill();
            shouyinTweener2.Kill();
        }
        float time = isInit == true ? 0f : 0.2f;
        isInit = false;
        shouyinTweener1 = shouyinSlider1.DOValue(value, time);
        shouyinTweener2 = shouyinSlider2.DOValue(value, time);
      
        //shouyinSlider1.value = value;
        //shouyinSlider2.value = value;
    }
    //private void OnGUI()
    //{
    //    if (GUILayout.Button("测试"))
    //    {
    //        ShouYinTips();
    //    }
    //}
    public void ShowPiaoChuan( UnityEngine.Events.UnityAction unityAction=null,string spriteName="红包", params string[] value)
    {
        TipsShowBase.Instance.Show("TipsShow4", MainUI.Instance.bornTf, MainUI.Instance.targetTf, new Sprite[] { 
        ResourceManager.Instance.GetSprite(spriteName)
        }, null, unityAction, 1.5f, value);
    }
    public void ShowPiaoChuan(UnityEngine.Events.UnityAction unityAction = null, Sprite sprite=null, params string[] value)
    {
        TipsShowBase.Instance.Show("TipsShow4", MainUI.Instance.bornTf, MainUI.Instance.targetTf, new Sprite[] {
       sprite
        }, null, unityAction, 1.5f, value);
    }
    public void ShowPiaoChuanText(float scale=1,params string[] value)
    {
        TipsShowBase.Instance.Show("TipsShow2", MainUI.Instance.bornTf, targetTf, null, new Color[] { Color.black }, null, scale, value

        );
    }
    private void ShouYinTips()
    {
        TipsShowBase.Instance.Show("TipsShow2", MainUI.Instance.bornTf, targetTf, null, new Color[] { Color.black, Color.red }, null, 1.5f, "收银台装不下了!", "请尽快收起!"

        );
    }
    #endregion
    #region 左部ui
    public RectTransform ItemUI;
    public GameObject left;
    public Button leftBt;
    public List<Item> items=new List<Item>();
    public Text itemNameText;
   public void ShowItemUI(int id)
    {
     StartCoroutine(   CreactItem(id));
        itemNameText.text = Config.Instance.ShopList.Find(s => s.shop_id == id).shop_name;

        leftBt.interactable = false;
        left.gameObject.SetActive(true);
        ItemUI.DOAnchorPosX(-ItemUI.rect.width * 2, 0);
        ItemUI.DOAnchorPosX(0, 0.5f).SetEase(Ease.InOutSine).onComplete = () => { leftBt.interactable = true; };

    }

    private IEnumerator CreactItem(int id)
    {
        var list = Config.Instance.GoodsList.FindAll(s => s.shop_id == id);
        if (list != null)
        {

            for (int i = 0; i < list.Count; i++)
            {
                var item = GameObjectPool.Instance.CreateObject("item", ResourceManager.Instance.GetProGo("item"), ItemUI, Quaternion.identity).GetComponent<Item>();
                item.SetItem(ResourceManager.Instance.GetSprite(list[i].item_pic), list[i].item_name);
                items.Add(item);
                yield return 0;
            }
        }
    }

    public void HideItemUI()
    {
        ItemUI.DOAnchorPosX(-ItemUI.rect.width*2, 0.5f).SetEase(Ease.InOutSine).onComplete=()=> { left.gameObject.SetActive(false);
            for (int i = 0; i <items.Count; i++)
            {
                GameObjectPool.Instance.CollectObject(items[i].gameObject);
            }
            items.Clear();
        };
    }
    #endregion
    private void Awake()
    {
        Instance = this;
        Debug.LogError("初始化Mainui");
        InitAddAction();
        InitComponnent();
        SetRedStates(false);
        InitValue();
        if (ChuanDan > 0)
            TimedAction();
        else
        {

        }
    }
   
   
    private void InitAddAction()
    {
        UnityActionManager.Instance.AddAction<int>("刷新红包卷数量", AddRed);
        UnityActionManager.Instance.AddAction<int, float>("刷新后端红包卷数量", RefreshRed);
        UnityActionManager.Instance.AddAction<int>("刷新金币数量", AddGold);
        UnityActionManager.Instance.AddAction<int, int>("刷新传单数量", RefreshChuanDanText);
       
    }

    private void InitComponnent()
    {
        Timetext = Global.FindChild<Text>(transform, "time_Text");
        countText = Global.FindChild<Text>(transform, "count_Text");
        goldText = Global.FindChild<Text>(transform, "gold_Text");
        redText = Global.FindChild<Text>(transform, "red_Text");
        redGo= Global.FindChild(transform, "redGo");
        tixianBt = Global.FindChild<Button>(redGo.transform, "tixianBt");
        tixianBt.onClick.AddListener(() => {
            AndroidHelper.Instance.showWithDrawDialog();
        });
        chuandanGO = Global.FindChild(transform, "chuandanGO");
        ItemUI= Global.FindChild<RectTransform>(transform, "ItemUI");
        left= Global.FindChild(transform, "left");
        
        leftBt=Global.FindChild<Button>(transform, "leftBt");
     
        itemNameText= Global.FindChild<Text>(transform, "itemNameText");
        timeGO = Global.FindChild(transform, "timeGO");
        left.gameObject.SetActive(false);
        gold = goldText.GetComponent<NumberEffect>();
        red = redText.GetComponent<NumberEffect1>();
        shouyinSlider2= Global.FindChild<Slider>(transform, "shouyinSlider2");
      Transform shouyinTf=  GameObject.Find("收银台").transform;
        shouyinSlider1 = Global.FindChild<Slider>(shouyinTf, "shouyinSlider1");

        shouYinBt = Global.FindChild<Button>(transform, "ShouYinBt");
        shouYinBt1= Global.FindChild<Button>(shouyinTf, "ShouYinBt1");
        shouyinTipscanvasGroup= Global.FindChild<CanvasGroup>(shouyinTf, "shouyinTipscanvasGroup");
        shouyinAnim = Global.FindChild<SkeletonGraphic>(shouYinBt.transform, "shouyinAnim");
        shouYinBt.onClick.AddListener(SetZeroShouYin);
        shouYinBt.onClick.AddListener(()=>EventString(true));
        shouYinBt1.onClick.AddListener(SetZeroShouYin);
        shouYinBt1.onClick.AddListener(() => EventString(false));
        leftBt.onClick.AddListener(HideItemUI);
        bornTf = Global.FindChild<Transform>(PanelMgr.Instance.GetLayer(PanelLayer.Tips), "born");
        targetTf = Global.FindChild<Transform>(PanelMgr.Instance.GetLayer(PanelLayer.Tips), "target");
    }

    private void InitValue()
    {
      time=DataManager.Instance.gameUserDataConfig.chuanTime;
        ChuanDan = DataManager.Instance.chuanDanNumber;
        if (ChuanDan <= 0)
        {
            SetGetReded();
        }
        RefreshRed(JavaCallUnity.Instance.RedCount, 0);
       
        RefreshGold(ConfigData.DataManager.Instance.coinNumber, 0);
        shouyinMax = 10;
        shouyinCount = ConfigData.DataManager.Instance. gameUserDataConfig.shouYinCount;
       
    }

    public override void HideRed()
    {
        redGo.SetActive(false);
    }
}
