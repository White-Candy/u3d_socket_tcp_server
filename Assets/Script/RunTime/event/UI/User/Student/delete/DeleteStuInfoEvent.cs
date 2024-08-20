using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class DeleteStuInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("DeleteStuInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;
        
        await UniTask.SwitchToMainThread();
        StuInfo userInfo = JsonMapper.ToObject<StuInfo>(asynExPkg.messPkg.ret);
        List<StuInfo> new_list = await StorageHelper.DeleteInfo(StorageHelper.Storage.userInfos, x => x.userName == userInfo.userName);
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.DeleteStuInfoEvent);

        await UniTask.Yield();
    }
}