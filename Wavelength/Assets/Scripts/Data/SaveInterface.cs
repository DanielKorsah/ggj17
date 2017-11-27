using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveInterface : MonoBehaviour
{
    public static SaveInterface SI;
    public SaveSystem accessor;
    Data d;
    TimeLimit tLim;

    void Start ()
    {
        accessor = SaveSystem.Instance;
        Debug.Log (accessor.Data.UnlockedLevels.Count);
        accessor.Data.LastPlayed = accessor.Data.UnlockedLevels[accessor.Data.UnlockedLevels.Count - 1];
        d = SI.accessor.Data;

        tLim = (TimeLimit) GameObject.Find ("Timer").GetComponent ("TimeLimit");
        //make a static singleton
        #region
        if (SI == null)
        {
            DontDestroyOnLoad (gameObject);
            SI = this;
        }
        else
        {
            if (SI != this)
            {
                Destroy (gameObject);
            }
        }
        #endregion

    }

    //load most progressed level
    public void LoadMostProgressed ()
    {
        if (accessor.Data.Mode)
        {

            accessor.BRead ();
            Data d = accessor.Data;
            SceneManager.LoadScene (d.LastPlayed);
        }
        else
        {
            accessor.JRead ();
            Data d = accessor.Data;
            SceneManager.LoadScene (d.LastPlayed);
        }
    }

    //load last scene save

    //load a specific level

    //save current level - Just testing file writes for now
    public void SaveCurrentLevel ()
    {

        string thisLevel = SceneManager.GetActiveScene ().name;
        //make this the last level played

        accessor.Data.LastPlayed = thisLevel;
        Debug.Log (thisLevel + "is current level");
        if (!accessor.Data.UnlockedLevels.Contains (thisLevel))
        {
            if (accessor.Data.Mode)
            {
                Debug.Log (thisLevel + " saved to Bson");
                accessor.BRead ();

                //add level to list of unlockled levels
                if (!accessor.Data.UnlockedLevels.Contains (thisLevel))
                {
                    Debug.LogWarning ("enter the BSON if to check if the level is logged already");
                    d.UnlockedLevels.Add (thisLevel);
                    d.BestTimes.Add (float.MaxValue);
                    foreach (string element in d.UnlockedLevels)
                        Debug.Log (element);
                }

                d.LastPlayed = d.UnlockedLevels[d.UnlockedLevels.Count - 1];
                accessor.BWrite ();
            }
            else
            {
                Debug.Log (thisLevel + " saved to Json");
                d = accessor.JRead ();

                //add level to list of unlockled levels
                if (!accessor.Data.UnlockedLevels.Contains (thisLevel))
                {
                    Debug.LogWarning ("enter the BSON if to check if the level is logged already");

                    d.UnlockedLevels.Add (thisLevel);
                    d.BestTimes.Add (float.MaxValue);

                    foreach (string element in d.UnlockedLevels)
                        Debug.Log (element);
                }

                d.LastPlayed = d.UnlockedLevels[d.UnlockedLevels.Count - 1];
                Debug.Log ("LastPlayed = " + d.LastPlayed);
                accessor.JWrite (d);

                accessor.JRead ();
                Debug.Log ("Lastplayed after Save = " + d.LastPlayed);
            }
        }
    }

    //save time
    public void SaveTime (float time)
    {
        Debug.Log ("logged");
        string thisLevel = SceneManager.GetActiveScene ().name;

        if (d.UnlockedLevels.Contains (thisLevel))
        {
            if (accessor.Data.Mode)
            {
                Debug.Log ("Entered Json save method.");
                //bson save
                accessor.BRead ();
                d.BestTimes[d.UnlockedLevels.IndexOf (d.LastPlayed)] = time;
                accessor.BWrite ();
            }
            else
            {
                Debug.Log ("Entered Json save method");
                //json save
                accessor.JRead ();
                d.BestTimes[d.UnlockedLevels.IndexOf (d.LastPlayed)] = time;
                foreach (string element in d.UnlockedLevels)
                    Debug.Log (element);
                accessor.JWrite (d);
            }
        }
        else
        {

        }
    }

    public void JTest ()
    {
        accessor.JWrite (d);
    }

    public void BTest ()
    {
        accessor.BWrite ();
    }
}