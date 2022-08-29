using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TipsEffectBase : MonoBehaviour
{
    [Header("所有需要显示img组件")]
    public Image[] Img;
    [Header("所有需要显示text组件")]
    public Text[] tipsInfo;
    [Header("淡入淡出得组件")]
    protected Graphic[] graphics;
    [Header("控制外部框体宽度得组件")]
    public RectTransform[] rects;
    [Header("外部框体组件")]
    public RectTransform rectBack;
    // // public RectTransform firstRect;
    [Header("左右间隔宽度")]
    public float x;

    public Transform backTf;
    protected virtual void Awake()
    {
        rectBack = GetComponent<RectTransform>();
        graphics = GetComponentsInChildren<Graphic>();
        //if(rects==null)
        // rects = firstRect.GetComponentsInChildren<RectTransform>();
        foreach (var item in graphics)
        {
            item.DOFade(0, 0);
        }
    }

    public UnityAction onComplete;
    public virtual void Animation()
    {

        backTf.transform.DOScale(Vector3.one * 1.1f * scale, 0.3f).SetUpdate(true).onComplete
             += () => backTf.transform.DOScale(Vector3.one * 1f * scale, 0.3f).SetUpdate(true).onComplete += () =>
             {
                 foreach (var item in graphics)
                 {
                     item.DOFade(0, 0.8f).SetUpdate(true);
                 };
                 GameObjectPool.Instance.CollectObject(gameObject, 1f);

                 StartCoroutine(Global.Delay(0.7f, () =>
                 {
                     if (onComplete != null)
                     {
                         onComplete();
                     }
                 }));

             };

    }
    protected float scale = 1f;
    public virtual void Show(Transform targetTf,  Sprite[] sprite, UnityAction unityAction, Color[] color, float scale = 1f,params string[] tipsValue)
    {

        this.scale = scale;
        transform.localScale = Vector3.one * scale;
        if (Img != null)
        {

            for (int i = 0; i < Img.Length; i++)
            {
                if (sprite != null && sprite.Length - 1 >= i)
                    Img[i].sprite = sprite[i];
                else
                {
                    Img[i].sprite = null;
                    Img[i].rectTransform.sizeDelta = new Vector2(0, Img[i].rectTransform.sizeDelta.y);
                }

            }
        }
        if (tipsInfo != null)
        {
            for (int i = 0; i < tipsInfo.Length; i++)
            {
                if (tipsValue != null && tipsValue.Length - 1 >= i)
                    tipsInfo[i].text = tipsValue[i];
                else
                {
                    tipsInfo[i].text = null;
                    tipsInfo[i].rectTransform.sizeDelta = new Vector2(0, tipsInfo[i].rectTransform.sizeDelta.y);
                }
                if (color != null && color.Length - 1 >= i)
                    tipsInfo[i].color = color[i];
                else
                {
                    tipsInfo[i].color = Color.black;

                }
            }
        }
        onComplete = null;
        onComplete = unityAction;
        Animation(targetTf);
        StartCoroutine(ChangeX(x));

    }

    protected IEnumerator ChangeX(float x)
    {
        yield return 0;
        float weight = 0;
        foreach (var item in rects)
        {
            weight += item.sizeDelta.x;
        }
        rectBack.sizeDelta = new Vector2(weight + x, rectBack.rect.height);
    }

    protected virtual void Animation(Transform targetTf)
    {
        foreach (var item in graphics)
        {
            item.DOFade(1, 1f).SetUpdate(true);
        }
        transform.DOLocalMoveY(targetTf.localPosition.y, 0.6f).SetEase(Ease.OutQuint).SetUpdate(true).onComplete += Animation;
    }
}
