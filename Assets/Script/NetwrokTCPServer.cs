using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientPkg
{
    public Socket cli;
    public string ret;

    public ClientPkg()
    {

    }

    public ClientPkg(Socket cli, string ret)
    {
        this.cli = cli;
        this.ret = ret;
    }
}

public static class NetwrokTCPServer
{
    public static Socket m_Socket;

    public static Thread thread;

    public static List<Thread> ctList = new List<Thread>();

    public static bool m_Exit = true;

    public static Queue<ClientPkg> MessageQueue = new Queue<ClientPkg>();

    public static void LauncherServer(int port)
    {
        Debug.Log("-- LauncherServer");
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Socket.Bind(endPoint);
        m_Socket.Listen(2);

        thread = new Thread(ConnectListen);
        thread.Start();
    }

    public static void ConnectListen()
    {
        Debug.Log("-- ConnectListen");
        while (true)
        {
            Socket cli = m_Socket.Accept();
            string mess = "Hi there, I accept you request at " + DateTime.Now.ToString();
            SendToClient(cli, mess);

            try
            {
                Thread clientThread = new Thread(ReciveMessage);
                clientThread.Start(cli);
                ctList.Add(clientThread);
                // Debug.Log(ctList.Count);
            }
            catch
            {

            }
        }
    }

    /// <summary>
    /// ∑¢ÀÕ–≈œ¢
    /// </summary>
    /// <param name="cli"></param>
    /// <param name="mess"></param>
    public static void SendToClient(Socket cli, string mess)
    {
        cli.Send(Encoding.Unicode.GetBytes(mess + "-end"));
    }

    public static void ReciveMessage(object socket)
    {
        try
        {
            Socket cli = socket as Socket;
            ClientPkg pkg = new ClientPkg(cli, "");
            bool getLength = false;
            long datalength = 0;

            while (m_Exit)
            {
                try
                {
                    if (!getLength)
                    {
                        byte[] results = new byte[10240];
                        int length = cli.Receive(results);
                        string mess = Encoding.Unicode.GetString(results, 0, length);
                        Array.Clear(results, 0, results.Length);
                        JsonData data = JsonMapper.ToObject(mess);
                        datalength = int.Parse(data["pkgLength"].ToString());

                        Debug.Log("++++++++++++++++++++++++++++++++++++++++++" + mess);
                        getLength = true;
                    }
                    else
                    {
                        byte[] results = new byte[1024];
                        int length = cli.Receive(results);
                        string mess = Encoding.Unicode.GetString(results, 0, length);
                        Array.Clear(results, 0, results.Length);

                        if (datalength > pkg.ret.Count())
                        {
                            pkg.ret += mess;
                        }
                        else
                        {
                            MessageQueue.Enqueue(pkg);
                            getLength = false;
                            break;
                        }
                        //Debug.Log(0.1f);
                        Debug.Log("----------" + ((float)pkg.ret.Count() * 1.0f / (float)datalength * 1.0f * 100.0f) + "%");
                    }
                }
                catch
                {

                }

                //try
                //{
                //    //string Message = "";
                //    byte[] results = new byte[1024];
                //    int length = cli.Receive(results);
                //    string mess = Encoding.Unicode.GetString(results, 0, length);
                //    Array.Clear(results, 0, results.Length);
                //    //Debug.Log(mess);
                //    // Debug.Log(pkg.ret.Count());
                //    string endfield = mess.Substring(mess.Count() - 4, 4);
                //    if (endfield == "-end")
                //    {
                //        mess = mess.Substring(0, mess.Count() - 4);
                //        //Message += mess;

                //        pkg.ret += mess;
                //        MessageQueue.Enqueue(pkg);
                //    }
                //    else
                //    {
                //        pkg.ret += mess;
                //    }
                //}
                //catch
                //{

                //}
            }
        }
        catch
        {

        }
    }
}
