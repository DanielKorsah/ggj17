﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Bit
{


    void Awake()
    {
        bitType = BitType.Portal;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        PortalSpriteCheck();
    }

    private void PortalSpriteCheck()
    {
        // Determine if not bottom left portal square
        if (!(neighbours[(int)Direction.up].name.Contains("Portal") && neighbours[(int)Direction.right].name.Contains("Portal")))
        {
            // Find sprite renderer with portal
            SpriteRenderer[] sps = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sp in sps)
            {
                if (sp.gameObject != gameObject)
                {
                    // Hide portal sprite
                    sp.enabled = false;
                    break;
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // ~~~ Level end stuff
        }
    }
}