using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class DeleteFacInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("DeleteFacInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        FacultyInfo info = JsonMapper.ToObject<FacultyInfo>(asynExPkg.messPkg.ret);
        await UniTask.SwitchToMainThread();
        List<FacultyInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.Storage.faculiesInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.DeleteFacInfoEvent);

        await UniTask.Yield();
    }
}