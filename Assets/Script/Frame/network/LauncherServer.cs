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

            //BodyPkg data = JsonMapper.ToObject<BodyPkg>(ret);
            //string savepath = Application.streamingAssetsPath + "/" + data.fileName;
            //Debug.Log(savepath);
            //Debug.Log(data.fileData.Length);
            //Tools.Bytes2File(data.fileData, savepath);
        }
    }

    private void OnDestroy()
    {
        NetworkTCPServer.Clear();
    }
}
