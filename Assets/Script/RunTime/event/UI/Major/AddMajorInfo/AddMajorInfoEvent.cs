using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class AddMajorInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        await UniTask.RunOnThreadPool( async () =>
        {
            await UniTask.SwitchToMainThread();
            MajorInfo info = JsonMapper.ToObject<MajorInfo>(asynExPkg.messPkg.ret);
            List<MajorInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.majorInfo, x => x.MajorName == info.MajorName);

            string body = JsonMapper.ToJson(new_list);
            NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.AddMajorInfoEvent);
        });
    }
}