using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class PersistenceLogger : MonoBehaviour
{
    public string scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene().name;
        if (scene != "Main Menu" && scene != "Hub Scene"
            && scene != "Instruct" && scene != "theEnd")
        {
            Progress();
        }
        
    }


    public void HighScore()
    {
        float timer = GameObject.Find("Timer").GetComponent<TimeLimit>().time;

        
        string ct_write;
        string hs_file = scene + "_HighScore.Json";
        string hs_path = Path.Combine(Application.streamingAssetsPath, hs_file);

        //get the time from this run
        CompletionTime ct = new CompletionTime();
        ct.t = Math.Round(timer, 3);

        //serialise to Json
        ct_write = JsonUtility.ToJson(ct);

        //if no data exists post time
        if (!File.Exists(hs_path))
        {
            File.WriteAllText(hs_path, ct_write);

            Debug.Log("First time logged: " + ct.t + " seconds");
        }
        else
        {
            //read current best time data
            CompletionTime ct_read = new CompletionTime();
            string ct_as_text = File.ReadAllText(hs_path);
            ct_read = JsonUtility.FromJson<CompletionTime>(ct_as_text);

            //if time is faster overwrite with new time
            if (ct.t < ct_read.t)
            {
                File.WriteAllText(hs_path, ct_write);
                Debug.Log("New Highscore: " + scene + " in "+ ct.t + " seconds");
            }
            else
            {

                Debug.Log("Too slow: " + scene + " in "+ ct.t + " seconds");
            }
        }
    }


    void Progress()
    {
        Level level = new Level();

        level.name = SceneManager.GetActiveScene().name;

        //level name as Json
        string lvl_write = JsonUtility.ToJson(level);

        //Create file path
        string lvl_file = "SaveFile.Json";
        string lvl_path = Path.Combine(Application.streamingAssetsPath, lvl_file);

        //if no data exists post time
        if (!File.Exists(lvl_path))
        {
            File.WriteAllText(lvl_path, lvl_write);
            Debug.Log("Fresh save file: " + level.name);
        }
        else
        {

            File.WriteAllText(lvl_path, lvl_write);
            Debug.Log("Save file updated: " + level.name);
        }
    }
}

[Serializable]
public class CompletionTime
{
    public double t;
}

[Serializable]
public class Level
{
    public string name;
}
