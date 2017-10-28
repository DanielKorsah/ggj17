using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class LoadFromSave
{

    private bool contact = false;
    bool logged = false;
    float timer = 1f;
    string savePath;


    public bool Contact
    {
        get { return contact; }
        set { contact = value; }
    }

    //make dis bitch a singleton
    private static LoadFromSave instance;
    public static LoadFromSave Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoadFromSave();
            }
            return instance;
        }
    }


    LoadFromSave()
    {
        savePath = Application.streamingAssetsPath + "/SaveFile.Json";
    }

    public void LoadLastSave()
    {
        SceneManager.LoadScene(ReadLastSave());
    }


    public string ReadLastSave()
    {
        string nextScene;
        if (File.Exists(savePath))
        {
            SaveData data = ReadAllData(savePath);
            nextScene = data.Names[data.Names.Count - 1];
        }
        else
        {
            nextScene = "tut1";
        }
        return nextScene;
    }

    public SaveData ReadAllData(string dataPath)
    {
        if (File.Exists(dataPath))
        {
            SaveData data = SaveData.Instance;
            string lvl_as_text = File.ReadAllText(dataPath);
            data = JsonUtility.FromJson<SaveData>(lvl_as_text);
            return data;
        }
        else
        {
            SaveData defaultData = SaveData.Instance;
            //defaultData.Names = new List<string>()
            //default
            return defaultData;
        }
    }

}