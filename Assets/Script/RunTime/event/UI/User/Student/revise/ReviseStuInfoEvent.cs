using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class ReviseStuInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool(async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

            await UniTask.SwitchToMainThread();
            StuInfo inf = JsonMapper.ToObject<StuInfo>(asynExPkg.messPkg.ret);
            List<StuInfo> ls_inf = await StorageHelper.ReviseInfo(inf, StorageHelper.Storage.userInfos, x => x.userName == inf.userName);

            string s_inf = JsonMapper.ToJson(ls_inf);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_inf, EventType.ReviseStuInfoEvent);
        });
    }
}