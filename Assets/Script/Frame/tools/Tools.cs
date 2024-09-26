using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tools
{
    public static bool CheckMessageSuccess(int code)
    {
        return code == GlobalData.SuccessCode;
    }
    
    /// <summary>
    /// 把字节数组 => 文件流
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
    /// 把文件流 => 字节数组
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async UniTask<byte[]> File2Bytes(string path)
    {
        byte[] data = new byte[0];
        if (!File.Exists(path)) return data;

        await UniTask.Yield();

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

        return data;
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

    /// <summary>
    /// 等待器
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static async UniTask OnAwait(float sec, Action callback)
    {
        int duration = (int)(sec * 1000);
        await UniTask.Delay(duration);
        callback();
    }
    
    /// <summary>
    /// 为新【添加/更新】的文件更新新的版本号
    /// </summary>
    /// <returns></returns>
    public static string SpawnRandomCode()
    {
        return $"{Random.Range(1000, 9999)}-{Random.Range(1000, 9999)}-{Random.Range(1000, 9999)}-{Random.Range(1000, 9999)}";
    }

    /// <summary>
    /// String To Unicode
    /// </summary>
    /// <param name="inputText"></param>
    /// <returns></returns>
    public static string StringToUnicode(string inputText)
    {
        string newStr = "";
        for(int i = 0; i < inputText.Count(); ++i)
        {
            if (inputText[i] == '\\') newStr += '\\';
            newStr += inputText[i];
        }
        char[] charBuffer = newStr.ToCharArray ();
        byte[] buffer;
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < charBuffer.Length; i++)
        {
            if ((int)charBuffer[i] > 127)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes (charBuffer [i].ToString ());
                stringBuilder.Append (string.Format ("\\u{0:X2}{1:X2}", buffer [1], buffer [0]));
            }
            else 
            {
                stringBuilder.Append(charBuffer[i].ToString());
            }
        }
        return stringBuilder.ToString ();
    }    
}
