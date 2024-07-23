using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEvent
{
    public virtual void OnEvent(params object[] objs) { }
}
