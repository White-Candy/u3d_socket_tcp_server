
using UnityEngine;

public class EventDispatcher
{   
    public void Dispatcher(params object[] objs)
    {
        AsyncExpandPkg pkg = objs[0] as AsyncExpandPkg;
        string operateType = pkg.messPkg.operateType;
        string eventType = pkg.messPkg.event_type;
        BaseEvent @event = Tools.CreateObject<BaseEvent>(eventType);

        // TODO..待优化
        switch (operateType)
        {
            case "GET":
                @event.GetInfoEvent(pkg);
                break;
            case "ADD":
                @event.AddEvent(pkg);
                break;
            case "REVISE":
                @event.ReviseInfoEvent(pkg);
                break;
            case "DELETE":
                @event.DeleteInfoEvent(pkg);
                break;       
            case "NONE":
                @event.OnEvent(objs);
                break;
        }
    }
}