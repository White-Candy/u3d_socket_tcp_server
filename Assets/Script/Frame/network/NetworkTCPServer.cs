using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkTCPServer
{
    public Socket m_Socket;

    public static int ret_length = 1024000;
    public static byte[] results = new byte[ret_length];

    public static Queue<AsyncExpandPkg> MessQueue = new Queue<AsyncExpandPkg>();

    public static List<Socket> cliList = new List<Socket> ();

    public void LauncherServer(int port)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Socket.Bind(endPoint);
        m_Socket.Listen(10000);

        m_Socket.BeginAccept(AcceptAsync, null);
    }

    public void AcceptAsync(IAsyncResult ar)
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
            string mess = Encoding.Default.GetString(results, 0, length);
            Array.Clear(results, 0, results.Length);
            Debug.Log("Recive: ================ " + mess);

            string[] lengthSplit = mess.Split("|");
            string totalLength = lengthSplit[0];
            if (!pkg.messPkg.get_length && !string.IsNullOrEmpty(totalLength))
            {
                // Debug.Log("GET LENGTH: " + totalLength);
                pkg.messPkg.length = int.Parse(totalLength);
                pkg.messPkg.get_length = true;
                pkg.messPkg.ret += lengthSplit[1];
                totalLength = "";

                checkParcent(pkg);
            }
            else
            {
                if (pkg.messPkg.length > pkg.messPkg.ret.Count())
                {
                    pkg.messPkg.ret += mess;
                }
                // Debug.Log("GET MAIN: " + pkg.messPkg.ret);

                checkParcent(pkg);
            }
            cli.BeginReceive(results, 0, ret_length, 0, ReciveMessageAsync, pkg);
        }
        catch
        {

        }
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="cli"></param>
    /// <param name="mess"></param>
    public static async void SendAsync(Socket cli, string mess, EventType event_type, OperateType operateType)
    {
        string front = FrontPackage(cli, mess, event_type, operateType);
        string totalInfoPkg = "|" + front + "#" + mess;
        long totalLength = totalInfoPkg.Count();
        string finalPkg = totalLength.ToString() + totalInfoPkg;

        Debug.Log($"============={finalPkg}");
        SendPkg sp = new SendPkg() { socket = cli, content = finalPkg };
        var outputBuffer = Encoding.Default.GetBytes(sp.content);
        sp.socket.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, SendPkgAsyncCbk, sp);

        await UniTask.Yield();
    }

    /// <summary>
    /// ǰ�ð�
    /// </summary>
    /// <param name="cli"></param>
    /// <param name="mess"></param>
    public static string FrontPackage(Socket cli, string mess, EventType event_type, OperateType operateType)
    {
        MessPackage data = new MessPackage()
        {
            length = mess.Length,
            event_type = event_type.ToSafeString(),
            operate_type = operateType.ToSafeString(),
        };
        string s_info = JsonMapper.ToJson(data);
        return s_info;
    }

    /// <summary>
    /// �첽���ͻص�����
    /// </summary>
    /// <param name="ar"></param>
    private static void SendPkgAsyncCbk(IAsyncResult ar)
    {
        SendPkg sp = ar.AsyncState as SendPkg;
        // Debug.Log("Send Call back: " + DateTime.Now + " || " + sp.content);
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
    public void Clear()
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

    /// <summary>
    /// ǰ�ð������ݰ�����
    /// </summary>
    /// <param name="pkg"></param>
    public static void ParsingThePackageBody(string package, AsyncExpandPkg pkg)
    {
        string[] Split = package.Split("#");
        string front = Split[0];

        string main = Split[1];

        JsonData data = JsonMapper.ToObject(front);

        // ǰ�ð���ȡ���ݰ����ܳ��Ⱥ��¼�����
        pkg.messPkg.ip = data["ip"].ToString();
        pkg.messPkg.length = int.Parse(data["length"].ToString());
        pkg.messPkg.event_type = data["event_type"].ToString();
        pkg.messPkg.operate_type = data["operate_type"].ToString();
        //Debug.Log($"ParsingThePackageBody: {pkg.messPkg.event_type} || {pkg.messPkg.operate_type} ");
        pkg.messPkg.get_length = true;
        // Debug.Log("####### Main : " + main);

        pkg.messPkg.ret = main;
        MessQueueAdd(pkg);

        pkg.messPkg.Clear();
    }

    /// <summary>
    /// ���ȼ��
    /// </summary>
    /// <param name="pkg"></param>
    public static void checkParcent(AsyncExpandPkg pkg)
    {
        float percent = (float)(pkg.messPkg.ret.Count() + 1.0f)* 1.0f / (float)pkg.messPkg.length * 1.0f * 100.0f;
        Debug.Log("----------" +  pkg.messPkg.ip + " | " + percent + "%");  // Add message package for queue.
        if (percent >= 100.0f)
        {
            // Debug.Log("FINISH: " + pkg.messPkg.ret);
            pkg.messPkg.finish = true;
            ParsingThePackageBody(pkg.messPkg.ret, pkg);
        }
    }
}


/// <summary>
/// ����һ������������Ϣ�� ��Ϣ����
/// </summary>
public class MessPackage
{
    // public Socket socket = default; // ������Ϣ��soket
    public string ip = ""; // ip
    public string ret = ""; // ���͵���Ϣ
    public string operate_type = ""; // ��������
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
        operate_type = pkg.operate_type;
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

/// <summary>
/// ���Ͱ�
/// </summary>
public class SendPkg
{
    public Socket socket;
    public string content;
}