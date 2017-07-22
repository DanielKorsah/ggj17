using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayScript : MonoBehaviour
{

    private bool player = false;
    private bool press = false;

    private SpriteRenderer statik;

    private float timer = 1.0f;

    // Use this for initialization
    void Start()
    {
        statik = GameObject.FindGameObjectWithTag("Static").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player && (Input.GetKeyDown("e") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            press = true;
        }
        if (press == true)
        {
            statik.enabled = true;
            if (timer <= 0)
            {
                if (gameObject.name.Equals("play"))
                {
                    SceneManager.LoadScene("Hub Scene", LoadSceneMode.Single);
                }
                else if (gameObject.name.Equals("instruct"))
                {
                    SceneManager.LoadScene("Instruct", LoadSceneMode.Single);
                } 
                else if (gameObject.name.Equals("quit"))
                {
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
            }
            else
            {
                timer -= Time.deltaTime;
                //Debug.Log("timer");
            }
            //Debug.Log("work!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = false;
    }
}
