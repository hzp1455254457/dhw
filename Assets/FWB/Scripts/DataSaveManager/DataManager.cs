using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ConfigData
{
    /// <summary>
    /// ��Ϸ���ݹ�����
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        /// <summary>
        /// ��Ϸ�û�����
        /// </summary>
        /// 
     [HideInInspector]
        public GameUserDataConfig gameUserDataConfig = null;
        /// <summary>
        /// ��Ϸ��������
        /// </summary>
        /// 
        [HideInInspector]
        public GameMapDataConfig gameMapDataConfig ;

        public Action saveShopDataAction = null;
        #region 7�յ�¼
        public LoginData LoginData
        {
            get
            {
                if (loginData == null)
                {
                    if (HasKey("SevenLoginDatas"))
                    {
                        //  print(Load("SaveProduceQiPaos"));
                        loginData = JsonMapper.ToObject<LoginData>(GetString("SevenLoginDatas"));


                    }
                    else
                    {
                        loginData = new LoginData();
                        loginData. sevenLoginDatas = new List<SevenLoginData>() {
                    new SevenLoginData()
                    {
                        day=1,
                        type=1,
                        states=1,
                        gift_num=100
                    },
                    new SevenLoginData()
                    {
                        day = 2,
                        type = 2,
                        states = 0,
                        gift_num=5000
                    },
                     new SevenLoginData()
                    {
                        day = 3,
                        type = 2,
                        states = 0,
                        gift_num=15000
                    },
                      new SevenLoginData()
                    {
                        day = 4,
                        type = 2,
                        states = 0,
                        gift_num=25000
                    },
                       new SevenLoginData()
                    {
                        day = 5,
                        type = 1,
                        states = 0,
                        gift_num=2000
                    },
                        new SevenLoginData()
                    {
                        day = 10,
                        type = 3,
                        states = 0,
                        gift_num=8888000
                    }
                    };

                    }

                }
                return loginData;
            }


        }
        LoginData loginData;
        public void SetSevenLoginDate(int day)
        {
            var data =LoginData. sevenLoginDatas.FindAll(s => s.day <= day && s.states == 0);
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.states = 1;
                }
            }
            LoginData.RefreshStatus();
            //IsSet = true;
        }
        public void SaveSevenLoginData()
        {
            if (loginData != null)
            {
                SetString("SevenLoginDatas", JsonMapper.ToJson(loginData));
            }
        }
        #endregion
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            InitGameData();
        }
        /// <summary>
        /// ��Ϸ���ݳ�ʼ��
        /// </summary>
        void InitGameData()
        {
            if (HasKey("GameUserDataConfig"))
            {
                string result = GetString("GameUserDataConfig");
                if (string.IsNullOrEmpty(result))
                {
                    gameUserDataConfig = InitNewUserData();
                    InitUserData(gameUserDataConfig);
                }
                else
                {
                    try
                    {
                        gameUserDataConfig = JsonMapper.ToObject<GameUserDataConfig>(result);
                        InitUserData(gameUserDataConfig);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("��Ϸ�û����ݽ���ʧ��");
                    }
                }
            }
            else
            {
                gameUserDataConfig = InitNewUserData();
                InitUserData(gameUserDataConfig);
            }

            if (HasKey("GameMapDataConfig"))
            {
                string result = GetString("GameMapDataConfig");
                if (string.IsNullOrEmpty(result))
                {
                    gameMapDataConfig = new GameMapDataConfig();
                    gameMapDataConfig.mapID = 1;
                }
                else
                {
                    try
                    {
                        gameMapDataConfig = JsonMapper.ToObject<GameMapDataConfig>(result);
                    }
                    catch (Exception ex)
                    {

                        Debug.Log("��Ϸ��ͼ���ݽ���ʧ��");
                    }
                }
            }
            else
            {
                gameMapDataConfig = new GameMapDataConfig();
                gameMapDataConfig.mapID = 1;
            }
        }

        /// <summary>
        /// ���ظ������ݳ�ʼ��
        /// </summary>
        /// <param name="data"></param>
        void InitUserData(GameUserDataConfig data)
        {
            _chuanDanNumber = data.chuanDanNumber;
            _coinNumber = data.coinNumber;
            
        }
        /// <summary>
        /// ��ʼ�����û�����
        /// </summary>
        GameUserDataConfig InitNewUserData()
        {
            GameUserDataConfig t = new GameUserDataConfig();
            t.userName = "";
            t.coinNumber = 1000;
            t.chuanDanNumber = 100;
            t.chuanTime = 0;
            return t;
        }

        private void OnApplicationPause(bool pause)
        {

            
            if (pause)
            {
                saveShopDataAction?.Invoke();
                Debug.Log("��ͣ�洢����");
                string userData = JsonMapper.ToJson(gameUserDataConfig);
                SetString("GameUserDataConfig", userData);

                string gameData = JsonMapper.ToJson(gameMapDataConfig);
                SetString("GameMapDataConfig", gameData);
                SaveSevenLoginData();
                PlayerPrefs.Save();
                print("");
            }
        }
        float onlineTimes;
        private void Update()
        {
            onlineTimes += Time.deltaTime;
            if (onlineTimes >= 1)
            {
                gameUserDataConfig.onLineTime +=(int) onlineTimes;
                onlineTimes = 0;
            }
            //if (gameUserDataConfig.onLineTime >= 20)
            //{
            //    ChuanDanClickedPanel chuanDanClickedPanel=null;
            //    chuanDanClickedPanel.OnShowing();
            //   var tf= PosManager.Instance.bornTfs[100];
            //}
        }
        
        private void OnApplicationQuit()
        {
           
            saveShopDataAction?.Invoke();
            Debug.Log("�˳��洢����");
            string userData = JsonMapper.ToJson(gameUserDataConfig);
            SetString("GameUserDataConfig", userData);

            string gameData = JsonMapper.ToJson(gameMapDataConfig);
            SetString("GameMapDataConfig", gameData);
            SaveSevenLoginData();
            PlayerPrefs.Save();
        }
        public void ClearOlineTime()
        {
            gameUserDataConfig.onLineTime = 0;
            onlineTimes = 0;
        }
      public void CheckLoginDayCount()
        {
           if(gameUserDataConfig.loginTime == null)
            {
                gameUserDataConfig.loginTime = TimeManager.GetSystemTime().ToString();
            }
            else
            {
              if(DateTime.Parse(gameUserDataConfig.loginTime).Date != TimeManager.GetSystemTime().Date)//����ϴμ�¼��ʱ���ϵͳʱ���ǲ���ͬһ��
                {
                    ClearOlineTime();
                    gameUserDataConfig.loginTime = TimeManager.GetSystemTime().ToString();
                    if (gameUserDataConfig.huoyueDay == 0)
                    {
                        gameUserDataConfig.huoyueDay = 1;
                        SetSevenLoginDate(gameUserDataConfig.huoyueDay);
                    }
                    else
                    {

                        gameUserDataConfig.huoyueDay++;//���ӻ�Ծ����
                       
                            SetSevenLoginDate(gameUserDataConfig.huoyueDay);
                        
                       
                    }

                }
                else
                {
                    gameUserDataConfig.loginTime = TimeManager.GetSystemTime().ToString();
                }
            }
          
        }
        private bool HasKey(string key)
        {
            string des_key = EncryptDES(key);
            return PlayerPrefs.HasKey(des_key);
        }
        public string GetString(string key, string value = null)
        {
            if (HasKey(key))
            {
                string des_key = EncryptDES(key);
                string des_val = PlayerPrefs.GetString(des_key);
                string str_val = DecryptDES(des_val);
                return str_val;
            }
            return value;
        }
        public void SetString(string key, string value)
        {
            string des_key = EncryptDES(key);
            string des_val = EncryptDES(value.ToString());
            PlayerPrefs.SetString(des_key, des_val);
        }

        #region ���ݼ���
        //Ĭ����Կ����
        private static byte[] Keys = { 0xEF, 0xAB, 0x56, 0x78, 0x90, 0x34, 0xCD, 0x12 };
        /// <summary>
        /// DES�����ַ���
        /// </summary>
        /// <param name="encryptString">�����ܵ��ַ���</param>
        /// <returns>���ܳɹ����ؼ��ܺ���ַ�����ʧ�ܷ���Դ��</returns>
        public static string EncryptDES(string encryptString, string password = "huibang0")
        {
            string encryptKey = password;
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// DES�����ַ���
        /// </summary>
        /// <param name="decryptString">�����ܵ��ַ���</param>
        /// <param name="decryptKey">������Կ,Ҫ��Ϊ8λ,�ͼ�����Կ��ͬ</param>
        /// <returns>���ܳɹ����ؽ��ܺ���ַ�����ʧ�ܷ�null</returns>
        public static string DecryptDES(string decryptString, string password = "huibang0")
        {
            string decryptKey = password;
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region �û����ݼ���
        /// <summary>
        /// �û���������
        /// </summary>
        public int chuanDanNumber
        {
            get
            {
                return _chuanDanNumber;
            }
            set
            {
                _chuanDanNumber = value;
                gameUserDataConfig.chuanDanNumber = _chuanDanNumber;
                chuanDanAction?.Invoke(_chuanDanNumber);
            }
        }
        private int _chuanDanNumber;
        /// <summary>
        /// ������������
        /// </summary>
        public Action<int> chuanDanAction = null;

        /// <summary>
        /// �û���������
        /// </summary>
        public int coinNumber
        {
            get
            {
                return _coinNumber;
            }
            set
            {
                _coinNumber = value;
                gameUserDataConfig.coinNumber = _coinNumber;
                coinNumberAction?.Invoke(_coinNumber);
            }
        }
        private int _coinNumber;
        /// <summary>
        /// ���Ҹ��ļ���
        /// </summary>
        public Action<int> coinNumberAction = null;
        #endregion
    }

    /// <summary>
    /// �������
    /// </summary>
    /// 
    [Serializable]
    public class GameUserDataConfig
    {
        /// <summary>
        /// �û�����
        /// </summary>
        public string userName;
        /// <summary>
        /// ��������
        /// </summary>
        public int coinNumber;
        /// <summary>
        /// ��������
        /// </summary>
        public int chuanDanNumber;
        /// <summary>
        /// ������ʱʱ�䵥λ��
        /// </summary>
        public int chuanTime;
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public int onLineTime;
        public string loginTime;
        public int huoyueDay;
        /// <summary>
        /// �������
        /// </summary>
        public int achivePeopleCount;

        public int shouYinCount;

    }

    /// <summary>
    /// ��ͼ��������
    /// </summary>
    /// 
   
    public class GameMapDataConfig
    {
        /// <summary>
        /// ��ͼ���
        /// </summary>
        public int mapID;
        /// <summary>
        /// ��ͼ��Ӧ��̯λ��״̬
        /// </summary>
        public List<ShopStatus> mapShopStatusList = new List<ShopStatus>();
    }

    public class ShopStatus
    {
        public Shop shop;
        public string timeString;
    }
    public class LoginData
    {
       
      public  List<SevenLoginData> sevenLoginDatas;

         bool isGet ;
        public bool IsGet
        {
            set
            {
                isGet = value;
               // UnityActionManager.Instance.DispatchEvent("RefreshSevenIcon");

            }
            get { return isGet; }
        }

        public int lastCount = 0;
        public int currentCount = 0;
        public void SetCount()
        {
            lastCount = currentCount;
            currentCount = 0;
            Debug.LogError(".....current" + currentCount + ".....last" + lastCount);
        }
        public void AddCount()
        {
            currentCount++;
        }
        public void RefreshStatus()
        {
            IsGet = false;
            SetCount();
        }
    }
    /*
    /// <summary>
    /// ������ͼ�е��̵�����
    /// </summary>
    public class ShopStatus
    {
        /// <summary>
        /// ̯λID
        /// </summary>
        public int shopId;
        /// <summary>
        /// ̯λ���ƻ���ID
        /// </summary>
        public string shopName;
        /// <summary>
        /// ̯λ״̬���Ƿ����
        /// </summary>
        public bool isLocked;
        /// <summary>
        /// ̯λĿǰ�ڿ���ʲô��Ʒ
        /// </summary>
        public string shopCellItemName;
    }
    */
}