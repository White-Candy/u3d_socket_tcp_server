using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CliBody
{
    public Socket socket;
    public string ip;
    public string ret;
    public string event_type;
    public int length;
    public bool finish;
    public bool get_length = false;
}

public static class NetworkTCPServer
{
    public static Socket m_Socket;

    private static Thread Listen_thread;

    public static List<Thread> ctList = new List<Thread>();

    public static int ret_length = 1024000;
    public static byte[] results = new byte[1024000];

    public static void LauncherServer(int port)
    {
        Debug.Log("-- LauncherServer");
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Socket.Bind(endPoint);
        m_Socket.Listen(500);

        Listen_thread = new Thread(() => { m_Socket.BeginAccept(ConnectListen, m_Socket); });
        Listen_thread.Start();

        //thread = new Thread(ConnectListen);
        //thread.Start();
    }

    public static void ConnectListen(IAsyncResult ar)
    {
        Debug.Log("-- ConnectListen");

        Socket listenfd = (Socket)ar.AsyncState;
        Socket cli = listenfd.EndAccept(ar);

        string mess = "Hi there, I accept you request at " + DateTime.Now.ToString();
        SendToClient(cli, mess);

        try
        {
            Array.Clear(results, 0, results.Length);
            Thread cli_td = new Thread(() =>
            {
                CliBody cliBody = new CliBody();
                cliBody.socket = cli;
                cli.BeginReceive(results, 0, ret_length, 0, ReciveMessage, cliBody);
            });
            cli_td.Start();
            ctList.Add(cli_td);

            listenfd.BeginAccept(ConnectListen, listenfd);        
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
    public static void ReciveMessage(IAsyncResult ar)
    {
        Debug.Log("ReciveMessage");

        CliBody body = (CliBody)ar.AsyncState;
        Socket cli = body.socket;
        int length = cli.EndReceive(ar);
        Debug.Log("body.get_length: " + body.get_length + " | " + body.length + " | " + length);
        try
        {
            //Debug.Log("Mess length: " + length);
            string mess = Encoding.Unicode.GetString(results, 0, length);
            Array.Clear(results, 0, results.Length);

            if (!body.get_length)
            {
                JsonData data = JsonMapper.ToObject(mess);

                body.length = int.Parse(data["length"].ToString());
                body.event_type = data["type"].ToString();

                Debug.Log("++++++++++++++++++++++++++++++++++++++++++" + mess );
                body.get_length = true;
            }
            else
            {
                Debug.Log("body.get_length is Ture: " + mess);
                Debug.Log("mess count: " + mess.Count());
                if (body.length > body.ret.Count())
                {
                    body.ret += mess;
                }
                else
                {
                    Debug.Log("event_type: " + body.event_type);
                    body.finish = true;
                }
                //Debug.Log(0.1f);
                Debug.Log("----------" + ((float)body.ret.Count() * 1.0f / (float)body.length * 1.0f * 100.0f) + "%");
            }

            cli.BeginReceive(results, 0, ret_length, 0, ReciveMessage, body);
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
    public static void SendToClient(Socket cli, string mess)
    {
        Debug.Log(mess);
        cli.Send(Encoding.Unicode.GetBytes(mess + "-end"));
    }

    public static void Clear()
    {
        Debug.Log("NetworkTCPServer.ctList: " + ctList.Count);

        m_Socket.Close();
        Listen_thread.Abort();
        Listen_thread = null;

        for (int i = 0; i < ctList.Count; i++)
        {
            ctList[i].Abort();
            ctList[i] = null;
        }
        ctList.Clear();
    }
}
