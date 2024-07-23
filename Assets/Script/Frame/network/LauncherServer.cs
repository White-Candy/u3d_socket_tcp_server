using Cysharp.Threading.Tasks;
using LitJson;
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

    public void Start()
    {
        NetworkTCPServer.LauncherServer(5800);
    }
    
    void Update()
    {
        if (NetworkTCPServer.MessQueue.Count > 0)
        {
            var pkg = NetworkTCPServer.MessQueue.Dequeue();
            string ret = pkg.ret;
            Debug.Log(pkg.ip + " || " + pkg.length);

            IEvent @event = Tools.CreateObject<IEvent>(pkg.event_type);
            @event.OnEvent(pkg.ret);
        }
    }

    private void OnDestroy()
    {
        NetworkTCPServer.Clear();
    }
}
