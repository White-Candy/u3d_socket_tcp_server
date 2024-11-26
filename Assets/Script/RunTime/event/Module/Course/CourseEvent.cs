using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class CourseEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<CourseInfo> infs = await StorageHelper.GetInfo(StorageHelper.storage.courseInfo);
        
        string inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, inf, EventType.CourseEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.storage.courseInfo, x => x.CourseName == info.CourseName);

        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.CourseEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.storage.courseInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.CourseEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.storage.courseInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.CourseEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        CourseInfo info = JsonMapper.ToObject<CourseInfo>(pkg.messPkg.ret);
        List<CourseInfo> inf = StorageHelper.SearchInf(StorageHelper.storage.courseInfo, x => x.CourseName == info.CourseName);

        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.CourseEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }  
}