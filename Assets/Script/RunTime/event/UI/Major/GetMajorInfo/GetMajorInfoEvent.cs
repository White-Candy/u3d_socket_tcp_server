using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class GetMajorInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;
            
            await UniTask.SwitchToMainThread();
            List<MajorInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.majorInfo);
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetMajorInfoEvent);
        });
    }
}