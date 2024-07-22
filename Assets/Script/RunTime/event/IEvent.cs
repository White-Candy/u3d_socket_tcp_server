using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    ET_RESOURCES
}

public abstract class IEvent
{
    public EventType m_et;
}
