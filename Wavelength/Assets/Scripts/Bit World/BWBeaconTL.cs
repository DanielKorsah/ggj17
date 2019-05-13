using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWBeaconTL : Bit
{
    Wavelength startColour = Wavelength.None;

    void Awake()
    {
        bitType = BitType.BeaconTL;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    public override Wavelength BeaconGetWaveLength()
    {
        return startColour;
    }

    public override void GiveMulticolourInfo(Color32 pixel)
    {
        if (pixel.Equals(new Color32(153, 0, 0, 255)))
        {
            startColour = Wavelength.I;
        }
        else if (pixel.Equals(new Color32(153, 153, 0, 255)))
        {
            startColour = Wavelength.V;
        }
        else if (pixel.Equals(new Color32(153, 0, 153, 255)))
        {
            startColour = Wavelength.U;
        }
    }
}
