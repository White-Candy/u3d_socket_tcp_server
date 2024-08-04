using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLoginEvent : IEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

            UserInfo inf = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
            (inf.login, inf.hint) = await StorageExpand.CheckUserLogin(inf.userName, inf.password);

            string s_inf = JsonMapper.ToJson(inf);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_inf, EventType.UserLoginEvent);
        });
    }
}

/// <summary>
/// 用户信息包
/// </summary>
[Serializable]
public class UserInfo
{
    public string userName;
    public string password;
    public bool login;
    public string hint;
}