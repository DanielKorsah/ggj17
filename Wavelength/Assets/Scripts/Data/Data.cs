using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public List<string> UnlockedLevels { get; set; }
    public List<float> BestTimes { get; set; }
    public string LastPlayed { get; set; }
    public bool Mode { get; set; }

    //default data constructor
    public Data()
    {
        Debug.Log("Constructor");
        Mode = false;
        UnlockedLevels = new List<string> {};
        BestTimes = new List<float> {};
    }
}