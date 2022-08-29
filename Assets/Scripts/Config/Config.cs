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
        public int map_id;//第几关（第几个地图）
        public int custom_id;
        public int custom_need_click;//需要点击几次进入步行街
    }
    [SerializeField]
    public class Shop
    {
        public int map_id;// 第几关（第几个地图）
        public int shop_id;//商店的id
        public string shop_name;//商店的对应名称
        public string shop_pic; //商店的图片名称
        public int isunlock_initstate;//初始状态是否解锁 1=已解锁，0=未解锁,2=时间到达
        public int unlock_time;//建造时需要的时间（秒）
        public int unlock_money;//建造时所需的金币
        public int redCount;//建筑时红包显示数量
    }
    [SerializeField]
    public class Goods
    {
        public int item_id;//商品id
        public string item_name;//商品名称
        public string item_pic;//商品图片   
        public int shop_id;//商店的id
    }

}