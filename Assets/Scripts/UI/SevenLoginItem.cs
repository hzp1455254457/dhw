using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenLoginItem : MonoBehaviour
{
    public SevenLoginData sevenLoginData;
    public GameObject[] stateGos;
    public Button button;
   // public Transform bornTf,targetTf;
    public Text text,count;
    public Animator animator;
    //public GameObject guideGo;
    public int index;
    //public GameObject go;
    SevenLoginPanel sevenLoginPanel;
    public void Init()
    {
        text.text = string.Format("��{0}��", sevenLoginData.day);
        if (index == 2 || index == 3 || index == 1)
        {
            count.text = string.Format("{0}Ԫ", sevenLoginData.gift_num *MainUI.Instance.redScale);
        }
        else
            count.text = string.Format("{0}��", sevenLoginData.gift_num);
        SetStates(sevenLoginData.states);
        sevenLoginPanel = GetComponentInParent<SevenLoginPanel>();
    }

    public void SetStates(int states)
    {
        sevenLoginData.states = states;
        for (int i = 0; i < stateGos.Length; i++)
        {
            stateGos[i].SetActive(false);
        }
        stateGos[states].SetActive(true);
        if (states == 1)
        {
            animator.SetBool("walk", true);
           
          
        }
        else 
        {
            animator.SetBool("walk", false);
            if (states == 2)
            {
                button.interactable = false;
                stateGos[0].SetActive(true);
                if (index == 2 || index == 3)
                {
               
                }
            }
           
           
        }
       
    }
  
    // Update is called once per frame
    public void ClickFun()
    {if (!sevenLoginPanel.IsGet)
        {
            switch (sevenLoginData.states)
            {
                case 0:
                    ShowTips(1.5f,"���յ�¼������ȡŶ��");
                   // AndroidAdsDialog.Instance.requestCalendarPermission();

                    break;
                case 1:
                    AdwardFun();
                    break;
                case 2:
                    ShowTips(1.5f,"�Ѿ���ȡ����");
                    break;
            }
        }
        else
        {
            ShowTips(1f,"�����Ѿ����������Ŷ������������");
        }
    }
    public void ShowTips(float scale, string value)
    {
        // AndroidAdsDialog.Instance.ShowToasts(born, target, value, Color.black, null, null, scale);
        MainUI.Instance.ShowPiaoChuanText(scale, value);
    }

    public void AdwardFun()
    {
        switch (sevenLoginData.type)
        {
            case 1:

               sevenLoginPanel .ShowGuide(false);

                MainUI.Instance.AddGold(sevenLoginData.gift_num);
                MainUI.Instance.ShowPiaoChuan(null, "���", string.Format("+{0}", sevenLoginData.gift_num));
                break;
            case 2:
                //print("��ó齱����" + sevenLoginData.gift_num);
                //MainUI.Instance.AddRed(sevenLoginPanel.lastCount*1000);
                //MainUI.Instance.ShowPiaoChuan(null, "���", string.Format("+{0}Ԫ", sevenLoginPanel.lastCount ));
                PanelMgr.Instance.OpenPanel<SevenLoginClickedPanel>("SevenLoginClickedPanel", sevenLoginPanel.lastCount, sevenLoginData.day - 1);
             

                break;
            case 3:
              
                break;
            case 4:
             
                break;
            case 5:
                //PlayerData.Instance.GetDiamond(sevenLoginData.gift_num);
                //AndroidAdsDialog.Instance.ShowToasts(string.Format("+{0}", sevenLoginData.gift_num), ResourceManager.Instance.GetSprite("��ʯ"), Color.black);
                break;
            case 6:
           
                break;
        }
        print("���" + sevenLoginData.gift_num);
        SetStates(2);
        sevenLoginPanel.IsGet = true;
    }
    
}
