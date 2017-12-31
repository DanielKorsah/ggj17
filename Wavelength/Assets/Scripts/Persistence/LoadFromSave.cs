﻿using System.Collections;
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
    string save_path;


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
        save_path = Application.streamingAssetsPath + "/SaveFile.Json";
    }

    public void LoadLastSave()
    {
        SceneManager.LoadScene(ReadSave());
    }


    public string ReadSave()
    {
        string nextScene;
        if (File.Exists(save_path))
        {
            Level level = new Level();
            string lvl_as_text = File.ReadAllText(save_path);
            level = JsonUtility.FromJson<Level>(lvl_as_text);

            nextScene = level.name;
        }
        else
        {
            nextScene = "tut1";
        }
        return nextScene;
    }

}