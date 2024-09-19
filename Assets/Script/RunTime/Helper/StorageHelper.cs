using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class StorageHelper
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
    /// ���ͻ��˵���Դ�汾�Ƿ�ʱ���µ�
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
    /// ��������ļ��İ汾��Ϣ
    /// </summary>
    /// <param name="relative"></param>
    public static async void UpdateThisFileInfo(string relative)
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
        await SaveToDisk();
    }

    /// <summary>
    /// �� ScriptableObject �����ݱ��浽Ӳ����
    /// </summary>
    public static async UniTask SaveToDisk()
    {
        await UniTask.SwitchToMainThread();
        // Debug.Log("SaveToDisk");
        string s_json = JsonMapper.ToJson(Storage);
        File.WriteAllText(Application.streamingAssetsPath + "/Storage.json", s_json);
    }

    /// <summary>
    /// ����û���¼
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async static UniTask<UserInfo> CheckUserLogin(UserInfo info)
    {
        await UniTask.SwitchToMainThread();
        UserInfo usrInfo = info;
        int account_idx = Storage.usersInfo.FindIndex(x => x.userName == info.userName);
        if (account_idx != -1)
        {
            int pwd_idx = Storage.usersInfo.FindIndex(x => x.userName == info.userName && x.password == info.password);
            if (pwd_idx != -1)
            {
                usrInfo.Name = Storage.usersInfo[pwd_idx].Name;
                usrInfo.Gender = Storage.usersInfo[pwd_idx].Gender;
                usrInfo.Age = Storage.usersInfo[pwd_idx].Age;
                usrInfo.Identity = Storage.usersInfo[pwd_idx].Identity;
                usrInfo.idCoder = Storage.usersInfo[pwd_idx].idCoder;
                usrInfo.Contact = Storage.usersInfo[pwd_idx].Contact;
                usrInfo.UnitName = Storage.usersInfo[pwd_idx].UnitName;
                usrInfo.login = true;
                usrInfo.hint = "��¼�ɹ�";
            }
            else
            {
                usrInfo.hint = "�������";
            }
        }
        else
        {
            usrInfo.hint = "�û���������";
        }
        return usrInfo;
    }

    /// <summary>
    /// ע������
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public async static UniTask<UserInfo> Register(UserInfo inf)
    {
        await UniTask.SwitchToMainThread();
        if (Storage.usersInfo.Find(x => x.userName == inf.userName) == null)
        {
            inf.hint = "ע��ɹ�!";
            Storage.usersInfo.Add(inf);
            await SaveToDisk();
        }
        else
        {
            //���ע��ʧ�����inf�е�����
            inf.userName = "";
            inf.password = "";
            inf.hint = "���û�������!";
        }

        return inf;
    }

    /// <summary>
    /// ��ȡ��Ϣ
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
    /// ���
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
    /// �޸�
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
    /// ɾ��
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
    /// ����
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