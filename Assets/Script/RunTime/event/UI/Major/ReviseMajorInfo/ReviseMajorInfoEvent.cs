using Cysharp.Threading.Tasks;
using LitJson;
using OpenCover.Framework.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ReviseMajorInfoEvent : BaseEvent
{
    public override async void OnEvent(params object[] objs)
    {
        await UniTask.RunOnThreadPool (async () =>
        {
            AsyncExpandPkg asynExPkg = objs[0] as AsyncExpandPkg;

            await UniTask.SwitchToMainThread();
            MajorInfo info = JsonMapper.ToObject<MajorInfo>(asynExPkg.messPkg.ret);
            List<MajorInfo> infs = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.majorInfo, x => x.id == info.id);
            
            string s_infs = JsonMapper.ToJson(infs);
            NetworkTCPServer.SendAsync(asynExPkg.socket, s_infs, EventType.GetMajorInfoEvent);
        });
    }
}