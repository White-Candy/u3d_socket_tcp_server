using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLoginEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            UserInfo inf = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
            inf = await StorageHelper.CheckUserLogin(inf);

            string s_inf = JsonMapper.ToJson(inf);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_inf, EventType.UserLoginEvent, OperateType.NONE);
        });
    }
}