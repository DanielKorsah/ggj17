using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    GameObject portal;
    bool trigger;
	// Use this for initialization
	void Start ()
    {
        portal = GameObject.Find("EndPortal");
        //trigger = portal.GetComponent<EndPortal>().Contact;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (portal.GetComponent<EndPortal>().Contact )//trigger == true)                                For some reason trigger doesn't update whe I try to avoid GetComponent
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<AudioSource>().enabled = true;

        }
		
	}
}
