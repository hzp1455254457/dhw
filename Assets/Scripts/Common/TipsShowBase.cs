using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsShowBase
{
    private static TipsShowBase _Instance;

    public static TipsShowBase Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new TipsShowBase();
            }
            return _Instance;
        }
    }
    private TipsShowBase()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">预制体名称</param>
    /// <param name="bornTf">生成位置</param>
    /// <param name="targetTf">目标位置</param>
    /// <param name="value">text上面显示得文字，要显示几个就要传入几个</param>
    /// <param name="sprite">ui上面显示得图片</param>
    /// <param name="color">text显示得颜色</param>
    /// <param name="unityAction">结束时得回调函数</param>
    /// <param name="scale">放大比例</param>
    public void Show(string name, Transform bornTf, Transform targetTf,Sprite[] sprite, Color[] color, UnityEngine.Events.UnityAction unityAction = null, float scale = 1f, params string[] value)
    {
        var effect = GameObjectPool.Instance.CreateObject(name, ResourceManager.Instance.GetProGo(name), bornTf, Quaternion.identity);
        effect.GetComponent<TipsEffectBase>().Show(targetTf,  sprite, unityAction, color, scale,value);

    }
}
