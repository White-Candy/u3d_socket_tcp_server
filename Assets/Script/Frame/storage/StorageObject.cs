using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "storage", menuName = "storage/Storage_clump")]
public class StorageObject
{
    public List<ResourcesInfo> rsCheck = new List<ResourcesInfo>();
    public List<UserInfo> usersInfo = new List<UserInfo>();
    public List<FacultyInfo> faculiesInfo = new List<FacultyInfo>();
    public List<MajorInfo> majorInfo = new List<MajorInfo>();
    public List<ClassInfo> classesInfo = new List<ClassInfo>();
    public List<ColumnsInfo> columnsInfo = new List<ColumnsInfo>();
    public List<CourseInfo> courseInfo = new List<CourseInfo>();
    public List<ExamineInfo> examineesInfo = new List<ExamineInfo>();
    public List<ScoreInfo> scoresInfo = new List<ScoreInfo>();
    public List<NumOfPeopleInfo> numOfPeoInfo = new List<NumOfPeopleInfo>();
    public List<UsrTimeInfo> usrTimeInfo = new List<UsrTimeInfo>();
}

/// <summary>
/// �ͻ�����Դ����ǰ����м������
/// ����ͻ��˵İ汾��ͷ������Ĳ�һ������Ҫ����
/// </summary>
[Serializable]
public class ResourcesInfo : BaseInfo
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
    public string UnitName;
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
    public string TrainingScore;
    public string TheoryTime = "5"; // ����
    public string TrainingTime = "5"; // ����   
    public int PNum;
    public int SingleNum;
    public int MulitNum;
    public int TOFNum;
    public bool Status = false;
    public List<SingleChoice> SingleChoices = new List<SingleChoice>();
    public List<MulitChoice> MulitChoices = new List<MulitChoice>();
    public List<TOFChoice> TOFChoices = new List<TOFChoice>();

    public ExamineInfo() {}
    public ExamineInfo Clone ()
    {
        ExamineInfo inf = new ExamineInfo();
        inf.id = id;
        inf.ColumnsName = ColumnsName;
        inf.CourseName = CourseName;
        inf.RegisterTime = RegisterTime;
        inf.TrainingScore = TrainingScore;
        inf.PNum = PNum;
        inf.SingleNum = SingleNum;
        inf.MulitNum = MulitNum;
        inf.TOFNum = TOFNum;
        inf.TheoryTime = TheoryTime;
        inf.TrainingTime = TrainingTime;
        inf.Status = Status;
        foreach (var Option in SingleChoices) { inf.SingleChoices.Add(Option.Clone()); }
        foreach (var Option in MulitChoices) { inf.MulitChoices.Add(Option.Clone()); }
        foreach (var Option in TOFChoices) { inf.TOFChoices.Add(Option.Clone()); }
        return inf;
    }    
}

/// <summary>
/// ��ѡ���
/// </summary>
[Serializable]
public class SingleChoice
{
    public string Topic;
    public ItemChoice toA = new ItemChoice();
    public ItemChoice toB = new ItemChoice();
    public ItemChoice toC = new ItemChoice();
    public ItemChoice toD = new ItemChoice();
    public string Answer;
    public string Score = "";

    public SingleChoice Clone()
    {
        SingleChoice single = new SingleChoice();
        single.Topic = Topic;
        single.toA = toA.Clone();
        single.toB = toB.Clone();
        single.toC = toC.Clone();
        single.toD = toD.Clone();
        single.Answer = Answer;
        single.Score = Score;
        return single;
    }    
}

/// <summary>
/// ��ѡ
/// </summary>
[Serializable]
public class MulitChoice
{
    public string Topic;
    public List<MulitChoiceItem> Options = new List<MulitChoiceItem>(); // {{"A", "xxxxx", true}, {"B", "xxxxxxx", false}}
    public string Answer;
    public string Score = "";

    public MulitChoice Clone()
    {
        MulitChoice mulit = new MulitChoice();
        mulit.Topic = Topic;
        foreach (var Option in Options) { mulit.Options.Add(Option.Clone()); }
        mulit.Answer = Answer;
        mulit.Score = Score;
        return mulit;
    }
}

/// <summary>
/// �ж���
/// </summary>
[Serializable]
public class TOFChoice
{
    public string Topic;
    public ItemChoice toA = new ItemChoice();
    public ItemChoice toB = new ItemChoice();
    public string Answer;
    public string Score = "";

    public TOFChoice Clone()
    {
        TOFChoice tof = new TOFChoice();
        tof.Topic = Topic;
        tof.toA = toA.Clone();
        tof.toB = toB.Clone();
        tof.Answer = Answer;
        tof.Score = Score;
        return tof;
    }
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

    public ItemChoice Clone()
    {
        ItemChoice item = new ItemChoice();
        item.m_content = m_content;
        item.m_isOn = m_isOn;
        return item;
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

    public MulitChoiceItem Clone()
    {
        MulitChoiceItem item = new MulitChoiceItem();
        item.Serial = Serial;
        item.Content = Content;
        item.isOn = isOn;
        return item;
    }
}

public class BaseChoice {}

/// <summary>
/// �ɼ�������Ϣ
/// </summary>
[Serializable]
public class ScoreInfo : BaseInfo
{
    public string className;
    public string columnsName;
    public string courseName;
    public string registerTime; // �ôο��Ե�ע��ʱ��
    public string userName;
    public string Name;
    public string theoryScore;
    public string trainingScore;
    public bool theoryFinished; //�������ۿ����Ƿ����
    public bool trainingFinished; //����ʵѵ�����Ƿ����

    public ScoreInfo Clone()
    {
        ScoreInfo inf = new ScoreInfo()
        {
            className = className,
            columnsName = columnsName,
            courseName = courseName,
            registerTime = registerTime,
            userName = userName,
            Name = Name,
            theoryScore = theoryScore,
            trainingScore = trainingScore,
            theoryFinished = theoryFinished,
            trainingFinished = trainingFinished,
        };
        return inf;
    }    
}

/// <summary>
/// �˴�ͳ��
/// </summary>
[Serializable]
public class NumOfPeopleInfo : BaseInfo
{
    public string moduleName = ""; // ģ������
    public int count = 0; // ��ģ��ʹ���˴�
}

/// <summary>
/// ʱ��ͳ��
/// </summary>
[Serializable]
public class UsrTimeInfo : BaseInfo
{
    public string usrName = ""; // �û���
    public string moduleName = ""; // ģ������
    public int min = 0; // ʹ��ʱ��(����)
}