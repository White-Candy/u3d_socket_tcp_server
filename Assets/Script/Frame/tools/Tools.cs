using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Tools
{
    /// <summary>
    /// 把字节数组变成文件流
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="save_path"></param>
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

    /// <summary>
    /// 动态创建类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T CreateObject<T>(string name) where T : class
    {
        object obj = CreateObject(name);
        return obj == null ? null : obj as T;
    }

    /// <summary>
    /// 动态创建类
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static object CreateObject(string name)
    {
        object obj = null;
        try
        {
            Type type = Type.GetType(name, true);
            obj = Activator.CreateInstance(type);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        return obj;
    }
}
