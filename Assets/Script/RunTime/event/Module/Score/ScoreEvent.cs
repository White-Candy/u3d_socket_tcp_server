
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class ScoreEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ScoreInfo> inf = await StorageHelper.GetInfo(StorageHelper.Storage.scoresInfo);
        
        string s_inf = JsonMapper.ToJson(inf);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.GET);
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
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        int index = StorageHelper.Storage.scoresInfo.FindIndex(x => x.userName == info.userName 
                            && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);
        if (index < 0 || index >= StorageHelper.Storage.scoresInfo.Count) 
        {
            StorageHelper.Storage.scoresInfo.Add(info);
            int examIdx = StorageHelper.Storage.examineesInfo.FindIndex(x => x.ColumnsName == info.columnsName && x.CourseName == info.courseName
                        && x.RegisterTime == info.registerTime);
            StorageHelper.Storage.examineesInfo[examIdx].PNum += 1;
        }
        else StorageHelper.Storage.scoresInfo[index] = info;
        
        string s_inf = JsonMapper.ToJson(StorageHelper.Storage.scoresInfo);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.REVISE);
        await UniTask.Yield();
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        List<ScoreInfo> infs = await StorageHelper.DeleteInfo(StorageHelper.Storage.scoresInfo, x => x.userName == info.userName 
                                && x.courseName == info.courseName && x.registerTime == info.registerTime && x.className == info.className);

        string s_inf = JsonMapper.ToJson(infs);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ScoreInfo info = JsonMapper.ToObject<ScoreInfo>(pkg.messPkg.ret);
        bool isSearch = false;
        List<ScoreInfo> inf = new List<ScoreInfo>();
        foreach (var scoreInf in StorageHelper.Storage.scoresInfo) {inf.Add(scoreInf.Clone());}
        Debug.Log($"info.className : {info.className} | info.Name: {info.Name} | info.courseName: {info.courseName} | info.registerTime: {info.registerTime}");
        if (info.className.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.className == info.className); isSearch = true; }
        if (info.Name.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.Name == info.Name); isSearch = true; }
        if (info.courseName.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.courseName == info.courseName); isSearch = true; }
        if (info.registerTime.Count() > 0) {inf = StorageHelper.SearchInf(inf, x => x.registerTime == info.registerTime); isSearch = true; }
        if (isSearch == false) inf.Clear();
        
        string s_inf = JsonMapper.ToJson(inf);
        HttpServer.HttpSendAsync(pkg.Context, s_inf, EventType.ScoreEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }
}