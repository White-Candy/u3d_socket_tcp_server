using System.Collections.Generic;
using LitJson;

public class FacultyEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<FacultyInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.faculiesInfo);
        
        string s_infs = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, s_infs, EventType.GetFacInfoEvent);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.faculiesInfo, x => x.Name == info.Name);

        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.AddFacInfoEvent);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.faculiesInfo, x => x.id == info.id);
        
        string s_infs = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, s_infs, EventType.GetFacInfoEvent);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.Storage.faculiesInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.DeleteFacInfoEvent);
    }
}