using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour
{

    private bool contact = false;
    float timer = 1f;

    [SerializeField]
    public string SceneName;

    public bool Contact
    {
        get { return contact; }
        set { contact = value; }
    }


    private void Update()
    {
        if(contact == true)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        contact = true;
    }
}
