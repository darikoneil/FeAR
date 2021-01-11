using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.IO.Ports;
using UnityEngine;

/// <summary>
/// This 'SManager' class is the manager class for the implementation of unconditioned stimuli. Upon receiving the appropriate
/// timeline, it sends commands to a microcontroller to initiate stimuli & handles interface for synchronization & triggers. 
/// Although using FixedUpdate may be more accurate in realtime, using Update ensuring that the stimulus & synchronization drives are stable in relation to rendered frames
/// </summary>
public class SManager : MonoBehaviour
{
    //Other Classes & Objects
    private FManager FM;
    private SerialPort serialPort;

    //Protected Fields
    private string unityCMD = "\n";

    //Managed Fields
    //Serial Port Information
    private string _serialPortName = "COM3";
    public string SerialPortName
    {
        get { return _serialPortName; }

        private set { _serialPortName = value; }
    }

    private int _baudRate = 57600;
    public int BaudRate
    {
        get { return _baudRate; }

        private set { _baudRate = value; }
    }

    private void Awake()
    {
        //MY STUFF
        connect(SerialPortName, BaudRate);
        serialPort.Open();
        //Debug.LogFormat("SManager Serial Port Open at {0} ", SerialPortName);
        //Debug.Log("SManager Awake");
    }
    private void Start()
    {
        //YOUR STUFF
        FM = GetComponent<FManager>();

        //Start the coroutine for syncs (here because closer to 1st frame than enable)
        StartCoroutine(SyncPulse());
    }

    // Update is called once per frame
    private void Update()
    {
       // if (FM.StartTrigger)
       // {
            //Deliver Start Pulse
            //This Checks Latency for Time Adjustments
       // }
       // if (FM.StimTrigger)
       // {
            //access Dictionary and check frames
            //If bool
            //Deliver the stimulus
       // }
//
       // if (FM.EndTrigger)
      //  {
        //    //Deliver End Pulse to Imaging
       // }

        //Send the sync pulse


    }

    private void connect(string SerialPortName, Int32 BaudRate)
    {
        serialPort = new SerialPort(SerialPortName, BaudRate);
        serialPort.PortName = SerialPortName; //Redunancy Sanity Check
        serialPort.BaudRate = BaudRate; //Redunancy Sanity Check
        serialPort.DataBits = 8; //Redunancy Sanity Check
        serialPort.Parity = Parity.None; //Redunancy Sanity Check
        serialPort.StopBits = StopBits.One; //Redunancy Sanity Check
        //Debug.Log("Serial Port Created");
    }

    private void OnDestroy()
    {
        serialPort.Close();
    }
    
    private IEnumerator SyncPulse()
    {
        while(true)
        {
            serialPort.Write(unityCMD);
            yield return new WaitForSeconds(1);
            //Debug.Log("SENT SYNC PULSE");
        }
    }

}
