using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour {

    float downTime;
    private SpriteRenderer statik;

    private void Start()
    {
        statik = GameObject.FindGameObjectWithTag("Static").GetComponent<SpriteRenderer>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.R))
        {
            downTime = Time.time;
        }

        if (Input.GetKey(KeyCode.R))
        {
            if ((Time.time - downTime) >=2)
            {
                string levelName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(levelName, LoadSceneMode.Single);
                Debug.Log(levelName + " : " );
            }
        }

        //quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            downTime = Time.time;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            if ((Time.time - downTime) >= 1)
            {
                statik.enabled = true;
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            downTime = Time.time;
        }

        if (Input.GetKey(KeyCode.M))
        {
            if ((Time.time - downTime) >= 1)
            {
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            }
        }

    }
}
