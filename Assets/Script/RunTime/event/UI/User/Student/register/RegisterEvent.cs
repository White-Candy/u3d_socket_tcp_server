using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

            StuInfo inf = JsonMapper.ToObject<StuInfo>(asynExPkg.messPkg.ret);
            inf = await StorageHelper.Register(inf);
            
            string s_inf = JsonMapper.ToJson(inf);
            Debug.Log($"RegisterEvent: {s_inf}");
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_inf, EventType.RegisterEvent);
        });
    }
}
