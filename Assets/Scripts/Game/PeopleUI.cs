using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class PeopleUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public Slider slider;
    public CanvasGroup sliderCanvasGroup, qipaoCanvasGroup;
    public Image image,proImg;
    public Image image1;
    public Image biaoQingImg;
    public GameObject qipao1;
    public Text text,text1;
    Tweener sliderTweener;
    Tweener fadeTweener;
    Tweener fade1Tweener;
    bool isFading = false;
  public  RectTransform target;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        sliderCanvasGroup = slider.GetComponent<CanvasGroup>();
        qipaoCanvasGroup = image.GetComponent<CanvasGroup>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        text = Global.FindChild<Text>(transform, "Text");
        text1 = Global.FindChild<Text>(transform, "Text1");
        target= Global.FindChild<RectTransform>(transform, "target");
        proImg = image.transform.GetChild(0).GetComponent<Image>();
        gameObject.SetActive(false);
    }
    private void LateUpdate()
    {
        if (transform.lossyScale.x < 0)
        {
            canvas.transform.localScale = new Vector3(canvas.transform.localScale.x * -1, canvas.transform.localScale.y, 0);
        }
        else
        {
           
        }
    }
    public void SetUI(float value, UnityEngine.Events.UnityAction unityAction = null, UnityEngine.Events.UnityAction achiveAction = null)
    {
        if (!gameObject.activeInHierarchy)
        {

         
            gameObject.SetActive(true);
            
            
            sliderCanvasGroup.DOFade(0, 0f);
            fade1Tweener = sliderCanvasGroup.DOFade(1, 0.9f);
                fade1Tweener.onComplete = () =>
              {
                  fade1Tweener = null;
                  Anim(value, unityAction, achiveAction);
                 
              };
        }
        else
        {
           
            if (fadeTweener != null)
            {
                fadeTweener.Pause();
                fadeTweener.Kill();
              
                sliderCanvasGroup.DOFade(0, 0f);
                fade1Tweener = sliderCanvasGroup.DOFade(1, 0.9f);
                fade1Tweener.onComplete = () =>
                {
                    fade1Tweener = null;
                    Anim(value, unityAction, achiveAction);

                };
            }
            else
            {
                if (fade1Tweener != null)
                {
                    fade1Tweener.Kill();
                    sliderCanvasGroup.DOFade(1, 0f);
                }
                Anim(value, unityAction, achiveAction);
            }
           
        }
    }
    bool isFirst = true;
    private void Anim(float value, UnityAction unityAction, UnityAction achiveAction)
    {
        //Debug.LogError("Ö´ÐÐslide:"+value);
        if (sliderTweener != null)
        {
            sliderTweener.Pause();
            sliderTweener.Kill();
        }
        sliderTweener = slider.DOValue(value, 0.5f);
        sliderTweener.onComplete = () =>
        {
            sliderTweener = null;
            float valuefade = 0;
            if (GuideManager.Instance.isFirstGame)
            {
                if (isFirst)
                {
                    valuefade = 1;
                    isFirst = false;
                    if (value >= 1)
                    {
                        valuefade = 0;
                    }
                }
            }
          
                fadeTweener = sliderCanvasGroup.DOFade(valuefade, 0.3f);
            isFading = true;
                fadeTweener.onComplete = () =>
                {
                  
                    if (value >= 1)
                    {
                        achiveAction?.Invoke();
                        ShowQiPao(PeopleManager.Instance.GetStringValue());
                        if (GuideManager.Instance.isFirstGame)
                        {
                            GuideManager.Instance.GuideEvent();
                        }
                    }
                    else
                    {
                        
                            gameObject.SetActive(GuideManager.Instance.isFirstGame);
                            unityAction?.Invoke();
                        
                    }
                    fadeTweener = null;
                };
            
        };
    }

    public void HideUI()
    {
        slider.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        image1.gameObject.SetActive(false);
        biaoQingImg.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void ShowQiPao(string value)
    {
        qipao1.SetActive(true);
        text1.text = value;
       StartCoroutine( Global.Delay(2f, () => { qipao1.SetActive(false);gameObject.SetActive(false); }));

    }
    public void ShowQiPao(Sprite sprite,UnityEngine.Events.UnityAction unityAction,string value="È¹×Ó")
    {
        proImg.sprite = sprite;
        gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        qipaoCanvasGroup.DOFade(1, 0);
        text.text ="ÏëÂò"+ value;
        StartCoroutine(Global.Delay(5f, () =>
        {
            qipaoCanvasGroup.DOFade(0.6f, 1);
        }));
            StartCoroutine(Global.Delay(3f, () =>
        {
            unityAction?.Invoke();
        }));
    }
    public void ShowQiPao1(Sprite sprite, UnityEngine.Events.UnityAction unityAction)
    {
//image1.sprite = sprite;
        gameObject.SetActive(true);
        image1.gameObject.SetActive(true);
        StartCoroutine(Global.Delay(3f, () =>
        {
            unityAction?.Invoke();
        }));
    }
    public void ShowBiaoQing()
    {
        gameObject.SetActive(true);
        biaoQingImg.gameObject.SetActive(true);
    }
}
