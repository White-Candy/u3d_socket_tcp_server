using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class ColumnsEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ColumnsInfo> infs = await StorageHelper.GetInfo(StorageHelper.storage.columnsInfo);
        
        string inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, inf, EventType.ColumnsEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.storage.columnsInfo, x => x.Name == info.Name);

        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.ColumnsEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.storage.columnsInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.ColumnsEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> new_list = new List<ColumnsInfo>(StorageHelper.storage.columnsInfo);

        int i = -1;
        i = StorageHelper.storage.courseInfo.FindIndex(x => x.Columns == info.Name);
        if (i == -1) new_list = await StorageHelper.DeleteInfo(StorageHelper.storage.columnsInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.ColumnsEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> inf = StorageHelper.SearchInf(StorageHelper.storage.columnsInfo, x => x.Name == info.Name);

        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.ColumnsEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }      
}