using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.Pool;
public class UserEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {         
        List<StuInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.userInfos);
        
        string s_inf = JsonMapper.ToJson(infs); 
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.UserEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        List<StuInfo> usersList = JsonMapper.ToObject<List<StuInfo>>(pkg.messPkg.ret);
        List<StuInfo> new_list = await StorageHelper.AddInfo(usersList, StorageHelper.Storage.userInfos);

        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.UserEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        StuInfo inf = JsonMapper.ToObject<StuInfo>(pkg.messPkg.ret);
        List<StuInfo> ls_inf = await StorageHelper.ReviseInfo(inf, StorageHelper.Storage.userInfos, x => x.userName == inf.userName);

        string s_inf = JsonMapper.ToJson(ls_inf);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.UserEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        StuInfo userInfo = JsonMapper.ToObject<StuInfo>(pkg.messPkg.ret);
        List<StuInfo> new_list = await StorageHelper.DeleteInfo(StorageHelper.Storage.userInfos, x => x.userName == userInfo.userName);
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.UserEvent, OperateType.DELETE);
    }
}