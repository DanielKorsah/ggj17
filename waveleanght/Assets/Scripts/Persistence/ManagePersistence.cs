using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class ManagePersistence : MonoBehaviour {

    void onDestroy()
    {
        if (gameObject.name == "EndPortal")
        {
            HighScore();
        }
    }


	// Update is called once per frame
	void Update ()
    {

	}

    void HighScore()
    {
        float timer = GameObject.Find("Timer").GetComponent<TimeLimit>().time;

        string scene = SceneManager.GetActiveScene().name;
        string ct_write;
        string hs_path = Application.dataPath + scene + "_HighScore.Json";

        //get the time from this run
        CompletionTime ct = new CompletionTime();
        ct.t = Math.Round(timer, 3);

        //serialise to Json
        ct_write = JsonUtility.ToJson(ct);

        //if no data exists post time
        if (!File.Exists(hs_path))
        {
            File.WriteAllText(hs_path, ct_write);
        }
        else
        {
            //read current best time data
            CompletionTime ct_Read = new CompletionTime();
            ct_Read = JsonUtility.FromJson<CompletionTime>(hs_path);

            //if time is faster overwrite with new time
            if(ct.t < ct_Read.t)
            {
                File.WriteAllText(hs_path, ct_write);
            }
        }
    }
}

[Serializable]
public class CompletionTime
{
    public double t;
}
