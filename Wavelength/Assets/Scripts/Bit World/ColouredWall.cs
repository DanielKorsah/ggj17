﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredWall : Bit
{
    // True if colour at that index is required to hide wall
    private bool[] wallColours = new bool[] { false, false, false };
    // The display wavelength of this wall
    private Wavelength displayWavelength = Wavelength.I;
    // Collision box
    private BoxCollider2D wallCollider;

    // ~~~ Reference to a piece of shading for this wall
    public SpriteRenderer[] shadows;

    private static BitWorldLibrarian librarian;

    void Awake()
    {
        bitType = BitType.Wall;
        displayType = BitType.Void;
        neighbourDependant = true;
        showColour = false;
        wallCollider = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    override public void Initialise()
    {
        // If nothing else has found the librarian find it
        if(librarian == null)
        {
            librarian = FindObjectOfType<BitWorldLibrarian>();
        }
        base.Initialise();
    }

    // Set Wall BitType
    public override BitType BitTypeSet
    {
        set
        {
            // If these values are matched the coloured wall is freshly made and available for manipulation
            if (displayType == BitType.Void && bitType == BitType.Wall)
            {
                // Set bit and display type
                bitType = value;
                displayType = value;

                // Save all colours present in this wall type as true
                string wallCol = value.ToString();
                if (wallCol.Contains("I"))
                {
                    wallColours[(int)Wavelength.I] = true;
                }
                if (wallCol.Contains("V"))
                {
                    wallColours[(int)Wavelength.V] = true;
                }
                if (wallCol.Contains("U"))
                {
                    wallColours[(int)Wavelength.U] = true;
                }
            }
        }
    }

    // Get appropriate shading for wall shape and change colour
    protected override void UpdateSprite()
    {
        CalculateDisplayColour();
        //string wavel = displayType.ToString();
        //wavel = wavel.Substring(0, wavel.Length - 4);
        //WaveColour wavec = (WaveColour)System.Enum.Parse(typeof(WaveColour), wavel);
        //sprite.color = BitWorldKnowledge.Instance.BitTypeByColour[displayType];
        sprite.sprite = librarian.WallSpitesByDispWavelength[(int)displayWavelength];
        base.UpdateSprite();
        // ~~~ Way to show or hide shadow on the bottom
        string ws = wallShape.ToString();
        for(int i = 0; i < shadows.Length; ++i)
        {

            shadows[i].enabled = !(displayWavelength == Wavelength.None || ws.Contains((i + 1).ToString()));
        }
    }
    // Work out display type from wall type and shortlist (make tiles calculate in future)
    private void CalculateDisplayColour()
    {
        string dispType = "";
        CheckIfDisplaying(Wavelength.I, ref dispType);
        CheckIfDisplaying(Wavelength.V, ref dispType);
        CheckIfDisplaying(Wavelength.U, ref dispType);
        // If the wall exists for any colour
        if (dispType != "")
        {
            // If the wall used to be air, and had changable sprite colour, reset the colour to white
            if(displayType == BitType.Air)
            {
                sprite.color = Color.white;
            }
            displayWavelength = (Wavelength)System.Enum.Parse(typeof(Wavelength), dispType);
            dispType += "Wall";
            displayType = (BitType)System.Enum.Parse(typeof(BitType), dispType);
            wallCollider.enabled = true;
        }
        // If the wall is hidden
        else
        {
            displayWavelength = Wavelength.None;
            displayType = BitType.Air;
            wallCollider.enabled = false;
        }
    }
    // Checks if a colour required to hide the wall is absent, and appends the representative letter
    private void CheckIfDisplaying(Wavelength colour, ref string dispType)
    {
        if (wallColours[(int)colour] && !shortList[(int)colour])
        {
            dispType += colour.ToString();
        }
    }
}
