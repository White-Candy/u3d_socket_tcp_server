using Cysharp.Threading.Tasks;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetProjInfo : BaseEvent
{
    public override async void OnEvent(AsyncExpandPkg asynExPkg)
    {
        await UniTask.Yield();

        List<Proj> projs = new List<Proj>();
        foreach (var item in StorageHelper.Storage.columnsInfo)
        {
            Proj proj = new Proj
            {
                Columns = item.Name
            };

            List<string> courses = new List<string>();
            foreach (var course in StorageHelper.Storage.courseInfo)
            {
                if (course.Columns == item.Name)
                {
                    courses.Add(course.CourseName);
                }
            }
            proj.Courses = courses;
            projs.Add(proj);
        }

        string inf = JsonMapper.ToJson(projs);
        NetworkTCPServer.SendAsync(asynExPkg.socket, inf, EventType.GetProjInfo, OperateType.NONE);
    }
}

public struct Proj
{
    public string Columns; //项目名字
    public List<string> Courses; // 子项目列表姓名
}