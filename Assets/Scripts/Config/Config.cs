using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfigData
{
    public class Config : MonoBehaviour
    {
        private static Config instance;

        public List<Customer> CustomerList;
        public List<Shop> ShopList;
        public List<Goods> GoodsList;
       // public List<SevenLoginData> sevenLoginDataLists;
        public static Config Instance
        {
            get
            {
                if (!instance)
                {
                    GameObject obj = new GameObject("Config");
                    DontDestroyOnLoad(obj);

                    instance = obj.AddComponent<Config>();
                    instance.ReadData();
                }
                return instance;
            }
            set => instance = value;
        }

        private void Awake()
        {
            instance = this;
            ReadData();
        }
        void ReadData()
        {
            CustomerList = JsonMapper.ToObject<List<Customer>>(Resources.Load<TextAsset>("Config/CustomerList").text);
            ShopList = JsonMapper.ToObject<List<Shop>>(Resources.Load<TextAsset>("Config/ShopList").text);
            GoodsList = JsonMapper.ToObject<List<Goods>>(Resources.Load<TextAsset>("Config/GoodsList").text);
          //  sevenLoginDataLists = JsonMapper.ToObject<List<SevenLoginData>>(Resources.Load<TextAsset>("Config/sevenLoginDataLists").text);
        }
    }
    [SerializeField]
    public class Customer
    {
        public int map_id;//�ڼ��أ��ڼ�����ͼ��
        public int custom_id;
        public int custom_need_click;//��Ҫ������ν��벽�н�
    }
    [SerializeField]
    public class Shop
    {
        public int map_id;// �ڼ��أ��ڼ�����ͼ��
        public int shop_id;//�̵��id
        public string shop_name;//�̵�Ķ�Ӧ����
        public string shop_pic; //�̵��ͼƬ����
        public int isunlock_initstate;//��ʼ״̬�Ƿ���� 1=�ѽ�����0=δ����,2=ʱ�䵽��
        public int unlock_time;//����ʱ��Ҫ��ʱ�䣨�룩
        public int unlock_money;//����ʱ����Ľ��
        public int redCount;//����ʱ�����ʾ����
    }
    [SerializeField]
    public class Goods
    {
        public int item_id;//��Ʒid
        public string item_name;//��Ʒ����
        public string item_pic;//��ƷͼƬ   
        public int shop_id;//�̵��id
    }

}