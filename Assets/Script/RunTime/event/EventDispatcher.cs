
using UnityEngine;

public class EventDispatcher
{   
    public void Dispatcher(params object[] objs)
    {
        AsyncExpandPkg pkg = objs[0] as AsyncExpandPkg;
        string operateType = pkg.messPkg.operateType;
        string eventType = pkg.messPkg.event_type;
        Debug.Log(eventType + " || " + operateType);
        BaseEvent @event = Tools.CreateObject<BaseEvent>(eventType);

        // TODO..待优化
        switch (operateType)
        {
            case "Get":
                @event.GetInfoEvent(pkg);
                break;
            case "Add":
                @event.AddEvent(pkg);
                break;
            case "Revise":
                @event.ReviseInfoEvent(pkg);
                break;
            case "Delete":
                @event.DeleteInfoEvent(pkg);
                break;       
            case "None":
                @event.OnEvent(objs);
                break;
        }
    }
}