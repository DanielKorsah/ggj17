using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class LoadFromSave : MonoBehaviour
{

    private bool contact = false;
    bool logged = false;
    float timer = 1f;
    string save_path;

    
    public string nextScene;

    public bool Contact
    {
        get { return contact; }
        set { contact = value; }
    }


    void Start()
    {
        save_path = Application.streamingAssetsPath + "/SaveFile.Json";
    }
    private void Update()
    {
        if (contact == true)
        {

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if(File.Exists(save_path))
        {
            ReadSave();
        }
        else
        {
            nextScene = "tut2";
            Debug.Log(save_path);
        }


        contact = true;
    }


    string ReadSave()
    {
        Level level = new Level();
        string lvl_as_text = File.ReadAllText(save_path);
        level = JsonUtility.FromJson<Level>(lvl_as_text);

        nextScene = level.name;
        return nextScene;
    }

}