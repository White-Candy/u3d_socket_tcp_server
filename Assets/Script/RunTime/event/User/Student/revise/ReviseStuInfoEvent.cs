using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class ReviseStuInfoEvent : IEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

            // Debug.Log("ReviseStuInfoEvent");
            UserInfo inf = JsonMapper.ToObject<UserInfo>(asynExPkg.messPkg.ret);
            List<UserInfo> ls_inf = await StorageHelper.ReviseInfo<StudentStorageHelper>(inf) as List<UserInfo>;

            string s_inf = JsonMapper.ToJson(ls_inf);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_inf, EventType.ReviseStuInfoEvent);
        });
    }
}