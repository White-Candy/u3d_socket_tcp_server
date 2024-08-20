using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class LauncherServer : MonoBehaviour
{
    public bool exit = true;

    public CancellationTokenSource cts = new CancellationTokenSource();

    private EventDispatcher m_dispatcher = new EventDispatcher();

    public void Start()
    {
        NetworkTCPServer.LauncherServer(5800);
    }
    
    void FixedUpdate()
    {
        if (NetworkTCPServer.MessQueue.Count > 0)
        {
            var pkg = NetworkTCPServer.MessQueue.Dequeue();
            // BaseEvent @event = Tools.CreateObject<BaseEvent>(pkg.messPkg.event_type);
            // @event.OnEvent(pkg);

            m_dispatcher.Dispatcher(pkg);
        }
    }

    private async void OnDestroy()
    {
        await StorageHelper.SaveToDisk();
        NetworkTCPServer.Clear();
    }
}