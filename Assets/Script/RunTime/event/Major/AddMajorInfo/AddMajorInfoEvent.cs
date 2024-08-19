using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class AddMajorInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        await UniTask.RunOnThreadPool( async () =>
        {
            MajorInfo majorinfo = JsonMapper.ToObject<MajorInfo>(asynExPkg.messPkg.ret);
            List<MajorInfo> new_list = await StorageHelper.AddInfo<MajorStorageHelper>(majorinfo) as List<MajorInfo>;

            string body = JsonMapper.ToJson(new_list);
            NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.AddMajorInfoEvent);
        });
    }
}