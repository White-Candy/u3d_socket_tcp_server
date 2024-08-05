using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterEvent : IEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

            UserInfo inf = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
            inf = await StorageExpand.Register(inf);
            
            string s_inf = JsonMapper.ToJson(inf);
            Debug.Log($"RegisterEvent: {s_inf}");
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_inf, EventType.RegisterEvent);
        });
    }
}
