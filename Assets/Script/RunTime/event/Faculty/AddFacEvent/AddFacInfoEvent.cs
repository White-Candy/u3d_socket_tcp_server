using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class AddFacInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("AddFacInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        await UniTask.RunOnThreadPool( async () =>
        {
            FacultyInfo facultyinfo = JsonMapper.ToObject<FacultyInfo>(asynExPkg.messPkg.ret);
            List<FacultyInfo> new_list = await StorageHelper.AddInfo<FacultyStorageHelper>(facultyinfo) as List<FacultyInfo>;

            string body = JsonMapper.ToJson(new_list);
            Debug.Log($"new list after Addstuevent : {body}");
            NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.AddFacInfoEvent);
        });
    }
}