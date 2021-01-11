
using System;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

/// <summary>
/// This 'MManager' class is the manager class for matters of the movement of the mouse subject.
/// It translates motion input into the appropriate response in the game engine.
/// It communicates with quadratic encoder through a dedicated COM port.
/// It calls for new positions during FixedUpdate & thus is independent of graphical rendering.
/// It initiates communication with arduino so there is no mistiming issues due to waiting for input
/// It records positions only during update so that mouse position & frame counts are synchronized.
/// </summary>
public class MManager : MonoBehaviour
{
    //Classes/Objects
    private Rigidbody MouseBody;
    private Vector3 MousePath;
    private Vector3 MouseDelta;
    private SerialPort serialPort;

    //Private Fields
    private int pulses;
    private float deltaMov;
    private string unityCMD = "\n";

    //Managed Fields

    //Calibrators
    private float _calibrationConversion = 1000f;
    public float CalibrationConversion
    {
        get { return _calibrationConversion; }

        private set { _calibrationConversion = value; }
    }

    private float _wheelCircumference = 59.8424f;
    public float WheelCircumference
    {
        get { return _wheelCircumference; }

        private set { _wheelCircumference = value; }
    }

    private float _defSpeed = 0.1f;
    public float DefSpeed
    {
        get { return _defSpeed; }

        private set { _defSpeed = value; }
    }

    //Mouse Position Record
    private List<float> _mousePos;
    public List<float> _MousePos
    {
        get { return _mousePos; }
    }
    public float MousePos
    {
        get { return MousePos; }

        private set { _mousePos.Add(value); }
    }
    
    //Serial Port Information
    private string _serialPortName = "COM9";
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

    // Awake is called before Start
    private void Awake()
    {
        //MY STUFF
        _mousePos = new List<float>();
        connect(SerialPortName, BaudRate);
        serialPort.Open();
        //Debug.Log("Serial Port Open");
        //Debug.Log("MManager Awake");
    }

    private void Start()
    {
        //YOUR STUFF
        MouseBody = GameObject.Find("Sphere").GetComponent<Rigidbody>();
        MousePath = MouseBody.position;
        //Debug.Log("MManager Started");
    }

    private void Update()
    {
        MousePos = MouseBody.position.x;
  
    }

    private void FixedUpdate()
    {
        //Use Fixed Update to ensure that the graphical rendering is not impacted by motion input and for proper physics interactions
        //MousePath.x = MouseBody.position.x; // Sanity Check for Updated Mouse Position
        serialPort.Write(unityCMD);
        //Debug.Log("Unity Command Sent");
        pulses = Int32.Parse(serialPort.ReadLine());
        deltaMov = DefSpeed * pulses;
        Vector3 MouseDelta = new Vector3(deltaMov, 0.0f, 0.0f);
        MouseBody.position = MouseBody.position + MouseDelta;
        //Debug.LogFormat("{0} pulses counted and the deltaMov was {1}", pulses, deltaMov);
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

    void OnDestroy()
    {
        serialPort.Close();
    }

}


