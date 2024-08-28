using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Storage/Storage_clump")]
public class StorageObject : ScriptableObject
{
    public List<ResourcesInfo> rsCheck = new List<ResourcesInfo>();
    public List<UserInfo> usersInfo = new List<UserInfo>();
    public List<FacultyInfo> faculiesInfo = new List<FacultyInfo>();
    public List<MajorInfo> majorInfo = new List<MajorInfo>();
    public List<ClassInfo> classesInfo = new List<ClassInfo>();
    public List<ColumnsInfo> columnsInfo = new List<ColumnsInfo>();
    public List<CourseInfo> courseInfo = new List<CourseInfo>();
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

public class BaseInfo {}

/// <summary>
///  ѧԺ��Ϣ��
/// </summary>
[Serializable]
public class FacultyInfo : BaseInfo
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
public class UserInfo : BaseInfo
{
    public string userName;
    public string password;
    public string Name;
    public string Gender;
    public string Age;
    public string Identity;
    public string idCoder;
    public string Contact;
    public string className;
    public bool login = false;
    public string hint = "";
}

/// <summary>
///  רҵ��Ϣ��
/// </summary>
[Serializable]
public class MajorInfo : BaseInfo
{
    public string id;
    public string MajorName;
    public string RegisterTime;
    public string FacultyName;
    public string TeacherName;
}

/// <summary>
///  �༶��Ϣ��
/// </summary>
[Serializable]
public class ClassInfo : BaseInfo
{
    public string id;
    public string Class;
    public string RegisterTime;
    public string Faculty;
    public string Major;
    public string Teacher;
    public int Number;
}

/// <summary>
///  ��Ŀ��Ϣ��
/// </summary>
[Serializable]
public class ColumnsInfo : BaseInfo
{
    public string id;
    public string Name;
    public string RegisterTime;
}

/// <summary>
///  �γ���Ϣ��
/// </summary>
[Serializable]
public class CourseInfo : BaseInfo
{
    public string id;
    public string CourseName;
    public string Columns;
    public string Working;
    public string RegisterTime;
}