using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidWall : Bit
{
    

    private void Awake()
    {
        bitType = BitType.Wall;
        displayType = BitType.Wall;
        neighbourDependant = true;
        showColour = false;
    }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        world = FindObjectOfType<World>();
        spriteSheet = BitWorldSprites.Instantiate;
        GetNeighbours();
        GetBitShapeString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
