using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NumberEffect : MonoBehaviour
{
    public Text text;
   // int currentCount;
   // string currentValue1;
    //string currentValue2;
   //int targetCount;
    Tweener tweener=null;
    bool isZhiXing = false;
  
    public void Animation(int taget, string left,string right,float time=1f, int initCount=0,UnityEngine.Events.UnityAction unityAction=null)
    {
        //currentCount = initCount;
      
      

        if (tweener != null)
        {
            isZhiXing = false;
            //tweener.Pause();
            tweener.Kill();
            tweener.onUpdate = null;
        }
        tweener = DOTween.To(() => initCount, x => initCount = x, taget, time);
        tweener.SetUpdate(true);
        tweener.onKill = () => { SetText(left, taget, right);
           // Debug.LogError("KILL");
            if (!isZhiXing)
            {
                unityAction?.Invoke();
            }
        };
        tweener.onUpdate = () => {
            //print("currentCount" + currentCount);
            SetText(left, initCount, right);
            //Debug.LogError("UPDATA");
        };
        tweener.onComplete = () => { unityAction?.Invoke();
            SetText(left, taget, right);
            tweener = null;
            isZhiXing = true;
        };
    }
   

   public virtual void SetText(string left ,int value2,string right)
    {

    
        text.text = string.Format("{0}{1}{2}", left, value2, right);
    }
}
