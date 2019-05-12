using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBit : Bit
{
    Transform player;
    // Start is called before the first frame update
    override protected void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p)
        {
            player = p.transform;
            player.position = transform.position;
        }
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // If level is restarted
    public void Reset()
    {
        if (player)
        {
            player.position = transform.position;
        }
    }
}
