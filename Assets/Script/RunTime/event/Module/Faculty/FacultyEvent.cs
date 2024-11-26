using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class FacultyEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<FacultyInfo> infs = await StorageHelper.GetInfo(StorageHelper.storage.faculiesInfo);
        
        string inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, inf, EventType.FacultyEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.storage.faculiesInfo, x => x.Name == info.Name);

        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.FacultyEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.storage.faculiesInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.FacultyEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = new List<FacultyInfo>(StorageHelper.storage.faculiesInfo);
        int i = -1;
        i = StorageHelper.storage.majorInfo.FindIndex(x => x.FacultyName == info.Name);
        if (i == -1)
        {
            new_list = 
                await StorageHelper.DeleteInfo(StorageHelper.storage.faculiesInfo, (x) => {return x.id == info.id;});
        }
        
        
        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.FacultyEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> inf = StorageHelper.SearchInf(StorageHelper.storage.faculiesInfo, x => x.Name == info.Name);

        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.FacultyEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}