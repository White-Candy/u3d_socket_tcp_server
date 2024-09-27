using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        await UniTask.Yield();
    }
}