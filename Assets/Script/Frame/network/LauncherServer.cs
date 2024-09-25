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

    private NetworkTCPServer m_server = new NetworkTCPServer();

    public void Start()
    {
        m_server.LauncherServer("192.168.1.252", "5800"); 
    }
    
    void Update()
    {
        if (NetworkTCPServer.MessQueue.Count > 0)
        {
            var pkg = NetworkTCPServer.MessQueue.Dequeue();
            m_dispatcher.Dispatcher(pkg);
        }
    }

    private async void OnDestroy()
    {
        await StorageHelper.SaveToDisk();
        m_server.Clear();
    }
}