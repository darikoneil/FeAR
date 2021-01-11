
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageListener : MonoBehaviour
{

    public int VAL;
    // Start is called before the first frame update
    void OnMessageArrived(string msg)
    {
        VAL = Int32.Parse(msg);
        Debug.LogFormat("The Val is {0}", VAL);
    }

    // Update is called once per frame
    void OnConnectionEvent(bool success)
    {
        
    }
}
