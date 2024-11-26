using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class StorageHelper
{
    private static bool m_Init = false;

    private static StorageObject m_Storage;
    public static StorageObject storage
    {
        get
        {
            if (m_Storage == null)
                m_Storage = new StorageObject();
            return m_Storage;
        }
    }

    public static void Init()
    {
        HardDisk2Memory(FPath.STORAGE_USER, out storage.usersInfo);
        HardDisk2Memory(FPath.STORAGE_RESOURCE, out storage.rsCheck);
        HardDisk2Memory(FPath.STORAGE_FACULTY, out storage.faculiesInfo);
        HardDisk2Memory(FPath.STORAGE_MAJOR, out storage.majorInfo);
        HardDisk2Memory(FPath.STORAGE_CLASS, out storage.classesInfo);
        HardDisk2Memory(FPath.STORAGE_COLUMNS, out storage.columnsInfo);
        HardDisk2Memory(FPath.STORAGE_COURSE, out storage.courseInfo);
        HardDisk2Memory(FPath.STORAGE_EXAMINE, out storage.examineesInfo);
        HardDisk2Memory(FPath.STORAGE_SCORE, out storage.scoresInfo);
        // HardDisk2Memory(FPath.STORAGE_SOFTWARE, out storage.softwareInfo);
    }

    public static void Save()
    {
        Memory2HardDisk(FPath.STORAGE_USER, storage.usersInfo);
        Memory2HardDisk(FPath.STORAGE_RESOURCE, storage.rsCheck);
        Memory2HardDisk(FPath.STORAGE_FACULTY, storage.faculiesInfo);
        Memory2HardDisk(FPath.STORAGE_MAJOR, storage.majorInfo);
        Memory2HardDisk(FPath.STORAGE_CLASS, storage.classesInfo);
        Memory2HardDisk(FPath.STORAGE_COLUMNS, storage.columnsInfo);
        Memory2HardDisk(FPath.STORAGE_COURSE, storage.courseInfo);
        Memory2HardDisk(FPath.STORAGE_EXAMINE, storage.examineesInfo);
        Memory2HardDisk(FPath.STORAGE_SCORE, storage.scoresInfo);
        // Memory2HardDisk(FPath.STORAGE_SOFTWARE, storage.softwareInfo);
    }

    private static void HardDisk2Memory<T>(string filePath, out List<T> targetList)
    {
        string jsonString = FileHelper.ReadTextFile(filePath);
        if (jsonString.Count() == 0)
        {
            targetList = new List<T>();
            return;
        }

        targetList = JsonMapper.ToObject<List<T>>(jsonString);
    }

    private static void Memory2HardDisk<T>(string savePath, List<T> saveList)
    {
        if (saveList == null) return;
        string json = JsonMapper.ToJson(saveList);
        //Console.WriteLine($"{savePath}: {json}");
        FileHelper.WriteTextFile(savePath, json);
    }

    /// <summary>
    /// 检查客户端的资源版本是否时最新的
    /// </summary>
    /// <param name="cli_info"></param>
    public static ResourcesInfo GetThisInfoPkg(ResourcesInfo cli_info)
    {
        return storage.rsCheck.Find((x) => 
        {
            return (x.relaPath == cli_info.relaPath); 
        });
    }

    /// <summary>
    /// 保存这个文件的版本信息
    /// </summary>
    /// <param name="relative"></param>
    public static async void UpdateThisFileInfo(string relative)
    {
        string[] st = relative.Split("\\");
        string id = st[0];
        string moudleName = st[1];

        int idx = storage.rsCheck.FindIndex((x) => { return x.relaPath == relative; });
        if (idx != -1)
        {
            storage.rsCheck.RemoveAt(idx);
        }

        ResourcesInfo ri = new ResourcesInfo();
        ri.relaPath = relative;
        ri.version_code = Tools.SpawnRandomCode();
        storage.rsCheck.Add(ri);
        await SaveToDisk();
    }

    /// <summary>
    /// 将 ScriptableObject 的内容保存到硬盘中
    /// </summary>
    public static async UniTask SaveToDisk()
    {
        await UniTask.SwitchToMainThread();
        // Debug.Log("SaveToDisk");
        string s_json = JsonMapper.ToJson(storage);
        File.WriteAllText(Application.streamingAssetsPath + "/storage.json", s_json);
    }

    /// <summary>
    /// 检查用户登录
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async static UniTask<UserInfo> CheckUserLogin(UserInfo info)
    {
        await UniTask.SwitchToMainThread();
        UserInfo usrInfo = info;
        int account_idx = storage.usersInfo.FindIndex(x => x.userName == info.userName);
        if (account_idx != -1)
        {
            int pwd_idx = storage.usersInfo.FindIndex(x => x.userName == info.userName && x.password == info.password);
            if (pwd_idx != -1)
            {
                usrInfo.Name = storage.usersInfo[pwd_idx].Name;
                usrInfo.Gender = storage.usersInfo[pwd_idx].Gender;
                usrInfo.Age = storage.usersInfo[pwd_idx].Age;
                usrInfo.Identity = storage.usersInfo[pwd_idx].Identity;
                usrInfo.idCoder = storage.usersInfo[pwd_idx].idCoder;
                usrInfo.Contact = storage.usersInfo[pwd_idx].Contact;
                usrInfo.UnitName = storage.usersInfo[pwd_idx].UnitName;
                usrInfo.login = true;
                usrInfo.hint = "登录成功";
            }
            else
            {
                usrInfo.hint = "密码错误";
            }
        }
        else
        {
            usrInfo.hint = "用户名不存在";
        }
        return usrInfo;
    }

    /// <summary>
    /// 注册请求
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public async static UniTask<UserInfo> Register(UserInfo inf)
    {
        await UniTask.SwitchToMainThread();
        if (storage.usersInfo.Find(x => x.userName == inf.userName) == null)
        {
            inf.hint = "注册成功!";
            storage.usersInfo.Add(inf);
            await SaveToDisk();
        }
        else
        {
            //如果注册失败清空inf中的数据
            inf.userName = "";
            inf.password = "";
            inf.hint = "该用户名存在!";
        }

        return inf;
    }

    /// <summary>
    /// 获取信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async UniTask<List<T>> GetInfo<T>(List<T> storInfo) where T : BaseInfo
    {
        List<T> info = new List<T>();

        await UniTask.SwitchToMainThread();
        foreach (T inf in storInfo)
        {
            info.Add(inf);
        }
        return info;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static async UniTask<List<T>> AddInfo<T>(T inf, List<T> storInfo, Predicate<T> match = default) where T : BaseInfo
    {
        if (storInfo.Find(match) == null)
        {
            storInfo.Add(inf);
        }
        await SaveToDisk();
        return storInfo;
    }

    public static async UniTask<List<T>> AddInfo<T>(List<T> l_inf, List<T> storInfo) where T : UserInfo
    {
        foreach (var inf in l_inf)
        {
            await Register(inf);
        }
        await SaveToDisk();
        return storInfo;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
     public static async UniTask<List<T>> ReviseInfo<T>(T inf, List<T> storInfo,  Predicate<T> match) where T : BaseInfo
     {
        int index = storInfo.FindIndex(match);
        Debug.Log("ReviseInfo: " + index);
        if (index != -1)
        {
            storInfo[index] = inf;
            return storInfo;
        }
        await SaveToDisk();
        return new List<T>();
     }

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
     public static async UniTask<List<T>> DeleteInfo<T>(List<T> storInfo, Predicate<T> match) where T : BaseInfo
     {
        int idx = storInfo.FindIndex(match);
        if (idx != -1)
        {
            storInfo.RemoveAt(idx);
            return storInfo;
        }
        await SaveToDisk();
        return new List<T>();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="storInf"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static List<T> SearchInf<T> (List<T> storInf,  Predicate<T> match) where T : BaseInfo
    {
        List<T> list = new List<T>();
        list = storInf.FindAll(match);
        return list;
    }
}