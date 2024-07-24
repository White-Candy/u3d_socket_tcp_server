using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLoadEvent : IEvent
{
    public override async void OnEvent(params object[] objs) 
    {
        var expand_pkg = objs[0] as AsyncExpandPkg;
        if (expand_pkg.messPkg.ret != null) 
        {
            FileReqPkg pkg = JsonMapper.ToObject<FileReqPkg>(expand_pkg.messPkg.ret);
            string path = $"{Application.streamingAssetsPath}/Data/{pkg.filepath}";
            var file_bytes = await Tools.File2Bytes(path);
        }
    }
}

/// <summary>
/// 文件请求包
/// </summary>
public class FileReqPkg
{
    public string filepath;
}

