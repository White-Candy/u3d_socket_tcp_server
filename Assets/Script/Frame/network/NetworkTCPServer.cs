using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        m_Socket.Listen(0); // ���ޣ�

        m_Socket.BeginAccept(AcceptAsync, null);
    }

    public static void AcceptAsync(IAsyncResult ar)
    {
        // Socket listenfd = (Socket)ar.AsyncState;
        Socket cli = m_Socket.EndAccept(ar);

        // string mess = "Hi there, I accept you request at " + DateTime.Now.ToString();
        // var outputBuffer = Encoding.Unicode.GetBytes(mess);
        // cli.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, SendAsyncCallback, cli);

        try
        {
            Array.Clear(results, 0, results.Length);
            MessPackage client_pkg = new MessPackage();
            AsyncExpandPkg exp_pkg = new AsyncExpandPkg();

            cliList.Add(cli);

            exp_pkg.socket = cli;
            exp_pkg.messPkg = client_pkg;

            cli.BeginReceive(results, 0, ret_length, 0, ReciveMessageAsync, exp_pkg);

            // �ݹ�
            m_Socket.BeginAccept(AcceptAsync, null);
        }
        catch
        {

        }
    }

    /// <summary>
    /// �ͻ���������Ϣ����
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

                // ǰ�ð���ȡ���ݰ����ܳ��Ⱥ��¼�����
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
    /// ������Ϣcallback
    /// </summary>
    /// <param name="cli"></param>
    /// <param name="mess"></param>
    public static void SendAsyncCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            if (socket != null)
            {
                m_Socket = socket;
                int count = socket.EndSend(ar);
                Debug.Log("Send Success!: " + count);
            }
            else
            {
                Debug.Log("Send is NULL");
            }
        }
        catch (Exception)
        {

            throw;
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
    /// Ϊ��Ϣ���� Clone pkg ���Ҵ��
    /// </summary>
    /// <param name="pkg"></param>
    public static void MessQueueAdd(AsyncExpandPkg pkg)
    {
        AsyncExpandPkg exp_pkg = new AsyncExpandPkg(pkg);
        MessQueue.Enqueue(exp_pkg);
    }
}


/// <summary>
/// ����һ������������Ϣ�� ��Ϣ����
/// </summary>
public class MessPackage
{
    // public Socket socket = default; // ������Ϣ��soket
    public string ip = ""; // ����ip
    public string ret = ""; // �����͵���Ϣ
    public string event_type = ""; // �����Ϣ����ʲô����
    public int length = 0; // ��������ܳ���
    public bool finish = false; // �Ƿ���ȫ�հ�
    public bool get_length = false; // �Ƿ��Ѿ�ͨ��ǰ�ð���ȡ�������ݰ����ܳ���

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
/// �첽�ص���չ��
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