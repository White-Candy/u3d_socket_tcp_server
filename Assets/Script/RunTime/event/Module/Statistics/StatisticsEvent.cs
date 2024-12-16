using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.Pool;
public class StatisticsEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        Debug.Log("Statics Get Info Event..");
        List<UsrTimeInfo> infs = await StorageHelper.GetInfo(StorageHelper.storage.usrTimeInfo);

        string s_inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.StatisticsEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg) { }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg) { }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        UsrTimeInfo userInfo = JsonMapper.ToObject<UsrTimeInfo>(pkg.messPkg.ret);
        List<UsrTimeInfo> new_list = await StorageHelper.DeleteInfo(StorageHelper.storage.usrTimeInfo, x => x.usrName == userInfo.usrName && x.moduleName == userInfo.moduleName);

        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.StatisticsEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg) { }
}