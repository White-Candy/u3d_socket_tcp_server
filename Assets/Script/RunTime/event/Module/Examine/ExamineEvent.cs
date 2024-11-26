using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Cysharp.Threading.Tasks;

public class ExamineEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ExamineInfo> infs = await StorageHelper.GetInfo(StorageHelper.storage.examineesInfo);
        
        string inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, inf, EventType.ExamineEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ExamineInfo info = JsonMapper.ToObject<ExamineInfo>(pkg.messPkg.ret);
        List<ExamineInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.storage.examineesInfo, 
            x => x.CourseName == info.CourseName && x.RegisterTime == info.RegisterTime);

        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.ExamineEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        List<ExamineInfo> infoList = JsonMapper.ToObject<List<ExamineInfo>>(pkg.messPkg.ret);
        foreach (var info in infoList)
        {
            List<ExamineInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.storage.examineesInfo, x => x.id == info.id);   
            string s_inf = JsonMapper.ToJson(inf);
            SocketServer.SendAsync(pkg.socket, s_inf, EventType.ExamineEvent, OperateType.REVISE);
        }
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ExamineInfo info = JsonMapper.ToObject<ExamineInfo>(pkg.messPkg.ret);
        List<ExamineInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.storage.examineesInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.ExamineEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ExamineInfo info = JsonMapper.ToObject<ExamineInfo>(pkg.messPkg.ret);
        List<ExamineInfo> inf = StorageHelper.SearchInf(StorageHelper.storage.examineesInfo, x => x.CourseName == info.CourseName);

        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.ExamineEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }      
}