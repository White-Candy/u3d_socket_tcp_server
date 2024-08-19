using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class GetMajorInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;
            List<MajorInfo> infs = await StorageHelper.GetInfo<MajorStorageHelper>() as List<MajorInfo>;
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetFacInfoEvent);
        });
    }
}