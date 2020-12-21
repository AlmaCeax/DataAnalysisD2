using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventList<T>
{
    [SerializeField]
    public List<T> events;
    public EventList() { events = new List<T>(); }
}
