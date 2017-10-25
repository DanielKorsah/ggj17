using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    GameObject portal;
    bool contact;
    bool trigger;
    // Use this for initialization
    void Start()
    {

        portal = GameObject.Find("EndPortal");
        if (portal != null)
        {
            contact = portal.GetComponent<EndPortal>().Contact;
        }
        //trigger = portal.GetComponent<EndPortal>().Contact;
    }

    // Update is called once per frame
    void Update()
    {
        if (contact) //For some reason trigger doesn't update whe I try to avoid GetComponent
        {
            StaticRender();
        }

    }

    public void StaticRender()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<AudioSource>().enabled = true;
    }
}
