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
    public List<ExamineInfo> examineesInfo = new List<ExamineInfo>();
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

/// <summary>
///  ������Ϣ��
/// </summary>
[Serializable]
public class ExamineInfo : BaseInfo
{
    public string id;
    public string ColumnsName;
    public string CourseName;
    public string RegisterTime;
    public int TrainingScore;
    public int ClassNum;
    public int SingleNum;
    public int MulitNum;
    public int TOFNum;
    public bool Status;
    public List<SingleChoice> SingleChoices = new List<SingleChoice>();
    public List<MulitChoice> MulitChoices = new List<MulitChoice>();
    public List<TOFChoice> TOFChoices = new List<TOFChoice>();
}

/// <summary>
/// ��ѡ���
/// </summary>
[Serializable]
public class SingleChoice : BaseChoice
{
    public string Topic;
    public ItemChoice toA = new ItemChoice();
    public ItemChoice toB = new ItemChoice();
    public ItemChoice toC = new ItemChoice();
    public ItemChoice toD = new ItemChoice();
    public string Answer;
    public int Score = 0;
}

/// <summary>
/// ��ѡ
/// </summary>
[Serializable]
public class MulitChoice : BaseChoice
{
    public string Topic;
    public List<MulitChoiceItem> Options = new List<MulitChoiceItem>(); // {{"A", "xxxxx", true}, {"B", "xxxxxxx", false}}
    public string Answer;
    public int Score;
}

/// <summary>
/// �ж���
/// </summary>
[Serializable]
public class TOFChoice : BaseChoice
{
    public string Topic;
    public ItemChoice toA = new ItemChoice();
    public ItemChoice toB = new ItemChoice();
    public string Answer;
    public int Score;
}

/// <summary>
/// ����ģʽ�� һ��ѡ�����Ϣ
/// </summary>
[Serializable]
public class ItemChoice
{
    public string m_content = "";
    public bool m_isOn = false;

    public ItemChoice() {}

    public ItemChoice(string content, bool ison)
    {
        m_content = content;
        m_isOn = ison;
    }
}

/// <summary>
/// ��Ϊ��������û�а취 ���л� �ֵ����ͣ�����Ϊ�˱����ѡ���ѡ���Ҫ�Զ���һ����
/// </summary>
[Serializable]
public class MulitChoiceItem
{
    public string Serial = "A";
    public string Content = "";
    public bool isOn = false;

    public MulitChoiceItem() {}
    public MulitChoiceItem(string serial, string content, bool isOn)
    {
        Serial = serial;
        Content = content;
        this.isOn = isOn;
    }
}


public class BaseChoice {}