using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Text;
using System;
using System.Security.Cryptography;
using System.IO;

public class DataSaver : MonoSingleton<DataSaver>
{



    //ES3Settings settings;


    //private string _key = "c5e65bda-16c0-442a-be7f-e9d4d6b2765b";

    //����
    //private static DataSaver instance;

    //public static DataSaver Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            Debug.LogError("DataSaver is null !!");
    //        }
    //        return instance;
    //    }
    //    set => instance = value;
    //}
    // static string password="huibang0";
    public static string PassWord
    {
        get
        {
            string sysid = SystemInfo.deviceUniqueIdentifier;
            sysid = sysid.Substring(0, 8);
            return sysid;
        }
    }

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


    //Android���õķ���������Key
    public bool isES3 = false;//�Ƿ���es3�洢
    public void GetKey(string key)
    {

        Debug.Log("GetKey" + key);
    }
    public override void Init()
    {
        base.Init();

        //if (isES3)
            //ES3.CacheFile("HLDH.es3");
    }
    //private void Awake()
    //{
    //    instance = this;
    //    //if (string.IsNullOrEmpty(_key))
    //    //{
    //    //    Debug.LogError("_key Ϊ�գ���");
    //    //}
    //    //ES3���ã�ʹ��AES���ܣ�Gzipѹ��
    //    //settings = new ES3Settings(ES3.EncryptionType.AES, _key);
    //    //settings.compressionType = ES3.CompressionType.Gzip;
    //    //settings.location = ES3.Location.Cache;

    //    ES3.CacheFile("HLDH.es3");

    //}



    /// <summary>
    /// ����Ƿ���key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool HasKey(string key)
    {
        
        //if (isES3)
            //return ES3.KeyExists(key);
     
            string des_key = EncryptDES(key);
            //PlayerPrefs.DeleteKey(des_key);
            return PlayerPrefs.HasKey(des_key);
        

    }
    public void DeleteKey(string key)
    {

        if (HasKey(key))
        {
            if (isES3)
            {
                //ES3.DeleteKey(key);
            }
            else
            {
                string des_key = EncryptDES(key);

                PlayerPrefs.DeleteKey(des_key);
            }
        }


    }


    public void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value?1:0);
    }
    public bool GetBool(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    /// <summary>
    /// ����int
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetInt(string key, int value)
    {
        if (isES3)
        {
          //  ES3.Save<int>(key, value);
        }
        else
        {
            string des_key = EncryptDES(key);
            string des_val = EncryptDES(value.ToString());
            PlayerPrefs.SetString(des_key, des_val);
        }
    }
    public void Setlong(string key, long value)
    {
        if (isES3)
        {
           // ES3.Save<long>(key, value);
        }
        else
        {

        }
    }
    public void SetFloat(string key, float value)
    {
        if (isES3)
        {
           // ES3.Save<float>(key, value);
        }
        else
        {
            string des_key = EncryptDES(key);
            string des_val = EncryptDES(value.ToString());
            PlayerPrefs.SetString(des_key, des_val);
            //PlayerPrefs.SetFloat(key, value);
        }
    }
    public void SetQueue(string key, Queue<int> value)
    {
       // ES3.Save<Queue<int>>(key, value);
    }
    /// <summary>
    /// ��ȡint
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetInt(string key,int defaultValue=0)
    {
        if (HasKey(key))
        {
            if (isES3)
            {

            }
                //return ES3.Load<int>(key);
            else
            {
                string des_key = EncryptDES(key);
                string des_val = PlayerPrefs.GetString(des_key);
                string str_val = DecryptDES(des_val);

                int val = int.Parse(str_val);
                return val;
            }
        }
        return defaultValue;
    }
    public long Getlong(string key)
    {
        //if (ES3.KeyExists(key))
        //    return ES3.Load<long>(key);

        return 0;
    }
    public float GetFloat(string key,float value=0)
    {
        if (HasKey(key))
        {
            if (isES3) { }
               // return ES3.Load<float>(key);
            else
            {
                string des_key = EncryptDES(key);
                string des_val = PlayerPrefs.GetString(des_key);
                string str_val = DecryptDES(des_val);

                float val = float.Parse(str_val);
                return val;
            }
        }
        return value;
    }
    public Queue<int> GetQueue(string key)
    {
        //if (ES3.KeyExists(key))
        //    return ES3.Load<Queue<int>>(key);
        return null;
    }

    /// <summary>
    /// ����string
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetString(string key, string value)
    {
        if (isES3) { }
           // ES3.Save<string>(key, value);
        else
        {
            string des_key = EncryptDES(key);
            string des_val = EncryptDES(value.ToString());
            PlayerPrefs.SetString(des_key, des_val);
        }

    }
    /// <summary>
    /// ��ȡstring
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetString(string key, string defaultValue = "")
    {
        if (HasKey(key))
        {
            if (isES3) { }
                //return ES3.Load<string>(key);
            else
            {
                string des_key = EncryptDES(key);
                string des_val = PlayerPrefs.GetString(des_key);
                string str_val = DecryptDES(des_val);

                // float val = float.Parse(str_val);
                return str_val;
            }
        }
        return defaultValue;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
        else
        {
            if (isES3) { }
                //ES3.CacheFile("HLDH.es3");
        }
    }
    private void OnApplicationQuit()
    {
        SaveData();

    }
    public void SaveData()
    {
        if (isES3) { }
           // ES3.StoreCachedFile("HLDH.es3");
        else
        {
            PlayerPrefs.Save();
        }
    }
}

