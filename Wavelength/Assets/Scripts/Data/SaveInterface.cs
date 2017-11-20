using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveInterface : MonoBehaviour
{

    SaveSystem accessor;

    void Start()
    {
        accessor = SaveSystem.Instance;
    }

    //load most progressed level
    public void LoadMostProgressed()
    {
        if (accessor.Data.Mode)
        {
            accessor.BRead();
            Data d = accessor.Data;
            SceneManager.LoadScene(d.LastPlayed);
        }

    }

    //load last scene save

    //load a specific level

    //save current level - Just testing file writes for now
    public void SaveCurrentLevel()
    {
        accessor.JWrite();
        accessor.BWrite();
    }

    public void JTest()
    {
        accessor.JWrite();
    }

    public void BTest()
    {
        accessor.BWrite();
    }
}