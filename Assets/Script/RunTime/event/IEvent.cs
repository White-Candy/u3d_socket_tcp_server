using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None = 0,
    UploadEvent,
    DownLoadEvent,
    CheckEvent
}

public abstract class IEvent
{
    public virtual async void OnEvent(params object[] objs) { await UniTask.Yield(); }
}
