using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveInterface : MonoBehaviour
{
    public static SaveInterface SI;
    SaveSystem accessor;

    void Awake()
    {
        accessor = SaveSystem.Instance;

        //make a static singleton
        #region
        if (SI == null)
        {
            DontDestroyOnLoad(gameObject);
            SI = this;
        }
        else
        {
            if (SI != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion

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
        //make this the last level played
        string thisLevel = SceneManager.GetActiveScene().name;
        accessor.Data.LastPlayed = thisLevel;

        if (accessor.Data.Mode)
        {
            accessor.BRead();
            //add level to list of unlockled levels
            if (!accessor.Data.UnlockedLevels.Contains(thisLevel))
                accessor.Data.UnlockedLevels.Add(thisLevel);
            accessor.BWrite();
        }
        else
        {
            accessor.JRead();
            //add level to list of unlockled levels
            if (!accessor.Data.UnlockedLevels.Contains(thisLevel))
                accessor.Data.UnlockedLevels.Add(thisLevel);
            accessor.JWrite();
        }
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