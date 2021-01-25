using System;
using System.Collections;
using System.Collections.Generic;
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
    private SerialPort stimPort;
    private Dictionary<string, int> stimFramePairs;
    //private int i = 1;

    //Protected Fields
    private string unityCMD = "\n";

    //Managed Fields
    //Serial Port Information
    private string _serialPortName = "COM14";
    public string SerialPortName
    {
        get { return _serialPortName; }

        private set { _serialPortName = value; }
    }

    private string _stimPortName = "COM3";
    public string StimPortName
    {
        get { return _stimPortName; }

        private set { _stimPortName = value; }
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

        //SYNC PORT
        connect(SerialPortName, BaudRate);
        serialPort.Open();
        //STIM PORT
        connect_stim(StimPortName, BaudRate);
        stimPort.Open();

        //Debug.LogFormat("SManager Serial Port Open at {0} ", SerialPortName);
        //Debug.Log("SManager Awake");
    }
    private void Start()
    {
        //YOUR STUFF
        FM = GetComponent<FManager>();
        stimFramePairs = new Dictionary<string, int>();
        stimFramePairs = FM.StimFramePairs;

        //Start the coroutine for syncs (here because closer to 1st frame than enable)
        StartCoroutine(SyncPulse());
        //StartCoroutine(StimPulse(i));
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

    private void connect_stim(string SerialPortName, Int32 BaudRate)
    {
        stimPort = new SerialPort(SerialPortName, BaudRate);
        stimPort.PortName = SerialPortName; //Redunancy Sanity Check
        stimPort.BaudRate = BaudRate; //Redunancy Sanity Check
        stimPort.DataBits = 8; //Redunancy Sanity Check
        stimPort.Parity = Parity.None; //Redunancy Sanity Check
        stimPort.StopBits = StopBits.One; //Redunancy Sanity Check
        //Debug.Log("Stim Port Created");
    }

    private void OnDestroy()
    {
        serialPort.Close();
    }

    private IEnumerator SyncPulse()
    {
        while (true)
        {
            serialPort.Write(unityCMD);
            yield return new WaitForSeconds(1);
            //Debug.Log("SENT SYNC PULSE");
        }
    }

    //private IEnumerator StimPulse(int i)
    //{
     //   int tempFrame = 0;
    //   string tempStimString;
      //  int tempStimFrame;
      //
      //  tempFrame = Time.frameCount;
       // tempStimString = "Stim " + i + "";
       // stimFramePairs.TryGetValue(tempStimString, out tempStimFrame);
       //
       // yield return new WaitUntil(() => tempFrame >= tempStimFrame);
       // Debug.LogFormat("SENT {0} PULSE", i);
       // i++;
    //}

}
