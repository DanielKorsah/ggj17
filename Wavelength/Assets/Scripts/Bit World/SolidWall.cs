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
    override public void Initialise()
    {
        base.Initialise();
    }

    
}
