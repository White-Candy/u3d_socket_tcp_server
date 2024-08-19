using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Storage/Storage_clump")]
public class StorageObject : ScriptableObject
{
    public List<ResourcesInfo> rsCheck = new List<ResourcesInfo>();
    public List<UserInfo> userInfos = new List<UserInfo>();
    public List<FacultyInfo> faculiesInfo = new List<FacultyInfo>();
}

/// <summary>
/// 客户端资源更新前会进行检查请求
/// 如果客户端的版本码和服务器的不一致则需要更新
/// </summary>
[Serializable]
public class ResourcesInfo
{
    public string relaPath;
    public string version_code;
    public bool need_updata;


    public ResourcesInfo() { }

    public ResourcesInfo(ResourcesInfo clone)
    {
        relaPath = clone.relaPath;
        version_code = clone.version_code;
        need_updata = clone.need_updata;
    }
}

/// <summary>
///  学院信息包
/// </summary>
[Serializable]
public class FacultyInfo
{
    public string id;
    public string Name;
    public string RegisterTime;
    public string TeacherName;
}

/// <summary>
/// 用户信息包
/// </summary>
[Serializable]
public class UserInfo
{
    public string userName;
    public string Name;
    public string Gender;
    public string idCoder;
    public string Age;
    public string Contact;
    public string HeadTeacher;
    public string className;
    public string password;
    public int level;
    public bool login = false;
    public string hint = "";
}