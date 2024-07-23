using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UploadEvent : IEvent
{
    public override void OnEvent(params object[] objs) 
    {
        string ret = objs[0] as string;
        Debug.Log("UploadEvent!: " + ret.Count());

        FilePackage data = JsonMapper.ToObject<FilePackage>(ret);
        string savepath = Application.streamingAssetsPath + "/" + data.fileName;
        Tools.Bytes2File(data.fileData, savepath);
    }
}

/// <summary>
/// ÎÄ¼þ°ü
/// </summary>
public class FilePackage
{
    public string fileName;
    public byte[] fileData;
}

