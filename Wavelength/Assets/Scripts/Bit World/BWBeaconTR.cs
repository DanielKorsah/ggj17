using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWBeaconTR : Bit
{
    Direction facing = Direction.up;

    void Awake()
    {
        bitType = BitType.BeaconTR;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override public void Initialise()
    {
        base.Initialise();
    }

    public override Direction BeaconGetFacing()
    {
        return facing;
    }

    public override void GiveMulticolourInfo(Color32 pixel)
    {
        if (pixel.Equals(new Color32(0, 135, 204, 255)))
        {
            facing = Direction.up;
        }
        else if (pixel.Equals(new Color32(0, 101, 153, 255)))
        {
            facing = Direction.right;
        }
        else if (pixel.Equals(new Color32(0, 67,102, 255)))
        {
            facing = Direction.down;
        }
        else if (pixel.Equals(new Color32(0, 33, 51, 255)))
        {
            facing = Direction.left;
        }
    }
}
