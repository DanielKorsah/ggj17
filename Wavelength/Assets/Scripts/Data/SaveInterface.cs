using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveInterface : MonoBehaviour
{
    public static SaveInterface SI;
    public SaveSystem accessor;
    Data d;
    void Start()
    {
        accessor = SaveSystem.Instance;
        Debug.Log(accessor.Data.UnlockedLevels.Count);
        accessor.Data.LastPlayed = accessor.Data.UnlockedLevels[accessor.Data.UnlockedLevels.Count - 1];
        d = accessor.Data;
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
        else
        {
            accessor.JRead();
            Data d = accessor.Data;
            SceneManager.LoadScene(d.LastPlayed);
        }

    }

    //load last scene save

    //load a specific level

    //save current level - Just testing file writes for now
    public void SaveCurrentLevel()
    {

        string thisLevel = SceneManager.GetActiveScene().name;
        //make this the last level played

        accessor.Data.LastPlayed = thisLevel;
        Debug.Log(thisLevel + "is current level");
        if (!accessor.Data.UnlockedLevels.Contains(thisLevel))
        {
            if (accessor.Data.Mode)
            {
                Debug.Log(thisLevel + " saved to Bson");
                accessor.BRead();
                d.LastPlayed = d.UnlockedLevels[d.UnlockedLevels.Count - 1];

                //add level to list of unlockled levels
                if (!accessor.Data.UnlockedLevels.Contains(thisLevel))
                {
                    accessor.Data.UnlockedLevels.Add(thisLevel);
                    d.BestTimes.Add(float.MaxValue);
                }

                accessor.BWrite();
            }
            else
            {
                Debug.Log(thisLevel + " saved to Json");
                accessor.JRead();
                d.LastPlayed = d.UnlockedLevels[d.UnlockedLevels.Count - 1];
                d.BestTimes.Add(float.MaxValue);
                //add level to list of unlockled levels
                if (!accessor.Data.UnlockedLevels.Contains(thisLevel))
                    accessor.Data.UnlockedLevels.Add(thisLevel);
                accessor.JWrite();
            }
        }
    }

    //save time
    public void SaveTime(float time)
    {
        string thisLevel = SceneManager.GetActiveScene().name;

        if (!accessor.Data.UnlockedLevels.Contains(thisLevel))
        {
            if (accessor.Data.Mode)
            {
                //bson save
                accessor.BRead();
                d.BestTimes[d.UnlockedLevels.IndexOf(d.LastPlayed)] = time;
                accessor.BWrite();
            }
            else
            {
                //json save
                accessor.JRead();
                d.BestTimes[d.UnlockedLevels.IndexOf(d.LastPlayed)] = time;
                accessor.JWrite();
            }
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