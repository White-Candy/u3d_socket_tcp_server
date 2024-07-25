using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Tools
{
    /// <summary>
    /// ���ֽ����� => �ļ���
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="save_path"></param>
    public static void Bytes2File(byte[] buffer, string save_path)
    {
        if (File.Exists(save_path))
        {
            File.Delete(save_path);
        }
        else
        {
            string dir = save_path;
            int idx = dir.LastIndexOf('\\');
            dir = dir.Substring(0, idx);
            Directory.CreateDirectory(dir);
        }

        FileStream fs = new FileStream(save_path, FileMode.CreateNew);
        lock (fs)
        {
            BinaryWriter bw = new BinaryWriter(fs);
            lock (bw)
            {
                bw.Write(buffer, 0, buffer.Length);
                bw.Close();
            }
            fs.Close();
        }
    }

    /// <summary>
    /// ���ļ��� => �ֽ�����
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async UniTask<byte[]> File2Bytes(string path)
    {
        byte[] data = new byte[0];
        if (!File.Exists(path)) return data;

        await UniTask.RunOnThreadPool(() =>
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            lock (fs)
            {
                try
                {
                    data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                }
                catch { }
                finally { fs.Close(); }
            }

        });
        return data;
    }

    /// <summary>
    /// ��̬������
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
    /// ��̬������
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

    // �ȴ���
    public static async UniTask OnAwait(float sec, Action callback)
    {
        int duration = (int)(sec * 1000);
        await UniTask.Delay(duration);
        callback();
    }
}
