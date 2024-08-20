using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections.Generic;

public class GetFacInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;
            await UniTask.SwitchToMainThread();
            List<FacultyInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.faculiesInfo);
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetFacInfoEvent);
        });
    }
}