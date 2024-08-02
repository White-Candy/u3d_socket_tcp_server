using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLoadEvent : IEvent
{
    public override async void OnEvent(params object[] objs) 
    {
        Debug.Log("DownLoadEvent!");
        var expand_pkg = objs[0] as AsyncExpandPkg;
        if (expand_pkg.messPkg.ret != null) 
        {
            FileReqPkg pkg = JsonMapper.ToObject<FileReqPkg>(expand_pkg.messPkg.ret);
            string rela = pkg.relaPath;

            string[] split_str = rela.Split('\\');
            string name = split_str[split_str.Length - 1];
            string path = $"{Application.streamingAssetsPath}/Data/{pkg.relaPath}";

            DataSendPkg data = new DataSendPkg()
            { 
                fileName = name,
                relativePath = rela,
                fileData = await Tools.File2Bytes(path)
            };

            string str_data = JsonMapper.ToJson(data);
            NetworkTCPServer.SendAsync(expand_pkg.socket, str_data, EventType.DownLoadEvent);
        }
    }
}

/// <summary>
/// �ļ������
/// </summary>
public class FileReqPkg
{
    public string relaPath;
}

public class DataSendPkg
{
    public string fileName;
    public string relativePath; // ���·��
    public byte[] fileData;
}
