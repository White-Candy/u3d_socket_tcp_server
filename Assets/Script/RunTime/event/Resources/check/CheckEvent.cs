using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg expand_pkg)
    {
        UpdatePackage up = JsonMapper.ToObject<UpdatePackage>(expand_pkg.messPkg.ret);
        string relativePath = up.relativePath;
        Debug.Log("CheckEvent relativePath: " + up.relativePath);
        List<ResourcesInfo> bufInfo = new List<ResourcesInfo>();
        foreach (var inf in StorageHelper.Storage.rsCheck)
        {
            ResourcesInfo info = new ResourcesInfo(inf);
            if (inf.relaPath.Contains(relativePath))
            {
                bufInfo.Add(info);
            }
        }

        foreach (var inf in bufInfo)
        {
            int i = up.filesInfo.FindIndex(x => x.relaPath == inf.relaPath);
            if (i >= 0 && i < up.filesInfo.Count)
                inf.need_updata = up.filesInfo[i].version_code != inf.version_code ? true : false;
            else
                inf.need_updata = true;
        }
        up.filesInfo = bufInfo;

        foreach (var info in up.filesInfo)
        {
            Debug.Log("===================================== info: " + info.relaPath);
        }

        string body = await JsonHelper.AsyncToJson(up);
        NetworkTCPServer.SendAsync(expand_pkg.socket, body, EventType.CheckEvent, OperateType.NONE);
        await UniTask.Yield();
    }
}

public class UpdatePackage
{
    public string relativePath;
    public List<ResourcesInfo> filesInfo = new List<ResourcesInfo>();
}
