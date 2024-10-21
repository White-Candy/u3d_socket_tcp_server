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

    // private NetworkTCPServer m_server = new NetworkTCPServer();

    private HttpServer m_HttpServer = new HttpServer();

    public void Awake()
    {

    }

    public async void Start()
    {
        await FileHelper.ReadFileContent(Application.streamingAssetsPath + "\\Net\\IP.txt", (ip) => 
        {
            string[] ipSplit = ip.Split(":");
            string url = ipSplit[0], port = ipSplit[1];
            m_HttpServer.LauncherServer(url, port);
        });
    }

    public void Update()
    {
        if (HttpServer.MessQueue.Count > 0)
        {
            var pkg = HttpServer.MessQueue.Dequeue();
            m_dispatcher.Dispatcher(pkg);
        }
    }

    private async void OnDestroy()
    {
        await StorageHelper.SaveToDisk();
        m_HttpServer.Clear();
        //m_server.Clear();
    }
}