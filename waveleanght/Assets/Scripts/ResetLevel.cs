using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour {

    float downTime;

    private void Start()
    {
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
    }
}
