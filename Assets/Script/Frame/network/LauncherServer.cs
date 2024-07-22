using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BodyPkg
{
    public string fileName;
    public byte[] fileData;
}

public class LauncherServer : MonoBehaviour
{
    public bool exit = true;

    public CancellationTokenSource cts = new CancellationTokenSource();

    public void Start()
    {
        NetworkTCPServer.LauncherServer(6100);
    }
    
    void Update()
    {
        //if (NetworkTCPServer.MessageQueue.Count > 0)
        //{
        //    var pkg = NetworkTCPServer.MessageQueue.Dequeue();
        //    string ret = pkg.ret;

        //    BodyPkg data = JsonMapper.ToObject<BodyPkg>(ret);
        //    string savepath = Application.streamingAssetsPath + "/" + data.fileName;
        //    Debug.Log(savepath);
        //    Debug.Log(data.fileData.Length);
        //    Tools.Bytes2File(data.fileData, savepath);
        //}
    }

    private void OnDestroy()
    {
        // cts.Cancel();
        // cts.Dispose();
        // exit = false;
        NetworkTCPServer.Clear();
        GlobalData.ServerIsListen = false;
    }
}
