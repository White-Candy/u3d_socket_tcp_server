

using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class FileHelper
{
    public static async UniTask ReadFileContent(string path, Action<string> action = default)
    {
        UnityWebRequest req = UnityWebRequest.Get(path);
        await req.SendWebRequest();

        string content = req.downloadHandler.text;

        if (action != null)
        {
            action(content);
        }
    }
}