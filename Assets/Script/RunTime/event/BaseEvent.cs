using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperateType
{
    NONE = 0, GET, ADD, REVISE, DELETE,
}

public enum EventType
{
    None = 0,
    UploadEvent,
    DownLoadEvent,
    CheckEvent,
    UserLoginEvent,
    RegisterEvent,
    UserEvent,
    MajorEvent,
    FacultyEvent,
    GetEvent,
}

public abstract class BaseEvent
{
    public virtual async void OnEvent(AsyncExpandPkg pkg) { await UniTask.Yield(); }
    public virtual async void GetInfoEvent(AsyncExpandPkg pkg) { await UniTask.Yield(); }
    public virtual async void AddEvent(AsyncExpandPkg pkg) { await UniTask.Yield(); }
    public virtual async void ReviseInfoEvent(AsyncExpandPkg pkg) { await UniTask.Yield(); }
    public virtual async void DeleteInfoEvent(AsyncExpandPkg pkg){ await UniTask.Yield(); }
}