using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public static class StorageHelper
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
    /// �� ScriptableObject �����ݱ��浽Ӳ����
    /// </summary>
    public static void SaveToDisk()
    {
        // Debug.Log("SaveToDisk");
        string s_json = JsonMapper.ToJson(Storage);
        File.WriteAllText(Application.persistentDataPath + "/Storage.json", s_json);
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

        int account_idx = Storage.userInfos.FindIndex(x => x.userName == info.userName && x.level == info.level);
        if (account_idx != -1)
        {
            int pwd_idx = Storage.userInfos.FindIndex(x => x.userName == info.userName && x.password == info.password);
            if (pwd_idx != -1)
            {
                info.login = true;
                info.hint = "��¼�ɹ�!";
            }
            else
            {
                info.hint = "�������!";
            }
        }
        else
        {
            info.hint = "�û���������!";
        }
        return info;
    }

    /// <summary>
    /// ע������
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public async static UniTask<UserInfo> Register(UserInfo inf)
    {
        await UniTask.SwitchToMainThread();
        if (Storage.userInfos.Find(x => x.userName == inf.userName) == null)
        {
            inf.hint = "ע��ɹ�!";
            Storage.userInfos.Add(inf);
            SaveToDisk();
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
    /// ��ȡѧ������Ϣ
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

    /// <summary>
    /// ���ѧ����Ϣ��Ӳ����
    /// </summary>
    /// <param name="l_inf"></param>
    /// <returns></returns>
    public static async UniTask<List<UserInfo>> AddStusInfo(List<UserInfo> l_inf)
    {
        foreach (var inf in l_inf)
        {
            await Register(inf);
        }
        return Storage.userInfos;
    }

    /// <summary>
    /// �޸�ѧ����Ϣ
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public static async UniTask<List<UserInfo>> ReviseStuInfo(UserInfo inf)
    {
        await UniTask.SwitchToMainThread();

        int index = Storage.userInfos.FindIndex(x => x.userName == inf.userName);
        if (index != -1)
        {
            Storage.userInfos[index] = inf;
            return Storage.userInfos;
        }
        return new List<UserInfo>();
    }

    /// <summary>
    /// ɾ��ѧ����Ϣ
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public static List<UserInfo> DeleteStuInfo(UserInfo inf)
    {
        int idx = Storage.userInfos.FindIndex(x => x.userName == inf.userName);
        if (idx != -1)
        {
            Storage.userInfos.RemoveAt(idx);
            return Storage.userInfos;
        }
        return new List<UserInfo>();
    }

    /// <summary>
    /// ��ȡѧԺ��Ϣ
    /// </summary>
    /// <returns></returns>
    public static async UniTask<List<FacultyInfo>> GetFaculiesInfo()
    {
        List<FacultyInfo> faculies = new List<FacultyInfo>();

        await UniTask.SwitchToMainThread();
        foreach (FacultyInfo info in Storage.faculiesInfo)
        {
            faculies.Add(info);   
        }
        return faculies;
    }

    /// <summary>
    /// ���ѧԺ��Ϣ��Ӳ����
    /// </summary>
    /// <param name="l_inf"></param>
    /// <returns></returns>
    public static async UniTask<List<FacultyInfo>> AddFacInfo(FacultyInfo inf)
    {
        await UniTask.SwitchToMainThread();
        
        if (Storage.faculiesInfo.Find(x => x.Name == inf.Name) == null)
        {
            Storage.faculiesInfo.Add(inf);
        }
        return Storage.faculiesInfo;
    }

    /// <summary>
    /// �޸�ѧԺ��Ϣ
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public static async UniTask<List<FacultyInfo>> ReviseFacInfo(FacultyInfo inf)
    {
        await UniTask.SwitchToMainThread();
      
        int index = Storage.faculiesInfo.FindIndex(x => x.id == inf.id);
        if (index == -1)
        {
            Storage.faculiesInfo[index] = inf;
            return Storage.faculiesInfo;
        }
        return new List<FacultyInfo>();
    }

    /// <summary>
    /// ɾ��ѧԺ��Ϣ
    /// </summary>
    /// <param name="inf"></param>
    /// <returns></returns>
    public static List<FacultyInfo> DeleteFacInfo(FacultyInfo inf)
    {
        int idx = Storage.faculiesInfo.FindIndex(x => x.id == inf.id);
        if (idx != -1)
        {
            Storage.faculiesInfo.RemoveAt(idx);
            return Storage.faculiesInfo;
        }
        return new List<FacultyInfo>();
    }
}
