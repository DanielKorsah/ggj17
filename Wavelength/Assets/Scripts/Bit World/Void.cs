using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : Bit
{


    void Awake()
    {
        bitType = BitType.Void;
        displayType = BitType.Void;
        neighbourDependant = false;
        showColour = false;
    }

    private void Start()
    {
        world = FindObjectOfType<World>();
        spriteSheet = BitWorldSprites.Instantiate;
        GetNeighbours();
        GetBitShapeString();
    }
}
