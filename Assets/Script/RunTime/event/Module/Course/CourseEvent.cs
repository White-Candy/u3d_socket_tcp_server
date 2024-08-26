using System.Collections.Generic;
using LitJson;

public class CourseEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<CourseInfo> infs = await StorageHelper.GetInfo(StorageHelper.Storage.courseInfo);
        
        string inf = JsonMapper.ToJson(infs);
        NetworkTCPServer.SendAsync(pkg.socket, inf, EventType.CourseEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.Storage.courseInfo, x => x.CourseName == info.CourseName);

        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.CourseEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.Storage.courseInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        NetworkTCPServer.SendAsync(pkg.socket, s_inf, EventType.CourseEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.Storage.courseInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        NetworkTCPServer.SendAsync(pkg.socket, body, EventType.CourseEvent, OperateType.DELETE);
    }
}