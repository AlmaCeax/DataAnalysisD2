using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData
{
    public uint eventID;
}

public class KillEvent : EventData
{
    public Vector3 position;
    public Quaternion rotation;
    public string enemyType;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

public class DeathEvent : EventData
{
    public Vector3 position;
    public Quaternion rotation;
    public string enemyType;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

public class PositionEvent : EventData
{
    public Vector3 position;
    public Quaternion rotation;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

public class LifeLostEvent : EventData
{
    public Vector3 position;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

public class BoxDestroyedEvent : EventData
{
    public Vector3 position;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

public class JumpEvent : EventData
{
    public Vector3 position;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}


