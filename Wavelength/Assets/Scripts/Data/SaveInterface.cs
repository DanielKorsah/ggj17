using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveInterface : MonoBehaviour
{
    bool defaultMode;

    SaveSystem accessor;

    void Start()
    {
        accessor = SaveSystem.Instance;
    }

    //load most progressed level
    void loadMostProgressed()
    {
        if (defaultMode)
        {
            accessor.BRead();
            Data d = accessor.Data;
            SceneManager.LoadScene(d.LastPlayed);
        }

    }

    //load last scene save

    //load a specific level

    //save current level
}