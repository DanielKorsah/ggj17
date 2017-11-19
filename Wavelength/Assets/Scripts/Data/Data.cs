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
        Mode = true;
        UnlockedLevels = new List<string> { "tut1" };
        BestTimes = new List<float> { };
        LastPlayed = UnlockedLevels[UnlockedLevels.Count - 1];
    }
}