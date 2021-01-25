using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This 'FManager' class is the manager class for construction & execution of the actual behavioral paradigm.
/// It determines what & when unconditioned stimuli are introduced & synchronizes imaging with linked triggers.
/// </summary>
/// 
public class FManager : MonoBehaviour
{
    //Other Objects/Classes
    private GameObject FeAR;
    private DManager DM;

    //Unmanaged Fields
    public readonly int NumStim = 5;
    public readonly int HabTime = 120; //seconds
    public readonly int TotalTime = 1680; //seconds

    //Protected Fields
    private Dictionary<string, int> stimTimePairs;
    private int startFrame=0;
    private int tempAns;
    private int tempAnsFrames;
    private int habFrames;
    private int totalFrames;
    private int frameRate = 60;

    //Managed Fields
    public int StartFrame
    {
        get { return startFrame; }

    }
    public int TotalFrames
    {
        get { return totalFrames; }
    }
    public int HabFrames
    {
        get { return habFrames; }
    }
    public int FrameRate
    {
        get { return frameRate; }
    }
    //private bool _stimTrigger;
    //public bool StimTrigger
   // {
     //   get { return _stimTrigger; }
     //
       // private set { _stimTrigger = value; }
    //}
    //private int _stimStartFrame;
    //public int StimStartFrame
    //{
      //  get { return _stimStartFrame; }
        //
        //private set { _stimStartFrame = value; }
    //}
    //private bool _endTrigger;
    //public bool EndTrigger
    //{
      //  get { return _endTrigger; }
      //
        //private set { _endTrigger = value; }
    //}
    //private bool _startTrigger=true;
   // public bool StartTrigger
    //{
      //  get { return _startTrigger; }
      //
        //private set { _startTrigger = value; }
   // }
    private Dictionary<string, int> _stimFramePairs;
    public Dictionary<string, int> StimFramePairs;
    void Awake()
    {
        //MY STUFF
        stimTimePairs = new Dictionary<string, int>();
        _stimFramePairs = new Dictionary<string, int>();
        CreateUCS(NumStim,HabTime,TotalTime);
        //Debug.LogFormat("Stim Times are {0},{1},{2},{3},{4}", stimTimePairs["Stim 1"], stimTimePairs["Stim 2"], stimTimePairs["Stim 3"], stimTimePairs["Stim 4"], stimTimePairs["Stim 5"]);
        //Debug.Log("FeAR Manager is Awake");
    }
    
    void Start()
    {
        //YOUR STUFF;
        FeAR = GameObject.Find("FeAR_Manager");
        DM = FeAR.GetComponent<DManager>();
        //Debug.Log("FManager Started");
    }

     void Update()
    {

        if(Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;
            DM.DataExport();
            Debug.Log("Data Exported");
            Application.Quit();

        }
       // if(StartTrigger==false)
       // {
         //   StartTrigger = CheckStartTrigger();
         //   if (StartTrigger != false)
          //  {
          //      startFrame = Time.frameCount;
          //  }
       // }
       // if(CheckHab(startFrame,HabTime))
       // {
       //     StimStartFrame = Time.frameCount;
       //     StimTrigger = true;
       // }
       if(CheckTotalFrames(totalFrames,startFrame))
        {
            Time.timeScale = 0;
            DM.DataExport();
            //Debug.Log("Data Exported");
            Application.Quit();
        }
       //     EndTrigger = true;
      //  }
       // Debug.Log("FManager Updated");
    }
 
    private void CreateUCS(int NumStim, int HabTime, int TotalTime)
    {
        //convert times to frames
        totalFrames = TotalTime *= frameRate;
        habFrames = HabTime *= frameRate;
        ///This creates the behavioral timeline
       tempAns= (TotalTime-(HabTime+60)) / NumStim;
        tempAnsFrames = tempAns *= frameRate;
        //Debug.Log(tempAns);
        for(int i = 1; i <= NumStim; i++)
        {
            stimTimePairs.Add("Stim "+i, ((tempAns*i)+HabTime));
            _stimFramePairs.Add("Stim " + i + "", ((tempAnsFrames * i) + habFrames + startFrame));
            //Debug.LogFormat("{0} is tempAns and {1} is i", tempAns, i);
        }
    }

  //  private bool CheckStartTrigger()
   // {
   //     Debug.Log("CheckStartTrigger Assessed");
   //     return StartTrigger;
   // }

    private bool CheckHab(int startFrame, int habFrames)
    {
        if(Time.frameCount==(startFrame+habFrames))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckTotalFrames(int totalFrames, int startFrame)
    {
        if((totalFrames+startFrame)==Time.frameCount || (totalFrames+startFrame<Time.frameCount))
        {
            //Behavior is over
            return true;
        }
        else
        {
            //Behavior is running
            return false;
        }
    }
}
