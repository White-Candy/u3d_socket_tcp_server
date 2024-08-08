using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class StorageExpand
{
    private static bool m_Init = false;

    private static StorageObject m_storage;
    public static StorageObject Storage
    {
        get
        {
            if (m_storage == null)
            {
                m_storage = Resources.Load("Storage/Clump") as StorageObject;
            }

            if (File.Exists(Application.persistentDataPath + "/Storage.json") && !m_Init)
            {
                string s_json = File.ReadAllText(Application.persistentDataPath + "/Storage.json");
                JsonUtility.FromJsonOverwrite(s_json, m_storage);
                m_Init = true;
            }
            return m_storage;
        }
    }

    /// <summary>
    /// 检查客户端的资源版本是否时最新的
    /// </summary>
    /// <param name="cli_info"></param>
    public static ResourcesInfo GetThisInfoPkg(ResourcesInfo cli_info)
    {
        return Storage.rsCheck.Find((x) => 
        {
            return (x.relaPath == cli_info.relaPath); 
        });
    }

    /// <summary>
    /// 保存这个文件的版本信息
    /// </summary>
    /// <param name="relative"></param>
    public static void UpdateThisFileInfo(string relative)
    {
        string[] st = relative.Split("\\");
        string id = st[0];
        string moudleName = st[1];

        int idx = Storage.rsCheck.FindIndex((x) => { return x.relaPath == relative; });
        if (idx != -1)
        {
            Storage.rsCheck.RemoveAt(idx);
        }

        ResourcesInfo ri = new ResourcesInfo();
        ri.relaPath = relative;
        ri.version_code = Tools.SpawnRandomCode();
        Storage.rsCheck.Add(ri);
        SaveToDisk();
    }

    /// <summary>
    /// 将 ScriptableObject 的内容保存到硬盘中
    /// </summary>
    public static void SaveToDisk()
    {
        // Debug.Log("SaveToDisk");
        string s_json = JsonMapper.ToJson(Storage);
        File.WriteAllText(Application.persistentDataPath + "/Storage.json", s_json);
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

        int account_idx = Storage.userInfos.FindIndex(x => x.userName == info.userName && x.level == info.level);
        if (account_idx != -1)
        {
            int pwd_idx = Storage.userInfos.FindIndex(x => x.userName == info.userName && x.password == info.password);
            if (pwd_idx != -1)
            {
                info.login = true;
                info.hint = "登录成功!";
            }
            else
            {
                info.hint = "密码错误!";
            }
        }
        else
        {
            info.hint = "用户名不存在!";
        }
        return info;
    }

    /// <summary>
    /// 注册请求
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public async static UniTask<UserInfo> Register(UserInfo inf)
    {
        await UniTask.SwitchToMainThread();
        if (Storage.userInfos.Find(x => x.userName == inf.userName) == null)
        {
            inf.hint = "注册成功!";
            Storage.userInfos.Add(inf);
            SaveToDisk();
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
    /// 获取学生的信息
    /// </summary>
    /// <returns></returns>
    public static async UniTask<List<UserInfo>> GetStudentsInfo()
    {
        List<UserInfo> students = new List<UserInfo>();

        await UniTask.SwitchToMainThread();
        foreach (UserInfo info in Storage.userInfos)
        {
            if (info.level == 0)
            {
                students.Add(info);
            }
        }
        return students;
    }
}
