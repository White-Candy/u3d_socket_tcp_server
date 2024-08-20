using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections.Generic;

public class ReviseFacInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

            FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(asynExPkg.messPkg.ret);
            List<FacultyInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.faculiesInfo, x => x.id == info.id);
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetFacInfoEvent);
        });
    }
}