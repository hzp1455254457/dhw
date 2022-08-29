using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenIcon : MonoBehaviour
{
    public GameObject tips;
    public Button sevenBt;
    void Start()
    {

        gameObject.SetActive(MainUI.Instance.IsRedShow);
        if (MainUI.Instance.IsRedShow)
        {
            sevenBt = Global.FindChild<Button>(transform, "sevenBt");
            tips = Global.FindChild(transform, "tips");
            RefreshStutes();
            UnityActionManager.Instance.AddAction("RefreshSevenIcon", RefreshStutes);
            sevenBt.onClick.AddListener(ClickAction);
        }
    }
    private void RefreshStutes()
    {
        tips.SetActive(!ConfigData.DataManager.Instance.LoginData.IsGet);
    }
   private void ClickAction()
    {
        PanelMgr.Instance.OpenPanel<SevenLoginPanel>("SevenLoginPanel");
    }
}
