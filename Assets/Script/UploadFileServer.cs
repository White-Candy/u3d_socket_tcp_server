using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

public class BodyPkg
{
    public string fileName;
    public byte[] fileData;
}

public class UploadFileServer : MonoBehaviour
{
    void Start()
    {
        NetwrokTCPServer.LauncherServer(6100);
    }

    void Update()
    {
        if (NetwrokTCPServer.MessageQueue.Count > 0)
        {
            var pkg = NetwrokTCPServer.MessageQueue.Dequeue();
            string ret = pkg.ret;

            BodyPkg data = JsonMapper.ToObject<BodyPkg>(ret);
            string savepath = Application.streamingAssetsPath + "/" + data.fileName;
            Debug.Log(savepath);
            Debug.Log(data.fileData.Length);
            FileTool.Bytes2File(data.fileData, savepath);
        }
    }

    private void OnDestroy()
    {
        NetwrokTCPServer.m_Exit = false;
    }
}
