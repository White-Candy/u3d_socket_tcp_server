using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine.Pool;
public class MajorEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<MajorInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.majorInfo);
        
        string s_infs = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, s_infs, EventType.GetMajorInfoEvent);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.majorInfo, x => x.MajorName == info.MajorName);

        string s_inf = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.AddMajorInfoEvent);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.majorInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.GetMajorInfoEvent);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(pkg.messPkg.ret);
        List<MajorInfo> infs = await StorageHelper.DeleteInfo(StorageHelper.Storage.majorInfo, x => x.id == info.id);

        string s_inf = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.DeleteMajorInfoEvent);
    }
}