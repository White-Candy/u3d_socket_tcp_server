using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileTool
{
    public static async void Bytes2File(byte[] buffer, string save_path)
    {
        await UniTask.RunOnThreadPool(() => 
        {
            if (File.Exists(save_path))
            {
                File.Delete(save_path);
            }
            Debug.Log("buffer length: " + buffer.Length);
            FileStream fs = new FileStream(save_path, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buffer, 0, buffer.Length);
            bw.Close();
            fs.Close();
        });
    }
}
