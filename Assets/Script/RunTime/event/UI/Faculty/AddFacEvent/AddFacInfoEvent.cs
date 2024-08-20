using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class AddFacInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("AddFacInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        await UniTask.RunOnThreadPool( async () =>
        {
            FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(asynExPkg.messPkg.ret);
            await UniTask.SwitchToMainThread();
            List<FacultyInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.faculiesInfo, x => x.Name == info.Name);

            string body = JsonMapper.ToJson(new_list);
            Debug.Log($"new list after Addstuevent : {body}");
            NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.AddFacInfoEvent);
        });
    }
}

// @event.OnEvent