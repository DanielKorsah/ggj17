using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWBeaconBR : Bit
{
    Pickup upgrade = Pickup.none;

    void Awake()
    {
        bitType = BitType.BeaconBR;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    public override Pickup BeaconGetPickup()
    {
        return upgrade;
    }

    public override void GiveMulticolourInfo(Color32 pixel)
    {
        if (pixel.Equals(new Color32(0, 150, 255, 255)))
        {
            upgrade = Pickup.line;
        }
        else if (pixel.Equals(new Color32(0, 100, 255, 255)))
        {
            upgrade = Pickup.area;
        }
        else if (pixel.Equals(new Color32(0, 50, 255, 255)))
        {
            upgrade = Pickup.displace;
        }
    }
}
