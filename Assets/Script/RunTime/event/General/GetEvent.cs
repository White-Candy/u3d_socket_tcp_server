using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using Unity.VisualScripting;
using UnityEngine;

public class GetEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg pkg)
    {        
        JsonData jd = AllInfoToJsData();

        string inf = JsonMapper.ToJson(jd);
        HttpServer.HttpSendAsync(pkg.Context, inf, EventType.GetEvent, OperateType.NONE);

        await UniTask.Yield();
    }

    public JsonData AllInfoToJsData()
    {
        JsonData jd = new JsonData();

        List<string> facultiesList = new List<string>(); // 学院
        List<string> classesList = new List<string>(); // 班级
        List<string> majorList = new List<string>(); // 专业
        List<string> directorsList = new List<string>(); // 主任
        List<string> deanList = new List<string>(); // 院长
        List<string> teachersList = new List<string>(); // 老师
        List<string> columnsList = new List<string>(); // 栏目

        foreach (var faculty in StorageHelper.Storage.faculiesInfo)
            facultiesList.Add(faculty.Name);

        foreach (var major in StorageHelper.Storage.majorInfo)
            majorList.Add(major.MajorName);

        foreach (var _class in StorageHelper.Storage.classesInfo)
            classesList.Add(_class.Class);

        foreach (var user in StorageHelper.Storage.usersInfo)
        {
            switch (user.Identity)
            {
                case "老师":
                teachersList.Add(user.Name);
                break;

                case "主任":
                directorsList.Add(user.Name);
                break;

                case "院长":
                deanList.Add(user.Name);
                break;
            }
        }

        foreach (var col in StorageHelper.Storage.columnsInfo)
            columnsList.Add(col.Name);


        jd["facultiesList"] = JsonMapper.ToJson(facultiesList);
        jd["classesList"] = JsonMapper.ToJson(classesList);
        jd["majorList"] = JsonMapper.ToJson(majorList);
        jd["teachersList"] = JsonMapper.ToJson(teachersList);
        jd["directorsList"] = JsonMapper.ToJson(directorsList);
        jd["deanList"] = JsonMapper.ToJson(deanList);
        jd["columnsList"] = JsonMapper.ToJson(columnsList);
        jd["coursesList"] = JsonMapper.ToJson(StorageHelper.Storage.courseInfo);
        return jd;
    }
}