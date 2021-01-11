
using System;
using System.Text;
using CsvHelper;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using UnityEngine;

/// <summary>
/// This 'DManager' class is the manager class for data management.
/// It communicates with SQLite to rapidly store behavioral data.
/// </summary>
public class DManager : MonoBehaviour
{
    //Classes/Objects
    private GameObject FeAR;
    private MManager MM;
    private FManager FM;

    //Protected Fields
    private List<float> _mousePos;
    private int startFrame;
    private int habFrames;
    private int totalFrames;
    private int TotalTime;
    private int HabTime;
    private int NumStim;
    private Dictionary<string, int> stimFramePairs;

    //Managed Fields
    private string _destination;
    public string Destination
    {
        get { return _destination;  }

        private set { _destination = value; }
    }

    private void Awake()
    {
        //MY STUFF
        _mousePos = new List<float>();
        Destination = "C:\\Users\\YUSTE\\SAVE.dat";
    }
    // Start is called before the first frame update
    private void Start()
    {
        //YOUR STUFF
        FeAR = GameObject.Find("FeAR_Manager");
        MM = FeAR.GetComponent<MManager>();
        FM = FeAR.GetComponent<FManager>();
        stimFramePairs = FM.StimFramePairs;
        startFrame = FM.StartFrame;
        habFrames = FM.HabFrames;
        totalFrames = FM.TotalFrames;
        TotalTime = FM.TotalTime;
        HabTime = FM.HabTime;
        NumStim = FM.NumStim;

    }

    // Update is called once per frame
    private void Update()
    {
        //GRAB DATA ONLY AT END TO AVOID DOUBLING THE MEMORY LEAK
        //PROBABLY NOT WORTH EFFORT TO OPTIMIZE DATA SAVING FOR STREAMING
        //TO AVOID THE SINGLE MEMORY LEAK GIVEN THE SHORT BEHAVIORAL TIME

    }

    public void DataExport()
    {
        //EXPORT CALL
        _mousePos = MM._MousePos;
        //SaveData();
        SaveData2();
    }

    public void SaveData()
    {
        //SAVE DATA
        FileStream file;
        if (File.Exists(Destination))
        {
            file = File.OpenWrite(Destination);
        }
        else
        {
            file = File.Create(Destination);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, _mousePos);
        //bf.Serialize(file, startFrame);
        //bf.Serialize(file, habFrames);
        //bf.Serialize(file, totalFrames);
        //bf.Serialize(file, TotalTime);
        //bf.Serialize(file, HabTime);
        //bf.Serialize(file, NumStim);
        //bf.Serialize(file, stimFramePairs);
        file.Close();
    }

    public void SaveData2()
    {
       using (var writer = new StreamWriter("C:\\Users\\YUSTE\\SAVE_FILE.csv"))
       using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(_mousePos);
        }
    }
}
