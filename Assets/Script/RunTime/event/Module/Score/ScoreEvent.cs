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

        int examIdx = StorageHelper.Storage.examineesInfo.FindIndex(x => x.ColumnsName == info.columnsName && x.CourseName == info.courseName
            && x.RegisterTime == info.registerTime);
        StorageHelper.Storage.examineesInfo[examIdx].PNum += 1;
        
        string s_inf = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.ScoreEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        int index = StorageHelper.Storage.scoresInfo.FindIndex(x => x.userName == info.userName 
                            && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);
        if (index == -1) 
        {
            StorageHelper.Storage.scoresInfo.Add(info);
            int examIdx = StorageHelper.Storage.examineesInfo.FindIndex(x => x.ColumnsName == info.columnsName && x.CourseName == info.courseName
                        && x.RegisterTime == info.registerTime);
            StorageHelper.Storage.examineesInfo[examIdx].PNum += 1;
        }
        else StorageHelper.Storage.scoresInfo[index] = info;
        
        string s_inf = JsonMapper.ToJson(StorageHelper.Storage.scoresInfo);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.ScoreEvent, OperateType.REVISE);
        await UniTask.Yield();
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