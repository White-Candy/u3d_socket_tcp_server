using Cysharp.Threading.Tasks;
using LitJson;
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
    public async static UniTask<UserInfo> CheckUserLogin(string username, string password)
    {
        await UniTask.SwitchToMainThread();

        UserInfo inf = new UserInfo();
        int account_idx = Storage.userInfos.FindIndex(x => x.userName == username);
        if (account_idx != -1)
        {
            int pwd_idx = Storage.userInfos.FindIndex(x => x.userName == username && x.password == password);
            if (pwd_idx != -1)
            {
                inf = Storage.userInfos[pwd_idx];
                inf.login = true;
                inf.hint = "��¼�ɹ�!";
            }
            else
            {
                inf.hint = "�������!";
            }
        }
        else
        {
            inf.hint = "�û���������!";
        }
        return inf;
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
}
