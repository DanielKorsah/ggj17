using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public List<string> UnlockedLevels { get; set; } = new List<string> ();
    public List<float> BestTimes { get; set; } = new List<float> ();
    public string LastPlayed { get; set; }
    public bool Mode { get; set; }

    private static Data instance;

    public static Data Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Data ();
                Debug.Log ("Steup Data");
                instance.UnlockedLevels = new List<string> { "tut1" };
                instance.BestTimes = new List<float> { float.MaxValue };
            }
            else
            {
                Debug.Log ("Regular Data");
            }

            return instance;
        }
    }
}