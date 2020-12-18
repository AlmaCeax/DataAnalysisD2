using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Print(string s)
    {
        //instance._Print(s);
    }

    public static void _Print(string s)
    {
        throw new NotImplementedException();
    }
}
