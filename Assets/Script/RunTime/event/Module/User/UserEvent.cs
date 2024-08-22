using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.Pool;
public class UserEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {         
        List<UserInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.usersInfo);
        
        string s_inf = JsonMapper.ToJson(infs); 
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.UserEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        List<UserInfo> usersList = JsonMapper.ToObject<List<UserInfo>>(pkg.messPkg.ret);
        List<UserInfo> new_list = await StorageHelper.AddInfo(usersList, StorageHelper.Storage.usersInfo);

        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.UserEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        UserInfo inf = JsonMapper.ToObject<UserInfo>(pkg.messPkg.ret);
        List<UserInfo> ls_inf = await StorageHelper.ReviseInfo(inf, StorageHelper.Storage.usersInfo, x => x.userName == inf.userName);

        string s_inf = JsonMapper.ToJson(ls_inf);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.UserEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        UserInfo userInfo = JsonMapper.ToObject<UserInfo>(pkg.messPkg.ret);
        List<UserInfo> new_list = await StorageHelper.DeleteInfo(StorageHelper.Storage.usersInfo, x => x.userName == userInfo.userName);
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.UserEvent, OperateType.DELETE);
    }
}