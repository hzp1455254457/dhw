


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
public class GuideManager : MonoBehaviour
{
    
    public static GuideManager Instance;
 public   GuideMask guideMask;
 private GuideMask sanDMask;
    private void Awake()
    {
        Instance = this;
    


        GetisFirstGame();
    }

  
    private void Start()
    {

        if (!isFirstGame)
        {
            enabled = false;
            CameraManeger.Instance.guideCamera.gameObject.SetActive(false);
        }
        else
        {
            if (MainUI.Instance.IsRedShow)
            {
                CameraManeger.Instance.guideCamera.gameObject.SetActive(false);
                AndroidHelper.Instance.UploadDataEvent("jiaocheng_1");
                SetisFistGame();
                guideMask = Global.FindChild<GuideMask>(transform, "mask");
                sanDMask = Global.FindChild<GuideMask>(GameObject.Find("ÊÕÒøÌ¨").transform, "mask");
                GuideEvent();

            }
            else
            {
                isFirstGame = false;
            }
        }

    }
    int step = 0;
    GameObject time;
    GameObject guide;
    GameObject tips;
    PeopleControl peopleControl;
    
   public void GuideEvent()
    {
        switch (step)
        {
            case 0:
                SetGuide(false);
                guideMask.ShowMask(0.5f, 1.5f);
                Camera.main.transform.DOMoveX(-UICameraControl.Size, 1.5F);
                StartCoroutine(Global.Delay(2.1f, () => { guideMask.inner_trans = Global.FindChild<RectTransform>(transform, "target"); }));
                step++;
                break;
            case 1:
                time.SetActive(true);
                step++;
                peopleControl = PeopleManager.Instance.peopleControls[0];
                peopleControl.GetComponentInChildren<Collider2D>().enabled = true;
               
                CreactTips(new Vector3(peopleControl.peopleUI.target.transform.position.x+0.7f, peopleControl.peopleUI.target.transform.position.y,0));

                guide = Global.FindChild(transform, "guide");
                guide.SetActive(true);
                AndroidHelper.Instance.UploadDataEvent("jiaocheng_2");
                break;
            case 2:
                if (GuideManager.Instance.isFirstGame)
                {
                    AndroidHelper.Instance.UploadDataEvent("jiaocheng_3");
                }
                HideTips();
                guide.SetActive(false);
                guideMask.HideMask();
                Time.timeScale = 4;
                MoveCamera();
                step++;
                break;
            case 3:
                step++;
                Time.timeScale = 1;
                peopleControl.baseFSM.StopMove();
                CameraManeger.Instance.guideCamera.gameObject.SetActive(true);
                CameraManeger.Instance.guideCamera.transform.position = Camera.main.transform.position;
                guideMask.ShowMask(0.5f, 0.5f);

                break;
            case 4:
                step++;
                Time.timeScale = 1;
                peopleControl.baseFSM.MoveToTarget(PosManager.Instance.exitTransForm.position, 1, 1);
                CameraManeger.Instance.guideCamera.gameObject.SetActive(false);
                AndroidHelper.Instance.UploadDataEvent("jiaocheng_4");
                guideMask.HideMask();
                HideTips();
                break;
            case 5:
                step++;
                sanDMask.ShowMask(0.5F,0.5F, null,GameObject.Find("show").GetComponent<RectTransform>());
               
                Global.FindChild(transform, "botton").SetActive(true);
                Global.FindChild(transform, "Guidetips").SetActive(true);
                break;
            default:
                break;
        }
    }

    private void SetGuide(bool value)
    {
        MainUI.Instance.tixianBt.gameObject.SetActive(value);
        time = Global.FindChild(transform, "chuandanGO");
        Global.FindChild(transform, "sevenIcon").SetActive(value);
        time.SetActive(value);
        Global.FindChild(transform, "botton").SetActive(value);
        Global.FindChild(transform, "keliuBt").SetActive(value);
        Camera.main.GetComponent<MGS.UCamera.MouseTranslate>().enabled = value;
        var list = PosManager.Instance.GetTanWeis().FindAll(s => s.GetLockStatus() == 0);
        foreach (var item in list)
        {
            if (value)
            {
               if(item.shop_id == 4)
                {
                    CreactTips1(item.transform.position);
                    tips.layer = 6;
                    item.SetGuide();
                }
            }
            item.gameObject.SetActive(value);
        }
    }

    bool IsMove = false;
    Camera mainCamera;
    public void MoveCamera()
    {
        mainCamera = Camera.main;
        IsMove = true;
    }
    private void Update()
    {
        if (IsMove)
        {
            float x = Mathf.Clamp(peopleControl.GetPeopleTf().transform.position.x, -UICameraControl.Size, UICameraControl.Size);
            Vector3 vector3 = new Vector3(x, 0, -10);
            
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, vector3, 2* Time.deltaTime);
        }
    }
    public void CreactTips(Vector3 vector)
    {
        tips = GameObjectPool.Instance.CreateObject("tips", ResourceManager.Instance.GetProGo("tips"), PanelMgr.Instance.canvas.transform, Quaternion.identity);
        tips.transform.localScale = Vector3.one*0.7f ;
        Vector3 pos = Camera.main.WorldToScreenPoint(vector);
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(PanelMgr.Instance.canvas.GetComponent<RectTransform>(), pos, CameraManeger.Instance.uiCamera, out worldPoint))
        {
            
            tips.transform.position = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z);
        }
    }
    public void CreactTips1(Vector3 vector)
    {
        tips = GameObjectPool.Instance.CreateObject("tips1", ResourceManager.Instance.GetProGo("tips1"), PanelMgr.Instance.canvas.transform, Quaternion.identity);
        tips.transform.localScale = Vector3.one * 50;
        tips.transform.position = vector;
        tips.layer = 8;
    }
    public void HideTips()
    {
        GameObjectPool.Instance.CollectObject(tips);
    }
    public void SetisFistGame()
    {

        DataSaver.Instance.SetInt("isGuide", 1);
    }
    public void SetMask(RectTransform rectTransform)
    {
        guideMask.inner_trans = rectTransform;
    }
    public bool isFirstGame = true;
    public void GetisFirstGame()
    {
        if (DataSaver.Instance.HasKey("isGuide") == false)
        {
            isFirstGame =true;

        }
        else
            isFirstGame = false;
    }

   
    public UnityAction achieveGuideAction;
    public void AchieveGuide()
    {
       

        enabled = false;
      
        achieveGuideAction?.Invoke();
        PanelMgr.Instance.OpenPanel<SevenLoginPanel>("SevenLoginPanel");
        AndroidHelper.Instance.UploadDataEvent("jiaocheng_tx_qd");
        SetGuide(true);
        IsMove = false;
        sanDMask.HideMask();
        Global.FindChild(transform, "Guidetips").SetActive(false);
        isFirstGame = false;
    }
  

}
