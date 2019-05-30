using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : Bit
{
    private int lightLevel = 0;
    void Awake()
    {
        bitType = BitType.Void;
        displayType = BitType.Void;
        neighbourDependant = false;
        showColour = false;
    }

    static BitWorldMaker maker;

    override public void Initialise()
    {
        base.Initialise();
        if (maker == null)
        {
            maker = FindObjectOfType<BitWorldMaker>();
        }
        LightLevel = 0;
        maker.LateStartCall += CheckForWalls;
    }

    private void CheckForWalls()
    {
        foreach (Bit b in neighbours)
        {
            if (b == null)
            {
                continue;
            }
            if (b.DisplayTypeGet == BitType.Wall)
            {
                SetLightLevel(5);
            }
        }
    }

    public override void SetLightLevel(int ll)
    {
        // If this light level is more powerful, replace current light level
        if (ll > lightLevel)
        {
            LightLevel = ll;
            foreach (Bit b in neighbours)
            {
                if (b.DisplayTypeGet == BitType.Void)
                {
                    b.SetLightLevel(ll - 1);
                }
            }
        }
    }

    private int LightLevel
    {
        set
        {
            lightLevel = value;
            if (lightLevel == 0)
            {
                sprite.enabled = false;
            }
            else
            {
                sprite.enabled = true;
                UpdateSprite();
            }
        }
    }

    protected override void UpdateSprite()
    {
        Color newC = Color.white * (lightLevel / 6.0f);
        newC.a = 255;
        sprite.color = newC;
    }
}
