using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class AddStuInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("AddStuInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        await UniTask.SwitchToMainThread();
        List<StuInfo> usersList = JsonMapper.ToObject<List<StuInfo>>(asynExPkg.messPkg.ret);
        List<StuInfo> new_list = await StorageHelper.AddInfo(usersList, StorageHelper.Storage.userInfos);

        string body = JsonMapper.ToJson(new_list);
        Debug.Log($"new list after Addstuevent : {body}");
        NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.AddStuInfoEvent);
    }
}