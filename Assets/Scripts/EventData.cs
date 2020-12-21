using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData
{
    public uint eventID;
    public Vector3 position;
    public PlayerData pdata;
    public Quaternion rotation;
}

[System.Serializable]
public struct PlayerData
{
    public int playerID;
    public string playerName;
    public string playerSex;
    public string playerCountry;
    public string sessionId;
}

[System.Serializable]
public class KillEvent : EventData
{
    public string enemyType;
    public float timeStamp;
}

[System.Serializable]
public class DeathEvent : EventData
{
    public string enemyType;
    public float timeStamp;
}

[System.Serializable]
public class PositionEvent : EventData
{
    public float timeStamp;
}

[System.Serializable]
public class LifeLostEvent : EventData
{
    public string enemyType;
    public float timeStamp;
}

[System.Serializable]
public class BoxDestroyedEvent : EventData
{
    public float timeStamp;
}

[System.Serializable]
public class JumpEvent : EventData
{
    public float timeStamp;
}


