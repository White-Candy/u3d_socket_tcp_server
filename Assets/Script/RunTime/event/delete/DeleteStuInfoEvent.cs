using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class DeleteStuInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("DeleteStuInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        UserInfo userInfo = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
        List<UserInfo> new_list = StorageHelper.DeleteInfo(userInfo);
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.DeleteStuInfoEvent);
    }
}