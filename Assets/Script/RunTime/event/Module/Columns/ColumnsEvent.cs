using System.Collections.Generic;
using LitJson;

public class ColumnsEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ColumnsInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.columnsInfo);
        
        string inf = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, inf, EventType.ColumnsEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.columnsInfo, x => x.Name == info.Name);

        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.ColumnsEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.columnsInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.ColumnsEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ColumnsInfo info = JsonMapper.ToObject<ColumnsInfo>(pkg.messPkg.ret);
        List<ColumnsInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.Storage.columnsInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.ColumnsEvent, OperateType.DELETE);
    }
}