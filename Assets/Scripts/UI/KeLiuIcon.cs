using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeLiuIcon : MonoBehaviour
{
    public Text countText;
    public Button keliuBt;
    
    void Start()
    {

        gameObject.SetActive(MainUI.Instance.IsRedShow);
        if (MainUI.Instance.IsRedShow)
        {
            keliuBt = GetComponent<Button>();
            countText = Global.FindChild<Text>(transform, "countText");
            RefreshStutes(ConfigData.DataManager.Instance.gameUserDataConfig.achivePeopleCount);
            UnityActionManager.Instance.AddAction<int>("Refreshkeliu", RefreshStutes);
            keliuBt.onClick.AddListener(ClickAction);
        }
    }
    private void RefreshStutes(int count)
    {
        countText.text = count.ToString();
    }
   private void ClickAction()
    {
        AndroidHelper.Instance.UploadDataEvent("click_keliu_icon");
        PanelMgr.Instance.OpenPanel<KeLiuClickedPanel>("KeLiuClickedPanel");
    }
}
