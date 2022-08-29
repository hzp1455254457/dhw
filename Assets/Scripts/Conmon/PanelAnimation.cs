using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public abstract class PanelAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform backTf;
   
    public virtual void Animation(System.Action action = null, float scale = 1.0f,float time=0.3f)
    {
       
        backTf.transform.localScale = Vector3.zero;
        backTf.transform.DOScale(Vector3.one * 1.1f* scale, time).SetEase(Ease.OutSine).SetUpdate(true).onComplete
             += () =>
             {
                 backTf.transform.DOScale(Vector3.one * 1f* scale, time).SetUpdate(true).onComplete = ()=> action?.Invoke();
             };
    }
   
}
