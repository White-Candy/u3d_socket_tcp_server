using LitJson;
using UnityEngine;

//public class LoginData
//{
//    public string username;
//    public string password;
//}

public class Client : MonoBehaviour
{
    [HideInInspector]
    public static Server m_Server;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        m_Server = GetComponent<Server>();
    }

    /// <summary>
    /// Client ��¼����
    /// </summary>
    /// <param name="path"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public static async void Login(string path, string username, string password)
    {
        // NetHelper.UserLoginReq(username, password);
        //TODO..
        // Debug.Log($"Login Path: {path}");
        // UserInfo inf = new UserInfo
        // {
        //     userName = username,
        //     password = password
        // };

        // string str = NetHelper.GetFinalBody(JsonMapper.ToJson(inf), EventType.UserLoginEvent, OperateType.NONE);
        // await m_Server.Post(path, str, (text) => 
        // {
        //     Debug.Log(text);
        // });
    }
}
