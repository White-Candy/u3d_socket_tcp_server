using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections.Generic;

public class GetStuInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;
            
            await UniTask.SwitchToMainThread();
            List<StuInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.userInfos);
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetStuInfoEvent);
        });
    }
}