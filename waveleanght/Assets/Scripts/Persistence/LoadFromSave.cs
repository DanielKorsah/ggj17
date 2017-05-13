using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour
{

    private bool contact = false;
    bool logged = false;
    float timer = 1f;


    [SerializeField]
    public string NextScene;
    string thisScene = SceneManager.GetActiveScene().name;


    public bool Contact
    {
        get { return contact; }
        set { contact = value; }
    }


    private void Update()
    {
        if (contact == true)
        {

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        contact = true;

        //check we're in an actual level
        if (!logged && thisScene != "Main Menu" && thisScene != "Hub Scene"
            && thisScene != "Instruct" && thisScene != "theEnd")
        {

            //run the HighScore method on the persistence manager
            gameObject.GetComponent<PersistenceLogger>().HighScore();
            logged = true;
        }
