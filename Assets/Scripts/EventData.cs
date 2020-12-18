using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData
{
    public uint eventID;
}

[System.Serializable]
public class KillEvent : EventData
{
    public Vector3 position;
    public Quaternion rotation;
    public string enemyType;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

[System.Serializable]
public class DeathEvent : EventData
{
    public Vector3 position;
    public Quaternion rotation;
    public string enemyType;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

[System.Serializable]
public class PositionEvent : EventData
{
    public Vector3 position;
    public Quaternion rotation;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

[System.Serializable]
public class LifeLostEvent : EventData
{
    public Vector3 position;
    public uint playerId;
    public uint sessionId;
    public string enemyType;
    public float timeStamp;
}

[System.Serializable]
public class BoxDestroyedEvent : EventData
{
    public Vector3 position;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}

[System.Serializable]
public class JumpEvent : EventData
{
    public Vector3 position;
    public uint playerId;
    public uint sessionId;
    public float timeStamp;
}


