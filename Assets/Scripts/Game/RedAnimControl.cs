using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RedAnimControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void Anim(UnityEngine.Events.UnityAction unityAction=null)
    {
     AndroidHelper. isShowTabled =false;
        AndroidHelper.Instance.UploadDataEvent("click_finish_buy_drop_hb");
        AudioManager.Instance.PlaySound("点击掉落红包后");
        canClick = false;
           SkeletonAnimation anim = GetComponentInChildren<SkeletonAnimation>();
        Spine.TrackEntry trackEntry= anim.AnimationState.SetAnimation(0, "diaoluo-ing", false);
        trackEntry.Complete+=(s)=> { anim.AnimationState.SetAnimation(0, "diaoluo-done", true);
            canClick = true;
            unityAction?.Invoke();
        };
    }
    bool canClick = false;
    private void OnMouseDown()
    {if (!GuideManager.Instance.isFirstGame)
        { if (ClickManager.IsPointerOverUIObject()) return;
        
        }
        if (!canClick) return;
        gameObject.layer = 6;
        //Debug.LogError("点击红包");
        GameObjectPool.Instance.CollectObject(gameObject);
        if (!GuideManager.Instance.isFirstGame)
        {
            AndroidHelper.Instance.ShowTableVideo("红包", () =>
            {
                MainUI.Instance.AddRed((int)(JavaCallUnity.Instance.GetTableRedCount())); MainUI.Instance.ShowPiaoChuan(null, "红包", "+" + (JavaCallUnity.Instance.GetTableRedCount() * MainUI.Instance.redScale).ToString("f2") + "元");
                AndroidHelper.Instance.UploadDataEvent("finish_drop_hb");
            });
            
        }
        else
        {
           float value = 0.1f;
            if (AndroidHelper.Instance.RewardRPState())
            {
                value = 10f;

                
            }
            AndroidHelper.Instance.getCourseRewardRP();
            MainUI.Instance.AddRed((int)(value / MainUI.Instance.redScale)); MainUI.Instance.ShowPiaoChuan(null, "红包", "+" + value + "元");
            GuideManager.Instance.GuideEvent();
            
        }
        AndroidHelper.isShowTabled = true;
    }
}
