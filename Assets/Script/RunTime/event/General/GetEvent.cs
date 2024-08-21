using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class GetEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg pkg)
    {        
        JsonData jd = new JsonData();
        List<string> teachersList = new List<string>();
        List<string> facultiesList = new List<string>();

        foreach (var faculty in StorageHelper.Storage.faculiesInfo)
            facultiesList.Add(faculty.Name);

        jd["facultiesList"] = JsonMapper.ToJson(facultiesList);
        string inf = JsonMapper.ToJson(jd);
        NetworkTCPServer.SendAsync(pkg.socket, inf, EventType.GetEvent, OperateType.NONE);

        await UniTask.Yield();
    }
}