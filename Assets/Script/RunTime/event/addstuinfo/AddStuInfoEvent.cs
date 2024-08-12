using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class AddStuInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)    
    {
        Debug.Log("AddStuInfoEvent..");
        AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

        List<UserInfo> usersList = JsonMapper.ToObject<List<UserInfo>>(asynExPkg.messPkg.ret);
        List<UserInfo> new_list = await StorageExpand.AddStusInfo(usersList);

        string body = JsonMapper.ToJson(new_list);
        Debug.Log($"new list after Addstuevent : {body}");
        NetworkTCPServer.SendAsync(asynExPkg.socket, body, EventType.AddStuInfoEvent);
    }
}