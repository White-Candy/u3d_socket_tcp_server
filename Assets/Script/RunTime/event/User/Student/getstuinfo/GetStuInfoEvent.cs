using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections.Generic;

public class GetStuInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;
            
            List<UserInfo> infs = await StorageHelper.GetInfo<StudentStorageHelper>() as List<UserInfo>;
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetStuInfoEvent);
        });
    }
}