using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine.Pool;
public class ScoreEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ScoreInfo> inf = await StorageHelper.GetInfo(StorageHelper.Storage.scoresInfo);
        
        string s_inf = JsonMapper.ToJson(inf);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.ScoreEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        List<ScoreInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.scoresInfo, x => x.userName == info.userName 
                                && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);

        string s_inf = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.ScoreEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        List<ScoreInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.scoresInfo, x => x.userName == info.userName 
                                && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);
        
        string s_inf = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.ScoreEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        List<ScoreInfo> infs = await StorageHelper.DeleteInfo(StorageHelper.Storage.scoresInfo, x => x.userName == info.userName 
                                && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);

        string s_inf = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.ScoreEvent, OperateType.DELETE);
    }
}