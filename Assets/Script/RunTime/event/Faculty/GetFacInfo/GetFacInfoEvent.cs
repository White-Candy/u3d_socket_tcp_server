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
            List<FacultyInfo> infs = await StorageHelper.GetInfo<FacultyStorageHelper>() as List<FacultyInfo>;
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetFacInfoEvent);
        });
    }
}