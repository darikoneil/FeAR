using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class COM_SERIAL : MonoBehaviour
{
    public string portName = "COM9";
    public int baudRate = 19200;
    public GameObject messageListener;
    public int reconnectionDelay = 500;
    public int maxUnreadMessages = 1;
    public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
    public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";
    protected Thread thread;
    protected SerialThreadLines serialThread;
    private string unityCmd = "\n";
    public int VAL;

      void OnEnable()
{
    serialThread = new SerialThreadLines(portName, baudRate, reconnectionDelay, maxUnreadMessages);
    thread = new Thread(new ThreadStart(serialThread.RunForever));
    thread.Start();
}

void OnDisable()
{
    // If there is a user-defined tear-down function, execute it before
    // closing the underlying COM port.
    if (userDefinedTearDownFunction != null)
        userDefinedTearDownFunction();

    // The serialThread reference should never be null at this point,
    // unless an Exception happened in the OnEnable(), in which case I've
    // no idea what face Unity will make.
    if (serialThread != null)
    {
        serialThread.RequestStop();
        serialThread = null;
    }

    // This reference shouldn't be null at this point anyway.
    if (thread != null)
    {
        thread.Join();
        thread = null;
    }
}

void Update()
{
        SendSerialMessage(unityCmd);
        VAL = Int32.Parse(ReadSerialMessage());
        Debug.LogFormat("The Number of Pulses is {0}", VAL);





    // If the user prefers to poll the messages instead of receiving them
    // via SendMessage, then the message listener should be null.
   // if (messageListener == null)
      //  return;

    // Read the next message from the queue
    //string message = (string)serialThread.ReadMessage();
   // if (message == null)
    //    return;

    // Check if the message is plain data or a connect/disconnect event.
   // if (ReferenceEquals(message, SERIAL_DEVICE_CONNECTED))
    //    messageListener.SendMessage("OnConnectionEvent", true);
  //  else if (ReferenceEquals(message, SERIAL_DEVICE_DISCONNECTED))
   //    messageListener.SendMessage("OnConnectionEvent", false);
   // else
    //    messageListener.SendMessage("OnMessageArrived", message);
}

public string ReadSerialMessage()
{
    // Read the next message from the queue
    return (string)serialThread.ReadMessage();
}

public void SendSerialMessage(string message)
{
    serialThread.SendMessage(message);
}


// ------------------------------------------------------------------------
// Executes a user-defined function before Unity closes the COM port, so
// the user can send some tear-down message to the hardware reliably.
// ------------------------------------------------------------------------
public delegate void TearDownFunction();
private TearDownFunction userDefinedTearDownFunction;
public void SetTearDownFunction(TearDownFunction userFunction)
{
    this.userDefinedTearDownFunction = userFunction;
}

}
