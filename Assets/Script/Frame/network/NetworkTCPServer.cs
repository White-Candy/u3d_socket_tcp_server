using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public static class NetworkTCPServer
{
    public static Socket m_Socket;

    public static int ret_length = 1024000;
    public static byte[] results = new byte[ret_length];

    public static Queue<AsyncExpandPkg> MessQueue = new Queue<AsyncExpandPkg>();

    public static List<Socket> cliList = new List<Socket> ();

    public static void LauncherServer(int port)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Socket.Bind(endPoint);
        m_Socket.Listen(0); // 无限！

        m_Socket.BeginAccept(AcceptAsync, null);
    }

    public static void AcceptAsync(IAsyncResult ar)
    {
        // Socket listenfd = (Socket)ar.AsyncState;
        Socket cli = m_Socket.EndAccept(ar);
        try
        {
            Array.Clear(results, 0, results.Length);
            MessPackage client_pkg = new MessPackage();
            AsyncExpandPkg exp_pkg = new AsyncExpandPkg();

            cliList.Add(cli);

            exp_pkg.socket = cli;
            exp_pkg.messPkg = client_pkg;

            cli.BeginReceive(results, 0, ret_length, 0, ReciveMessageAsync, exp_pkg);

            // 递归
            m_Socket.BeginAccept(AcceptAsync, null);
        }
        catch
        {

        }
    }

    /// <summary>
    /// 客户端请求信息接收
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="task"></param>
    /// <returns></returns>
    public static void ReciveMessageAsync(IAsyncResult ar)
    {
        AsyncExpandPkg pkg = (AsyncExpandPkg)ar.AsyncState;
        Socket cli = pkg.socket;
        int length = cli.EndReceive(ar);

        try
        {
            string mess = Encoding.Unicode.GetString(results, 0, length);
            Array.Clear(results, 0, results.Length);

            Debug.Log("+++++" + mess); // log message of front package
            if (!pkg.messPkg.get_length)
            {
                JsonData data = JsonMapper.ToObject(mess);

                // 前置包获取内容包的总长度和事件类型
                pkg.messPkg.ip = data["ip"].ToString();
                pkg.messPkg.length = int.Parse(data["length"].ToString());
                pkg.messPkg.event_type = data["type"].ToString();
                pkg.messPkg.get_length = true;
            }
            else
            {
                if (pkg.messPkg.length > pkg.messPkg.ret.Count())
                {
                    pkg.messPkg.ret += mess;
                }

                float percent = (float)pkg.messPkg.ret.Count() * 1.0f / (float)pkg.messPkg.length * 1.0f * 100.0f;
                Debug.Log("----------" +  pkg.messPkg.ip + " | " + percent + "%");  // Add message package for queue.

                if (percent >= 100.0f)
                {
                    pkg.messPkg.finish = true;
                    MessQueueAdd(pkg);
                    pkg.messPkg.Clear();
                }
            }
            cli.BeginReceive(results, 0, ret_length, 0, ReciveMessageAsync, pkg);
        }
        catch
        {

        }
    }

    /// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="cli"></param>
    /// <param name="mess"></param>
    public static async void SendAsync(Socket cli, string mess, EventType event_type)
    {
        SendFrontPackage(cli, mess, event_type);

        await Tools.OnAwait(0.1f, () =>
        {
            SendPkg sp = new SendPkg() { socket = cli, content = mess };
            var outputBuffer = Encoding.Unicode.GetBytes(sp.content);
            sp.socket.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, SendPkgAsyncCbk, sp);
        });
    }

    /// <summary>
    /// 发送前置包
    /// </summary>
    /// <param name="cli"></param>
    /// <param name="mess"></param>
    public static void SendFrontPackage(Socket cli, string mess, EventType event_type)
    {
        MessPackage data = new MessPackage()
        {
            length = mess.Length,
            event_type = event_type.ToSafeString()
        };
        string s_info = JsonMapper.ToJson(data);
        var outputBuffer = Encoding.Unicode.GetBytes(s_info);

        SendPkg sp = new SendPkg() { socket = cli, content = s_info };
        // Debug.Log("Send Now DateTime: " + DateTime.Now);
        cli.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, SendPkgAsyncCbk, sp);
    }

    /// <summary>
    /// 等到前置包发送结束后才会发送主体包
    /// </summary>
    /// <param name="ar"></param>
    private static void SendPkgAsyncCbk(IAsyncResult ar)
    {
        SendPkg sp = ar.AsyncState as SendPkg;
        Debug.Log("Send Call back: " + DateTime.Now + " || " + sp.content);
        try
        {
            if (sp.socket != null)
            {
                sp.socket.EndSend(ar);
                // Debug.Log("sp.mess: " + sp.content);
                
            }
        }
        catch (SocketException e)
        {
            Debug.Log("socket send fail" + e.ToString());
        }
    }

    /// <summary>
    /// Destroy Clear
    /// </summary>
    public static void Clear()
    {
        Debug.Log(cliList.Count);
        foreach (var cli in cliList)
        {
            cli.Close();
        }
        cliList.Clear();

        m_Socket.Close();
    }

    /// <summary>
    /// 为消息队列 Clone pkg 并且存放
    /// </summary>
    /// <param name="pkg"></param>
    public static void MessQueueAdd(AsyncExpandPkg pkg)
    {
        AsyncExpandPkg exp_pkg = new AsyncExpandPkg(pkg);
        MessQueue.Enqueue(exp_pkg);
    }
}


/// <summary>
/// 这是一个接受完整信息的 信息包类
/// </summary>
public class MessPackage
{
    // public Socket socket = default; // 发送信息的soket
    public string ip = ""; // 他的ip
    public string ret = ""; // 他发送的信息
    public string event_type = ""; // 这个信息属于什么类型
    public int length = 0; // 这个包的总长度
    public bool finish = false; // 是否完全收包
    public bool get_length = false; // 是否已经通过前置包获取到了内容包的总长度

    public void Clear()
    {
        // socket = default;
        ip = "";
        ret = "";
        event_type = "";
        length = 0;
        finish = false;
        get_length = false;
    }

    public MessPackage() { }

    public MessPackage(MessPackage pkg)
    {
        // socket = pkg.socket;
        ip = pkg.ip;
        ret = pkg.ret;
        event_type = pkg.event_type;
        length = pkg.length;
        finish = pkg.finish;
        get_length = pkg.get_length;
    }
}


/// <summary>
/// 异步回调扩展包
/// </summary>
public class AsyncExpandPkg
{
    public Socket socket;
    public MessPackage messPkg;

    public AsyncExpandPkg() { }

    public AsyncExpandPkg(AsyncExpandPkg pkg)
    {
        socket = pkg.socket;
        messPkg = new MessPackage(pkg.messPkg);
    }
}

/// <summary>
/// 发送包
/// </summary>
public class SendPkg
{
    public Socket socket;
    public string content;
}