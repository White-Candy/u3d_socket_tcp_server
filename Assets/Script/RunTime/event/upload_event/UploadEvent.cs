using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UploadEvent : IEvent
{
    public override async void OnEvent(params object[] objs) 
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            AsyncExpandPkg ret = objs[0] as AsyncExpandPkg;
            Debug.Log("UploadEvent!");

            FilePackage data = JsonMapper.ToObject<FilePackage>(ret.messPkg.ret);
            string savepath = Application.streamingAssetsPath + "\\Data\\" + data.relativePath;
            Tools.Bytes2File(data.fileData, savepath);

            await UniTask.SwitchToMainThread();
            StorageExpand.UpdateThisFileInfo(data.relativePath);
        });
    }
}

/// <summary>
/// ÎÄ¼þ°ü
/// </summary>
public class FilePackage
{
    public string fileName;
    public string relativePath;
    public byte[] fileData;
}

