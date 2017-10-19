﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private Image glowUni;
    private Image glowOmni;
    private Image glowJump;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip powerupSound;
    GameObject manager;

    private int focusPickup;
    private int boostPickup;
    private int projectPickup;

    // Use this for initialization
    void Start()
    {
        focusPickup = 0;
        boostPickup = 0;
        projectPickup = 0;

        manager = GameObject.Find("Audio Manager");
        //        source = manager.GetComponents<AudioSource>()[1];
        //        source.clip = powerupSound;

        glowUni = GameObject.Find("GlowingUni").GetComponent<Image>();
        glowOmni = GameObject.Find("GlowingOmni").GetComponent<Image>();
        glowJump = GameObject.Find("GlowingJump").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Focus-/---------------/
    //get
    public int FocusPickup
    {
        get
        {
            return focusPickup;
        }
    }

    // ++
    public void AddFocusPickup()
    {
        focusPickup++;
        displayFocus();
        //source.Play();
    }
    // --
    public void SubFocusPickup()
    {
        focusPickup--;
        displayFocus();
    }

    // display
    private void displayFocus()
    {
        if (focusPickup > 0)
        {
            glowUni.enabled = true;
        }
        else
        {
            glowUni.enabled = false;
        }
    }

    //Boost-/---------------/
    //get
    public int BoostPickup
    {
        get
        {
            return boostPickup;
        }
    }

    // ++
    public void AddBoostPickup()
    {
        boostPickup++;
        displayBoost();
        //source.Play();
    }
    // --
    public void SubBoostPickup()
    {
        boostPickup--;
        displayBoost();
    }

    // display
    private void displayBoost()
    {
        if (boostPickup > 0)
        {
            glowOmni.enabled = true;
        }
        else
        {
            glowOmni.enabled = false;
        }
    }

    //Project-/------------------/
    //get
    public int ProjectPickup
    {
        get
        {
            return projectPickup;
        }
    }

    // ++
    public void AddProjectPickup()
    {
        projectPickup++;
        displayProject();
        //source.Play();
    }
    // --
    public void SubProjectPickup()
    {
        projectPickup--;
        displayProject();
    }

    // display
    private void displayProject()
    {
        if (projectPickup > 0)
        {
            glowJump.enabled = true;
        }
        else
        {
            glowJump.enabled = false;
        }
    }
}
