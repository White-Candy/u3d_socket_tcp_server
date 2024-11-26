using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine.Pool;
public class MajorEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<MajorInfo> infs = await StorageHelper.GetInfo(StorageHelper.storage.majorInfo);
        
        string s_inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.MajorEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.storage.majorInfo, x => x.MajorName == info.MajorName);

        string s_inf = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.MajorEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.storage.majorInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.MajorEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = new List<MajorInfo>(StorageHelper.storage.majorInfo);

        int i = -1;
        i = StorageHelper.storage.classesInfo.FindIndex(x => x.Major == info.MajorName);
        if (i == -1) { infs = await StorageHelper.DeleteInfo(StorageHelper.storage.majorInfo, x => x.id == info.id); }

        string s_inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.MajorEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> inf = StorageHelper.SearchInf(StorageHelper.storage.majorInfo, x => x.MajorName == info.MajorName);

        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.MajorEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}