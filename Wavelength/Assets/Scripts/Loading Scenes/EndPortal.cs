using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour
{

    private bool contact = false;
    bool logged = false;
    float timer = 1f;
    TimeLimit tLim;
    Data d;

    [SerializeField]
    public string NextScene;
    string thisScene;

    public bool Contact
    {
        get { return contact; }
        set { contact = value; }
    }

    public void Start ()
    {
        d = SaveInterface.SI.accessor.Data;
        tLim = (TimeLimit) GameObject.Find ("Timer").GetComponent ("TimeLimit");
        thisScene = SceneManager.GetActiveScene ().name;
        Debug.Log ("this scene = " + thisScene);
        SaveInterface.SI.SaveCurrentLevel ();
    }

    private void Update ()
    {
        if (contact == true)
        {

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene (NextScene, LoadSceneMode.Single);
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {

        //check we're in an actual level
        if (!logged && thisScene != "Main Menu" && thisScene != "Hub Scene" &&
            thisScene != "Instruct" && thisScene != "theEnd")
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.Log ("Save, nigga!");
            }
            d = SaveInterface.SI.accessor.JRead ();
            //run the HighScore method on the persistence manager
            d.BestTimes[d.UnlockedLevels.IndexOf (thisScene)] = tLim.time;
            SaveInterface.SI.SaveTime (tLim.time);

            logged = true;
        }

        contact = true;

    }
}