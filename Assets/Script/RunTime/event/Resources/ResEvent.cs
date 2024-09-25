using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ResEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ResourcesInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.rsCheck);
        
        string inf = JsonMapper.ToJson(infs);
        NetworkTCPServer.HttpSendAsync(pkg.Context, inf, EventType.ResEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ResourcesInfo info = JsonMapper.ToObject<ResourcesInfo>(pkg.messPkg.ret);
        List<ResourcesInfo> new_list = new List<ResourcesInfo>(StorageHelper.Storage.rsCheck);

        int i = -1;
        i = StorageHelper.Storage.rsCheck.FindIndex(x => x.relaPath == info.relaPath);
        if (i != -1) 
        {
            string deletePath = Application.streamingAssetsPath + "\\Data\\" + info.relaPath;
            File.Delete(deletePath);
            new_list = await StorageHelper.DeleteInfo(StorageHelper.Storage.rsCheck, (x) => {return x.relaPath == info.relaPath;});
        }
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.HttpSendAsync(pkg.Context, body, EventType.ResEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }      
}