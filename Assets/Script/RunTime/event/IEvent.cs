using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None = 0,
    UploadEvent,
    DownLoadEvent,
    CheckEvent,
    UserLoginEvent,
    RegisterEvent,
    GetStuInfoEvent,
    AddStuInfoEvent,
    ReviseStuInfoEvent,
    DeleteStuInfoEvent,
    GetFacInfoEvent,
    AddFacInfoEvent,
    ReviseFacInfoEvent,
    DeleteFacInfoEvent,
    GetMajorInfoEvent,
    AddMajorInfoEvent,
    ReviseMajorInfoEvent,
    DeleteMajorInfoEvent
}

public abstract class BaseEvent
{
    public virtual async void OnEvent(params object[] objs) { await UniTask.Yield(); }

    public virtual async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }

    public virtual async void AddEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();        
    }


    public virtual async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }

    public virtual async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        await UniTask.Yield();
    }
}
