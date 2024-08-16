using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections.Generic;

public class GetFacInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;
            List<FacultyInfo> infs = await StorageHelper.GetFaculiesInfo();
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetFacInfoEvent);
        });
    }
}

/// <summary>
///  学院信息包
/// </summary>
[Serializable]
public class FacultyInfo
{
    public string id;
    public string Name;
    public string RegisterTime;
    public string TeacherName;
}