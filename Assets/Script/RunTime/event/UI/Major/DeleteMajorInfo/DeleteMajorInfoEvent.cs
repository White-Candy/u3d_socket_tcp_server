using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class DeleteMajorInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        await UniTask.SwitchToMainThread();
        MajorInfo info = JsonMapper.ToObject<MajorInfo>(asynExPkg.messPkg.ret);
        List<MajorInfo> infs = await StorageHelper.DeleteInfo(StorageHelper.Storage.majorInfo, x => x.id == info.id);

        string s_infs = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.DeleteMajorInfoEvent);

        await UniTask.Yield();
    }
}