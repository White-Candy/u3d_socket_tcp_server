using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class LauncherServer : MonoBehaviour
{
    public bool exit = true;

    public CancellationTokenSource cts = new CancellationTokenSource();

    private EventDispatcher m_dispatcher = new EventDispatcher();

    private SocketServer m_server = new SocketServer();

    // private HttpServer m_HttpServer = new HttpServer();

    public async void Awake()
    {
        await FileHelper.ReadFileContent(Application.streamingAssetsPath + "\\Net\\IP.txt", (ip) => 
        {
            string[] ipSplit = ip.Split(":");
            string url = ipSplit[0], port = ipSplit[1];

            m_server.LauncherServer(int.Parse(port)); 
        });
    }

    public async void Start()
    {
        await UniTask.Yield();
    }
    
    public void Update()
    {
        if (SocketServer.MessQueue.Count > 0)
        {
            var pkg = SocketServer.MessQueue.Dequeue();
            m_dispatcher.Dispatcher(pkg);
        }
    }

    private async void OnDestroy()
    {
        await StorageHelper.SaveToDisk();
        //m_HttpServer.Clear();
        m_server.Clear();
    }
}