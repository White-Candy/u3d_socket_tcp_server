using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;

public class GetEvent : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg pkg)
    {        
        JsonData jd = new JsonData();

        List<string> facultiesList = new List<string>(); // 学院
        List<string> classesList = new List<string>(); // 班级
        List<string> majorList = new List<string>(); // 专业
        List<string> directorsList = new List<string>(); // 主任
        List<string> deanList = new List<string>(); // 院长
        List<string> teachersList = new List<string>(); // 老师

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

        jd["facultiesList"] = JsonMapper.ToJson(facultiesList);
        jd["classesList"] = JsonMapper.ToJson(classesList);
        jd["majorList"] = JsonMapper.ToJson(majorList);
        jd["teachersList"] = JsonMapper.ToJson(teachersList);
        jd["directorsList"] = JsonMapper.ToJson(directorsList);
        jd["deanList"] = JsonMapper.ToJson(deanList);

        string inf = JsonMapper.ToJson(jd);
        NetworkTCPServer.SendAsync(pkg.socket, inf, EventType.GetEvent, OperateType.NONE);

        await UniTask.Yield();
    }
}