using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Writer : MonoBehaviour
{
    public static Writer instance = null;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void GenerateJsonString<T>(EventList<T> eventsData, string fileName)
    {
        string json = JsonUtility.ToJson(eventsData);
        string path = Application.dataPath + "/Data/" + fileName;
        File.WriteAllText(path, json);
    }

    public void ReadFile<T>(ref EventList<T> eventsData, string fileName)
    {
        string path = Application.dataPath + "/Data/" + fileName;
        string json = File.ReadAllText(path);
        eventsData = JsonUtility.FromJson<EventList<T>>(json);
    }
}
