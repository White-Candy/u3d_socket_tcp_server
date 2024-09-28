using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class FacultyEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<FacultyInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.faculiesInfo);
        
        string inf = JsonMapper.ToJson(infs);
        HttpServer.HttpSendAsync(pkg.Context, inf, EventType.FacultyEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.faculiesInfo, x => x.Name == info.Name);

        string body = JsonMapper.ToJson(new_list);
        HttpServer.HttpSendAsync(pkg.Context, body, EventType.FacultyEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.faculiesInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.FacultyEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> new_list = new List<FacultyInfo>(StorageHelper.Storage.faculiesInfo);
        int i = -1;
        i = StorageHelper.Storage.majorInfo.FindIndex(x => x.FacultyName == info.Name);
        if (i == -1)
        {
            new_list = 
                await StorageHelper.DeleteInfo(StorageHelper.Storage.faculiesInfo, (x) => {return x.id == info.id;});
        }
        
        
        string body = JsonMapper.ToJson(new_list);
        HttpServer.HttpSendAsync(pkg.Context, body, EventType.FacultyEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(pkg.messPkg.ret);
        List<FacultyInfo> inf = StorageHelper.SearchInf(StorageHelper.Storage.faculiesInfo, x => x.Name == info.Name);

        string s_inf = JsonMapper.ToJson(inf);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.FacultyEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}