
// using Cysharp.Threading.Tasks;
// using LitJson;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using Unity.VisualScripting;
// using UnityEngine;

// public class NetworkTCPServer
// {
//     public Socket m_Socket;

//     public static int ret_length = 1024000;
//     public static byte[] results = new byte[ret_length];
//     public static Queue<AsyncExpandPkg> MessQueue = new Queue<AsyncExpandPkg>();

//     public static List<Socket> cliList = new List<Socket> ();

//     public void LauncherServer(int port)
//     {
//         IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

//         m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//         m_Socket.Bind(endPoint);
//         m_Socket.Listen(10000);

//         m_Socket.BeginAccept(AcceptAsync, null);
//     }
    
//     public void AcceptAsync(IAsyncResult ar)
//     {
//         Socket listenfd = (Socket)ar.AsyncState;
//         Socket cli = m_Socket.EndAccept(ar);
//         try
//         {
//             Array.Clear(results, 0, results.Length);
//             MessPackage client_pkg = new MessPackage();
//             AsyncExpandPkg exp_pkg = new AsyncExpandPkg();

//             // cliList.Add(cli);

//             exp_pkg.socket = cli;
//             exp_pkg.messPkg = client_pkg;

//             // Debug.Log($"新的连接~ 当前连接数: {cliList.Count}");
//             cli.BeginReceive(results, 0, ret_length, 0, ReciveMessageAsync, exp_pkg);
//             // 递归
//             m_Socket.BeginAccept(AcceptAsync, null);
//         }
//         catch { }
//     }

//     /// <summary>
//     /// 客户端请求信息接收
//     /// </summary>
//     /// <param name="socket"></param>
//     /// <param name="task"></param>
//     /// <returns></returns>
//     public void ReciveMessageAsync(IAsyncResult ar)
//     {
//         AsyncExpandPkg pkg = (AsyncExpandPkg)ar.AsyncState;
//         Socket cli = pkg.socket;
//         int length = cli.EndReceive(ar);

//         try
//         {
//             string mess = Encoding.Default.GetString(results, 0, length);
//             Array.Clear(results, 0, results.Length);
//             Debug.Log($"======================= mess: {mess} | {mess.Count()}");

//             //关闭这个客户端连接
//             if (mess == "Close" || mess.Count() == 0)
//             {
//                 cli.Close();
//                 int removeIdx = cliList.FindIndex((x) => { return x == cli; });
//                 cliList.RemoveAt(removeIdx);
//                 return;
//             }

//             string[] messages = mess.Split("@");
//             foreach (var message in messages)
//             {
//                 InforProcessing(message, pkg);
//             }
            
//             cli.BeginReceive(results, 0, ret_length, 0, ReciveMessageAsync, pkg);
//         }
//         catch { }
//     }

//     public static void Send(HttpListenerContext context, string mess)
//     {
//         Debug.Log("Send NULL");
//         HttpListenerResponse response = context.Response;
//         response.ContentLength64 = Encoding.UTF8.GetByteCount(mess);
//         response.ContentType = "text/html; charset=UTF-8";
//         Stream output = response.OutputStream;
//         StreamWriter writer = new StreamWriter(output);
//         writer.Write(mess);
//         writer.Close();
//     }

//     /// <summary>
//     /// 发送信息
//     /// </summary>
//     /// <param name="cli"></param>
//     // /// <param name="mess"></param>
//     public static async void SendAsync(Socket cli, string mess, EventType event_type, OperateType operateType)
//     {
//         await UniTask.Yield();
//         string front = FrontPackage(cli, mess, event_type, operateType);
//         string totalInfoPkg = "|" + front + "#" + mess + "@";
//         long totalLength = totalInfoPkg.Count();
//         string finalPkg = totalLength.ToString() + totalInfoPkg;

//         // Debug.Log($"============={totalLength} | {front}");
//         SendPkg sp = new SendPkg() { socket = cli, content = finalPkg };
//         var outputBuffer = Encoding.Default.GetBytes(sp.content);
//         sp.socket.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, SendPkgAsyncCbk, sp);
//     }

//     /// <summary>
//     /// 异步发送回调函数
//     /// </summary>
//     /// <param name="ar"></param>
//     private static void SendPkgAsyncCbk(IAsyncResult ar)
//     {
//         SendPkg sp = ar.AsyncState as SendPkg;
//         // Debug.Log("Send Call back: " + DateTime.Now + " || " + sp.content);
//         try
//         {
//             if (sp.socket != null)
//             {
//                 sp.socket.EndSend(ar);
//             }
//         }
//         catch (SocketException e)
//         {
//             Debug.Log("socket send fail" + e.ToString());
//         }
//     }

//     /// <summary>
//     /// 前置包
//     /// </summary>
//     /// <param name="cli"></param>
//     /// <param name="mess"></param>
//     public static string FrontPackage(Socket cli, string mess, EventType event_type, OperateType operateType)
//     {
//         MessPackage data = new MessPackage()
//         {
//             socket = cli,
//             length = mess.Length,
//             event_type = event_type.ToSafeString(),
//             operate_type = operateType.ToSafeString(),
//         };
//         string s_info = JsonMapper.ToJson(data);
//         return s_info;
//     }

//     /// <summary>
//     /// 为消息队列 Clone pkg 并且存放
//     /// </summary>
//     /// <param name="pkg"></param>
//     public static void MessQueueAdd(AsyncExpandPkg pkg)
//     {
//         AsyncExpandPkg exp_pkg = new AsyncExpandPkg(pkg);
//         MessQueue.Enqueue(exp_pkg);
//     }

//     /// <summary>
//     /// 前置包和内容包解析
//     /// </summary>
//     /// <param name="pkg"></param>
//     public static void ParsingThePackageBody(string package, AsyncExpandPkg pkg)
//     {
//         string[] Split = package.Split("#");
//         string front = Split[0];
//         string main = Split[1];

//         JsonData data = JsonMapper.ToObject(front);
//         pkg.messPkg.ip = data["ip"].ToString();
//         pkg.messPkg.length = int.Parse(data["length"].ToString());
//         pkg.messPkg.event_type = data["event_type"].ToString();
//         pkg.messPkg.operate_type = data["operate_type"].ToString();
//         pkg.messPkg.get_length = true;
//         pkg.messPkg.ret = main;
//         MessQueueAdd(pkg);
//         pkg.messPkg.Clear();
//     }

//     /// <summary>
//     /// 进度检查
//     /// </summary>
//     /// <param name="pkg"></param>
//     public static void check(AsyncExpandPkg pkg)
//     {
//         int messLength = pkg.messPkg.ret.Count() + 2;
//         float percent = messLength * 1.0f / pkg.messPkg.length * 1.0f * 100.0f;
//         // Debug.Log($"messLength: {messLength}, messLength: {pkg.messPkg.ret}");
//         // Debug.Log($"pkg.messPkg.length: {pkg.messPkg.length}, percent: {percent}");
//         if (percent == 100.0f)
//         {
//             pkg.messPkg.finish = true;
//             ParsingThePackageBody(pkg.messPkg.ret, pkg);
//         }
//     }

//     /// <summary>
//     /// 信息处理
//     /// </summary>
//     /// <param name="mess"></param>
//     /// <param name="mp"></param>
//     public static void InforProcessing(string mess, AsyncExpandPkg pkg)
//     {
//         if (mess.Count() == 0 || mess == null) return;

//         // Debug.Log("================ mess : " + mess + " || " + mess.Count());
//         string[] lengthSplit = mess.Split("|");
//         string totalLength = lengthSplit[0];
//         if (!pkg.messPkg.get_length && !string.IsNullOrEmpty(totalLength))
//         {
//             pkg.messPkg.length = int.Parse(totalLength);
//             pkg.messPkg.get_length = true;
//             pkg.messPkg.ret += lengthSplit[1];
//         }
//         else
//         {
//             if (pkg.messPkg.length > pkg.messPkg.ret.Count())
//             {
//                 pkg.messPkg.ret += mess;
//             }
//         }
//         check(pkg);
//     }
   
//     /// <summary>
//     /// Destroy Clear
//     /// </summary>
//     public void Clear()
//     {
//         Debug.Log(cliList.Count);
//         foreach (var cli in cliList)
//         {
//             cli.Close();
//         }
//         cliList.Clear();

//         m_Socket.Close();
//     }
// }


// /// <summary>
// /// 这是一个接受完整信息的 信息包类
// /// </summary>
// public class MessPackage
// {
//     public Socket socket = default; // 发送信息的soket
//     public string ip = ""; // ip
//     public string ret = ""; // 发送的信息
//     public string operate_type = ""; // 操作类型
//     public string event_type = ""; // 这个信息属于什么类型
//     public int length = 0; // 这个包的总长度
//     public bool finish = false; // 是否完全收包
//     public bool get_length = false; // 是否已经通过前置包获取到了内容包的总长度

//     public void Clear()
//     {
//         socket = default;
//         ip = "";
//         ret = "";
//         event_type = "";
//         length = 0;
//         finish = false;
//         get_length = false;
//     }

//     public MessPackage() { }

//     public MessPackage(MessPackage pkg)
//     {
//         socket = pkg.socket;
//         ip = pkg.ip;
//         ret = pkg.ret;
//         event_type = pkg.event_type;
//         operate_type = pkg.operate_type;
//         length = pkg.length;
//         finish = pkg.finish;
//         get_length = pkg.get_length;
//     }
// }


// /// <summary>
// /// 异步回调扩展包
// /// </summary>
// public class AsyncExpandPkg
// {
//     public Socket socket;
//     public MessPackage messPkg;

//     public AsyncExpandPkg() { }

//     public AsyncExpandPkg(AsyncExpandPkg pkg)
//     {
//         socket = pkg.socket;
//         messPkg = new MessPackage(pkg.messPkg);
//     }
// }

// /// <summary>
// /// 发送包
// /// </summary>
// public class SendPkg
// {
//     public Socket socket;
//     public string content;
// }