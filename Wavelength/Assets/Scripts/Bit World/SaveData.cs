using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static readonly string MaxLevelString = "MaxLevel";

    private void Start()
    {
        BitWorldMaker.MaxLevel = GetMaxLevelData();    
    }

    public static int GetMaxLevelData()
    {
        if (PlayerPrefs.HasKey(MaxLevelString))
        {
            return PlayerPrefs.GetInt(MaxLevelString);
        }
        else
        {
            return 0;
        }
    }

    public static void WriteMaxLevelData(int max)
    {
        PlayerPrefs.SetInt(MaxLevelString, max);
        PlayerPrefs.Save();
    }

    public static void RemovePlayerData()
    {
        PlayerPrefs.DeleteAll();
    }
}
