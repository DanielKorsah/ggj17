﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBit : Bit
{
    public Transform playerPrefab;
    Transform player;

    void Awake()
    {
        bitType = BitType.Spawn;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        player = FindObjectOfType<BWPlayerController>()?.transform;
        if (player == null)
        {
            player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            player.position = transform.position;
        }
        base.Start();
    }

    // Put the player back at the start
    public override void ResetBit()
    {
        if (player)
        {
            player.position = transform.position;
        }
    }
}
