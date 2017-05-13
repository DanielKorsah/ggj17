using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    GameObject portal;
    GameObject portal2;
    GameObject portal3;
    bool trigger;
	// Use this for initialization
	void Start ()
    {

        portal = GameObject.Find("EndPortal");

        if (SceneManager.GetActiveScene().name == "Hub Scene")
        {
            portal2 = GameObject.Find("EndPortal (1)");
            portal3 = GameObject.Find("EndPortal (2)");
        }
        //trigger = portal.GetComponent<EndPortal>().Contact;
    }
	
	// Update is called once per frame
	void Update ()
    {
        


        if (SceneManager.GetActiveScene().name == "Hub Scene")
        {
            if (portal.GetComponent<EndPortal>().Contact || portal2.GetComponent<EndPortal>().Contact || portal3.GetComponent<LoadFromSave>().Contact) //For some reason trigger doesn't update whe I try to avoid GetComponent
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<AudioSource>().enabled = true;
            }
        }
        else
        {
            if (portal.GetComponent<EndPortal>().Contact) //For some reason trigger doesn't update whe I try to avoid GetComponent
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<AudioSource>().enabled = true;
            }
        }

    }
}
