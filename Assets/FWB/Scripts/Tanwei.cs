using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace ConfigData
{
    public class Tanwei : MonoBehaviour
    {
        public int shop_id;
        public Sprite lockSprite;
        /// <summary>
        /// ��ǰ�̵����Ϣ
        /// </summary>
        private ShopStatus currenShop = null;
        private Action<TimeSpan> waitingForLockAction = null;
        /// <summary>
        /// �Ƿ��ڽ����У�̯λ
        /// </summary>
        private bool isInBuilding = false;
        Sprite sprite;
        public GameObject add, build, shangpu, shopText, timeText,tipsGo;
        // Start is called before the first frame update
        void Awake()
        {
            waitingForLockAction = SetWaitingChange;
            InitShopStatus();
        }

        /// <summary>
        /// �ȴ�ʱ����ת�ļ���
        /// </summary>
        /// <param name="t"></param>
        void SetWaitingChange(TimeSpan t)
        {
            shopText.GetComponent<TextMeshPro>().text = currenShop.shop.shop_name + "������";
            string dayString = t.Days > 0 ? t.Days + "��" : "";
            string hourString = t.Hours > 0 ? t.Hours + "ʱ" : "";
            string minuteString = t.Minutes > 0 ? t.Minutes + "��" : "";
            string timeSpring = "";
            if (string.IsNullOrEmpty(dayString))
            {
                if (string.IsNullOrEmpty(hourString))
                {
                    if (string.IsNullOrEmpty(minuteString))
                    {
                        timeSpring = t.Seconds + "��";
                    }
                    else
                    {
                        timeSpring = minuteString;
                    }
                }
                else
                {
                    timeSpring = hourString;
                }

            }
            else
            {
                timeSpring = dayString;
            }
            timeText.GetComponent<TextMeshPro>().text = "����ʱ:"+ timeSpring;
        }
        bool isGuide = false;
        public void SetGuide()
        {
            isGuide = true;
        }
        /// <summary>
        /// ��ʼ���̵�״̬
        /// </summary>
        public void InitShopStatus()
        {
            Debug.Log("��ʼ������");
            if (DataManager.Instance.gameMapDataConfig.mapShopStatusList != null)
            {
                ShopStatus t = DataManager.Instance.gameMapDataConfig.mapShopStatusList.Find((t) => t.shop.shop_id == shop_id && t.shop.map_id == DataManager.Instance.gameMapDataConfig.mapID);
                if (t != null)
                {
                    InitShopData(t);
                }
                else
                {
                    Shop c = Config.Instance.ShopList.Find((t) => t.shop_id == shop_id && t.map_id == DataManager.Instance.gameMapDataConfig.mapID);
                    ShopStatus a = new ShopStatus();
                    a.shop = c;
                    a.timeString = null;
                    InitShopData(a);
                }
            }
            //���Ӵ洢�̵����ݼ���
            DataManager.Instance.saveShopDataAction += SaveShopData;
        }

        /// <summary>
        /// ��ʼ���̵�����
        /// </summary>
        /// <param name="c"></param>
        public void InitShopData(ShopStatus c)
        {
            currenShop = c;
            sprite = Resources.Load<Sprite>("shop_pic/" + currenShop.shop.shop_pic);
            if (currenShop.shop.isunlock_initstate == 0)
            {
                GetComponent<SpriteRenderer>().sprite = null;
                if (!string.IsNullOrEmpty(currenShop.timeString))
                {
                    add.gameObject.SetActive(false);
                    build.gameObject.SetActive(true);
                    shangpu.SetActive(false);
                    StartCoroutine(waitingForUnLock(currenShop.shop.unlock_time));
                    gameObject.layer = 7;
                    AStarManager.CreactWay();
                }
                else
                {
                    add.gameObject.SetActive(true);
                    build.gameObject.SetActive(false);
                    shangpu.SetActive(false);
                    gameObject.layer = 6;
                    AStarManager.CreactWay();
                }
            }
            else if(currenShop.shop.isunlock_initstate == 1)
            {
               
                JieSuo();
            }
            else
            {
                if (MainUI.Instance.IsRedShow)
                { Timed(); }
                else
                {
                    JieSuo();
                }
            }

        }

        /// <summary>
        /// �Ƿ��������
        /// </summary>
        private bool unlockImmediately = false;
        /// <summary>
        /// �ȴ�������Э��
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        IEnumerator waitingForUnLock(int time)
        {
            DateTime unlock_time = DateTime.Parse(currenShop.timeString);
            TimeSpan t = TimeManager.GetSystemTime() - unlock_time;
            while (t.TotalSeconds < time && !unlockImmediately)
            {
                isInBuilding = true;
                Debug.Log("waiting:" + t.TotalSeconds);
                DateTime end = unlock_time.AddSeconds(time);
                TimeSpan t2 = end - DateTime.Now;
                waitingForLockAction?.Invoke(t2);
                yield return new WaitForSeconds(1.0f);
                t = DateTime.Now - unlock_time;
            }
            if (MainUI.Instance.IsRedShow)
            {
                AndroidHelper.Instance.UploadDataEvent("unlock_shop");
                Timed();
            }
            else
            {
                JieSuo();
            }
        }

        /// <summary>
        /// �������̯λ
        /// </summary>
        public void UnLockTanWei()
        {
            build.gameObject.SetActive(true);
            add.gameObject.SetActive(false);
            shangpu.SetActive(false);
            currenShop.timeString = TimeManager.GetSystemTime().ToString();
            StartCoroutine(waitingForUnLock(currenShop.shop.unlock_time));
            gameObject.layer = 7;
            AStarManager.CreactWay();
        }
        /// <summary>
        /// �������̯λ
        /// </summary>
        public void UnLockTanWeiImmediately()
        {
            if (currenShop.shop.isunlock_initstate == 0)
                unlockImmediately = true;
        }
        /// <summary>
        /// չʾ����ҳ��
        /// </summary>
        /// <param name="t"></param>
        public void ShowUnLockPage(Tanwei t)
        {
            if (!isInBuilding)
            {
                if (isGuide)
                {
                    isGuide = false;
                    GuideManager.Instance.HideTips();
                    AndroidHelper.Instance.UploadDataEvent("jiaocheng_jiesuo_wjt");
                }
                if (MainUI.Instance.IsRedShow)
                {
                    AndroidHelper.Instance.UploadDataEvent("click_daibaitan_btn");

                  PanelMgr.Instance.OpenPanel<TanweiDaiJieSuoPanel>("TanweiDaiJieSuoPanel", sprite, currenShop.shop.shop_name, currenShop.shop.unlock_time, currenShop.shop.unlock_money, this); }
                else
                {
                    if (MainUI.Instance.goldValue >= GetShopInfo().unlock_money)
                    {
                        int value = MainUI.Instance.goldValue;
                        int value1 = value - GetShopInfo().unlock_money;

                        MainUI.Instance.RefreshGold(value1, 0.1f);
                        UnLockTanWei();
                    }
                }
            }
            else
            {
              
                
            }
        }
        /// <summary>
        /// ���ۻ���
        /// </summary>
        public void SellItems()
        {
            Debug.Log("������Ʒ");
            MainUI.Instance.ShowItemUI(currenShop.shop.shop_id);
        }

        /// <summary>
        /// �̵���
        /// </summary>
        public void ShopClick()
        {
            if (!ClickManager.IsPointerOverUIObject())
            {
                if (currenShop.shop.isunlock_initstate == 0)
                {
                    Debug.Log("��ס��");
                    ShowUnLockPage(this);
                }
                else if(currenShop.shop.isunlock_initstate == 1)
                {
                    SellItems();
                }
                else
                {
                    if (MainUI.Instance.IsRedShow)
                    {
                        PanelMgr.Instance.OpenPanel<TanweiDaiJieSuoRedPanel>("TanweiDaiJieSuoRedPanel", this, sprite);
                    }
                    else
                    {
                        JieSuo();
                    }
                }
            }
        }
     public   void JieSuo()
        {
            currenShop.shop.isunlock_initstate = 1;
            build.gameObject.SetActive(false);
            add.gameObject.SetActive(false);
            shangpu.SetActive(true);
            gameObject.layer = 7;
            GetComponent<SpriteRenderer>().sprite = sprite;
            shangpu.GetComponent<SpriteRenderer>().sprite = sprite;
            if(shangpu.GetComponent<BoxCollider2D>()==null)
            shangpu.AddComponent<BoxCollider2D>();
            AStarManager.CreactWay();
        }
        void Timed()
        {
            shopText.GetComponent<TextMeshPro>().text = currenShop.shop.shop_name + "�������";
            isInBuilding = false;
            currenShop.timeString = null;
            currenShop.shop.isunlock_initstate = 2;
            unlockImmediately = false;
           // build.gameObject.SetActive(false);
            add.gameObject.SetActive(false);
            if (MainUI.Instance.IsRedShow)
            {
                tipsGo.SetActive(true);
            }
            timeText.SetActive(false);
            //shangpu.SetActive(true);
            //GetComponent<SpriteRenderer>().sprite = sprite;
            //shangpu.GetComponent<SpriteRenderer>().sprite = sprite;
            //if (shangpu.GetComponent<BoxCollider2D>() == null)
            //    shangpu.AddComponent<BoxCollider2D>();
            gameObject.layer = 7;
            AStarManager.CreactWay();
        }
        private void OnMouseDown()
        {
            ShopClick();
        }

        /// <summary>
        /// �����̵�״̬���浽PlayerPrefs
        /// </summary>
        void SaveShopData()
        {
            List<ShopStatus> t = DataManager.Instance.gameMapDataConfig.mapShopStatusList;
            //�Ƿ���ӽ���List
            bool isAdding = false;
            if (t!=null&&t.Count>0)
            {
                for (int i = 0; i < t.Count; i++)
                {
                    if (t[i].shop.shop_id == currenShop.shop.shop_id)
                    {
                        DataManager.Instance.gameMapDataConfig.mapShopStatusList[i] = currenShop;
                        Debug.Log("�洢�̵�����:" + currenShop.shop.shop_id);
                        isAdding = true;
                        break;
                    }
                }
            }

            if (!isAdding)
            {
                if (DataManager.Instance.gameMapDataConfig.mapShopStatusList == null)
                    DataManager.Instance.gameMapDataConfig.mapShopStatusList = new List<ShopStatus>();
                DataManager.Instance.gameMapDataConfig.mapShopStatusList.Add(currenShop);
                Debug.Log("��Ӵ洢�̵�����:" + currenShop.shop.shop_id);
            }
            print(1);
        }
        /// <summary>
        /// 0δ������1�ǽ�����2�ǽ�����
        /// </summary>
        /// <returns></returns>
        public int GetLockStatus()
        {
            if (isInBuilding)
                return 2;
            else
                return currenShop.shop.isunlock_initstate;
        }

        /// <summary>
        /// ̯λ�̵���Ϣ
        /// </summary>
        /// <returns></returns>
        public Shop GetShopInfo()
        {
            return currenShop.shop;
        }
    }
}
