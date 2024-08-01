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

            if (File.Exists(Application.persistentDataPath + "Storage.json") && !m_Init)
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
}
