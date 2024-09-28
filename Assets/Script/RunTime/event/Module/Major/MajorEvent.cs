using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine.Pool;
public class MajorEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<MajorInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.majorInfo);
        
        string s_inf = JsonMapper.ToJson(infs);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.majorInfo, x => x.MajorName == info.MajorName);

        string s_inf = JsonMapper.ToJson(new_list);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.majorInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(infs);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = new List<MajorInfo>(StorageHelper.Storage.majorInfo);

        int i = -1;
        i = StorageHelper.Storage.classesInfo.FindIndex(x => x.Major == info.MajorName);
        if (i == -1) { infs = await StorageHelper.DeleteInfo(StorageHelper.Storage.majorInfo, x => x.id == info.id); }

        string s_inf = JsonMapper.ToJson(infs);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> inf = StorageHelper.SearchInf(StorageHelper.Storage.majorInfo, x => x.MajorName == info.MajorName);

        string s_inf = JsonMapper.ToJson(inf);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.MajorEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}