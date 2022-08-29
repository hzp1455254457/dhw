using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
enum DumpCommandType
{
    enumDump_Begin = 0,

    enumDump_BackgroundExecutionFlag = enumDump_Begin,      // 后台上传宕机信息（不显示DumpReport界面）
    enumDump_ForceUpload,                                   // Dump是否强制上报（忽略玩家在DumpReport界面上的选择）
    enumDump_LogDirectory,                                  // 设置log文件路径
    enumDump_CollectFile,                                   // 设置需收集的文件名
    enumDump_LogDirectory_UTF8,                             // 设置log文件路径（传入参数为UTF8，Unity/UE 使用此参数）
    enumDump_CollectFile_UTF8,                              // 设置需收集的文件名（传入参数为UTF8，Unity/UE 使用此参数）
    enumDump_SetURL,                                        // 设置上传url (internal_cn|internal_us|external|test) => (国内|海外|外部项目|内部测试)
    enumDump_SetBeta,                                       // 设置 beta
    enumDump_SetUserIdentifier,                             // 设置 UserIdentifier
    enumDump_AddExtraData,                                  // 添加额外数据
    enumDump_GM_TEST,                                       // GM测试指令

    enumDump_Count
}
public class DumpForPC : MonoBehaviour
{

    /// <summary>
    /// 设置AppVersion信息
    /// </summary>
    private static string YourAppVersion = "NA";

    [DllImport("Dumper64")]
    private static extern bool InitDumperCrasheye(string appkey, string version, string channId);

    [DllImport("Dumper64")]
    private static extern void UnInitDumper();

    [DllImport("Dumper64")]
    private static extern bool SetConfig(int nDumpCommandType, IntPtr pArg);

    [DllImport("Dumper64")]
    private static extern bool SetConfig(int nDumpCommandType, IntPtr[] pArg);

    //AddExtraData专用参数
    private static IntPtr[] extraData = new IntPtr[2];

    /// <summary>
    /// Windows的初始化
    /// </summary>
    /// <param name="appKeyForPC">平台申请的当前应用</param>
    /// <param name="channIdForPC">应用的渠道号</param>
    public static void Init(string appKeyForPC,string channIdForPC)
    {
        try
        {
            InitDumperCrasheye(appKeyForPC, YourAppVersion, channIdForPC);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 反初始化 CrasheyeSdk
    /// 在程序正常退出时调用
    /// </summary>
    public static void UnInit()
    {
        try
        {
            UnInitDumper();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 设置上报路径. 
    /// internal_cn => 国内自研项目
    /// internal_us => 海外自研项目
    /// external    => 外部项目
    /// 若不设置, 默认是 external
    /// 若要设置, 建议在 Init 之前调用.
    /// </summary>
    /// <param name="url">internal_cn/internal_us/external 三选一</param>
    public static void SetURL(string url)
    {
        if (String.IsNullOrEmpty(url))
        {
            Debug.LogError("set url is null or empty!");
            return;
        }

        var szLogDir = url.Replace("/", "\\");
        IntPtr pLogDir = Marshal.StringToHGlobalAnsi(szLogDir);
        SetConfig((int)DumpCommandType.enumDump_SetURL, pLogDir);
    }

    /// <summary>
    /// 标记当前版本为调试版本.
    /// 若要设置, 建议在 Init 之前调用.
    /// </summary>
    public static void SetBeta()
    {
        try
        {
            IntPtr pLogDir = default;
            SetConfig((int)DumpCommandType.enumDump_SetBeta, pLogDir);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        
    }

    /// <summary>
    /// 设置用户id. 
    /// </summary>
    /// <param name="userIdentifier">项目内部的用户id</param>
    public static void SetUserIdentifier(string userIdentifier)
    {
        if (string.IsNullOrEmpty(userIdentifier))
        {
            Debug.LogError("user identifier is null or empty!");
            return;
        }

        try
        {
            var szUserIdentifier = userIdentifier.Replace("/", "\\");
            IntPtr pUserIdentifier = Marshal.StringToHGlobalAnsi(szUserIdentifier);

            SetConfig((int)DumpCommandType.enumDump_SetUserIdentifier, pUserIdentifier);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 添加自定义数据,额外上报信息.
    /// 以键值对的形式添加额外信息, 添加的信息会被包含着崩溃报告中随报告一同上报.
    /// </summary>
    /// <param name="szKey">Key</param>
    /// <param name="szValue">Value</param>
    public static void AddExtraData(string szKey,string szValue)
    {
        try
        {
            extraData[0] = Marshal.StringToHGlobalAnsi(szKey);
            extraData[1] = Marshal.StringToHGlobalAnsi(szValue);

            SetConfig((int)DumpCommandType.enumDump_AddExtraData, extraData);

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 添加额外上报日志(文件).
    /// 添加的文件会被包含着崩溃报告中随报告一同上报.
    /// </summary>
    /// <param name="collectFile">需要额外收集的文件</param>
    public static void AddCustomLog(string collectFile)
    {
        try
        {
            var szCollectFile = collectFile.Replace("/", "\\");
            IntPtr pCollectFile = Marshal.StringToHGlobalAnsi(szCollectFile);

            SetConfig((int)DumpCommandType.enumDump_CollectFile, pCollectFile);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 设置版本号信息
    /// </summary>
    /// <param name="yourAppVersion"></param>
    public static void SetAppVersion(string yourAppVersion)
    {
        if (yourAppVersion == null)
        {
            return;
        }
        YourAppVersion = yourAppVersion;
    }
}
#endif
