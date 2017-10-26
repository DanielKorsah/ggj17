using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class PersistenceLogger : MonoBehaviour
{
    public string scene;
    LoadFromSave currentData;
    SaveData saveData;
    string saveFile;
    string savePath;


    void Start()
    {
        currentData = LoadFromSave.Instance;
        saveData = SaveData.Instance;

        saveFile = "SaveFile.Json";
        savePath = Path.Combine(Application.streamingAssetsPath, saveFile);

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

        saveData.t = Math.Round(timer, 3);

        //serialise to Json
        string saveWrite = JsonUtility.ToJson(saveData);

        //if no data exists post time
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, saveWrite);

            Debug.Log("First time logged: " + saveData.t + " seconds");
        }
        else
        {
            //read current best time data
            SaveData saveRead = new SaveData();
            string dataText = File.ReadAllText(savePath);
            saveRead = JsonUtility.FromJson<SaveData>(dataText);

            //if time is faster overwrite with new time
            if (saveData.t < saveRead.t)
            {
                File.WriteAllText(savePath, saveWrite);
                Debug.Log("New Highscore: " + scene + " in " + saveData.t + " seconds");
            }
            else
            {
                Debug.Log("Too slow: " + scene + " in " + saveData.t + " seconds");
            }
        }
    }


    void Progress()
    {
        saveData.name.Add(SceneManager.GetActiveScene().name);

        //level name as Json
        string lvl_write = JsonUtility.ToJson(saveData);



        //if no data exists post time
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, lvl_write);
            Debug.Log("Fresh save file: " + saveData.name);
        }
        else
        {

            File.WriteAllText(savePath, lvl_write);
            Debug.Log("Save file updated: " + saveData.name);
        }
    }


    private void CheckFile()
    {

    }
}


[Serializable]
public class SaveData
{
    //singlton
    private static SaveData instance;
    public static SaveData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveData();
            }
            return instance;
        }
    }

    public List<string> name;
    public double t;

}
