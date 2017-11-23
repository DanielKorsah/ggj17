using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public List<string> UnlockedLevels { get; set; } = new List<string>();
    public List<float> BestTimes { get; set; } = new List<float>();
    public string LastPlayed { get; set; }
    public bool Mode { get; set; }

    //start constructor
    public Data(string firstLevel)
    {
        Debug.Log("Steup Data");
        Mode = false;
        UnlockedLevels = new List<string> { firstLevel };
        BestTimes = new List<float> {};
    }

    public Data()
    {
        Debug.Log("Regular Data");
        Mode = false;
        UnlockedLevels = new List<string> {};
        BestTimes = new List<float> {};
    }
}