using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UploadEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg) 
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            FilePackage data = JsonMapper.ToObject<FilePackage>(asynExPkg.messPkg.ret);
            string savepath = Application.streamingAssetsPath + "\\Data\\" + data.relativePath;
            Tools.Bytes2File(data.fileData, savepath);

            await UniTask.SwitchToMainThread();
            StorageHelper.UpdateThisFileInfo(data.relativePath);
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

