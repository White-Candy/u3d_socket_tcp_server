using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEvent
{
    public virtual async void OnEvent(params object[] objs) { await UniTask.Yield(); }
}
