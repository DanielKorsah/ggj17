using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public List<string> UnlockedLevels { get; set; }
    List<float> BestTimes { get; set; }
    string LastPlayed { get; set; }
    bool Mode { get; set; }

    //default data constructor
    public Data()
    {
        Mode = true;
        UnlockedLevels = new List<string> { };
        BestTimes = new List<float> { };
        LastPlayed = null;
    }
}