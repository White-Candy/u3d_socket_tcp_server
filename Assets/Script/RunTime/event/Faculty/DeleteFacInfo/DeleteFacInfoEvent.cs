using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class DeleteFacInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("DeleteFacInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(asynExPkg.messPkg.ret);
        List<FacultyInfo> new_list = StorageHelper.DeleteInfo<FacultyStorageHelper>(info) as List<FacultyInfo>;
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.DeleteFacInfoEvent);

        await UniTask.Yield();
    }
}