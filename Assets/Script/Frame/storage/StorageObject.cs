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
/// �ͻ�����Դ����ǰ����м������
/// ����ͻ��˵İ汾��ͷ������Ĳ�һ������Ҫ����
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
///  ѧԺ��Ϣ��
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
/// �û���Ϣ��
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