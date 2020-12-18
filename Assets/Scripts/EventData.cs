using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData
{
    public uint eventID;
}

[System.Serializable]
public struct PlayerData
{
    public int playerID;
    public string playerName;
    public string playerSex;
    public string playerCountry;
    public uint sessionId;
}

[System.Serializable]
public class KillEvent : EventData
{
    public PlayerData pdata;
    public Vector3 position;
    public Quaternion rotation;
    public string enemyType;
    public float timeStamp;
}

[System.Serializable]
public class DeathEvent : EventData
{
    public PlayerData pdata;
    public Vector3 position;
    public Quaternion rotation;
    public string enemyType;
    public float timeStamp;
}

[System.Serializable]
public class PositionEvent : EventData
{
    public PlayerData pdata;
    public Vector3 position;
    public Quaternion rotation;
    public float timeStamp;
}

[System.Serializable]
public class LifeLostEvent : EventData
{
    public PlayerData pdata;
    public Vector3 position;
    public string enemyType;
    public float timeStamp;
}

[System.Serializable]
public class BoxDestroyedEvent : EventData
{
    public PlayerData pdata;
    public Vector3 position;
    public float timeStamp;
}

[System.Serializable]
public class JumpEvent : EventData
{
    public PlayerData pdata;
    public Vector3 position;
    public float timeStamp;
}


