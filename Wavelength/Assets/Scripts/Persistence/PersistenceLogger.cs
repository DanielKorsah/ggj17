using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class PersistenceLogger : MonoBehaviour
{
    public string scene;
    LoadFromSave loader;
    SaveData saveData;
    string saveFile;
    string savePath;
    int keyIndex;

    void Awake()
    {

        loader = LoadFromSave.Instance;



        saveFile = "SaveFile.Json";
        savePath = Path.Combine(Application.streamingAssetsPath, saveFile);

        //use data loader script to acess the current data file
        saveData = loader.ReadAllData(savePath);

        scene = SceneManager.GetActiveScene().name;
        //if not a special scene check if this level has been unlocked already
        if (scene != "Main Menu" && scene != "Hub Scene"
            && scene != "Instruct" && scene != "theEnd")
        {
            Progress();
        }

    }


    public void HighScore()
    {
        float timer = GameObject.Find("Timer").GetComponent<TimeLimit>().time;
        if (saveData.Names.Contains(scene))
        {
            keyIndex = saveData.Names.IndexOf(scene);
        }
        else
        {
            saveData.Names.Add(scene);
            keyIndex = saveData.Names.IndexOf(scene);
        }

        Debug.Log("index of " + scene + " is " + keyIndex);

        if (saveData.Times.Count < keyIndex + 1)
        {
            saveData.Times.Add(Math.Round(timer, 3));
        }
        else
        {
            saveData.Times[keyIndex] = Math.Round(timer, 3);
        }

        //serialise to Json
        string saveWrite = JsonUtility.ToJson(saveData);

        //if no data exists post time
        if (!File.Exists(savePath))
        {
            Debug.Log("Dosen't exist");
            File.Create(savePath);
            File.WriteAllText(savePath, saveWrite);

            Debug.Log("First time logged: " + saveData.Times[keyIndex] + " seconds");
        }
        else
        {
            //read current best time data
            SaveData saveRead = new SaveData();
            string dataText = File.ReadAllText(savePath);
            saveRead = JsonUtility.FromJson<SaveData>(dataText);

            //if time is faster overwrite with new time
            if (saveData.Times[keyIndex] < saveRead.Times[keyIndex])
            {
                File.WriteAllText(savePath, saveWrite);
                Debug.Log("New Highscore: " + scene + " in " + saveData.Times + " seconds");
            }
            else
            {
                Debug.Log("Too slow: " + scene + " in " + saveData.Times + " seconds");
            }
        }
    }


    void Progress()
    {


        keyIndex = saveData.Names.IndexOf(scene);
        saveData.Names.Add(scene);

        //level Names as Json
        string lvl_write = JsonUtility.ToJson(saveData);
        //if no data exists post time
        if (!File.Exists(savePath) && !saveData.Names.Contains(scene))
        {
            File.WriteAllText(savePath, lvl_write);
            Debug.Log("Fresh save file: " + saveData.Names[keyIndex]);
        }
        else if (!saveData.Names.Contains(scene))
        {
            File.WriteAllText(savePath, lvl_write);
            Debug.Log("Save file updated: " + saveData.Names[keyIndex]);
        }
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

    SaveData()
    {
        Names = new List<string>();
        Times = new List<double>();
    }
    public List<string> Names;
    public List<double> Times;

}
