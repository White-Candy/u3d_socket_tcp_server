using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitJson;

public class ClassEvent : BaseEvent
{
    public override async void GetInfoEvent(AsyncExpandPkg pkg)
    {
        List<ClassInfo> infs = await StorageHelper.GetInfo(StorageHelper.storage.classesInfo);

        Dictionary<string, int> classesNumber = new Dictionary<string, int>();
        foreach (var usr in StorageHelper.storage.usersInfo)
        {
            if (usr.Identity != "学生") continue;
            
            if (usr.UnitName != null && !classesNumber.ContainsKey(usr.UnitName))
            {
                classesNumber.Add(usr.UnitName, 0);
            }         

            if (usr.UnitName != null) classesNumber[usr.UnitName]++; 
        }

        foreach (var _class in infs)
        {
            _class.Number = classesNumber.ContainsKey(_class.Class) ? classesNumber[_class.Class] : 0;
        }

        string inf = JsonMapper.ToJson(infs);
        SocketServer.SendAsync(pkg.socket, inf, EventType.ClassEvent, OperateType.GET);
    }

    public override async void AddEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> new_list = await StorageHelper.AddInfo(info, StorageHelper.storage.classesInfo, x => x.Class == info.Class);

        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.ClassEvent, OperateType.ADD);
    }

    public override async void ReviseInfoEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> inf = await StorageHelper.ReviseInfo(info, StorageHelper.storage.classesInfo, x => x.id == info.id);
        
        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.ClassEvent, OperateType.REVISE);
    }

    public override async void DeleteInfoEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> new_list = 
            await StorageHelper.DeleteInfo(StorageHelper.storage.classesInfo, (x) => {return x.id == info.id;});
        
        string body = JsonMapper.ToJson(new_list);
        SocketServer.SendAsync(pkg.socket, body, EventType.ClassEvent, OperateType.DELETE);
    }

    public override async void SearchInfoEvent(AsyncExpandPkg pkg)
    {
        ClassInfo info = JsonMapper.ToObject<ClassInfo>(pkg.messPkg.ret);
        List<ClassInfo> inf = StorageHelper.SearchInf(StorageHelper.storage.classesInfo, x => x.Class == info.Class);

        string s_inf = JsonMapper.ToJson(inf);
        SocketServer.SendAsync(pkg.socket, s_inf, EventType.ClassEvent, OperateType.SEARCH);
        await UniTask.Yield();
    }    
}