using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredWall : Bit
{
    // True if colour at that index is required to hide wall
    private bool[] wallColours = new bool[] { false, false, false };
    // The display wavelength of this wall
    private Wavelength displayWavelength = Wavelength.I;
    // Collision box
    private BoxCollider2D collider;

    // ~~~ All the different coloured wall sprites (bad solution, shouldn't be on every bit) 
    public List<Sprite> sprites = new List<Sprite>();

    void Awake()
    {
        bitType = BitType.Wall;
        displayType = BitType.Void;
        neighbourDependant = true;
        showColour = false;
        collider = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
    }

    // Set Wall BitType
    public override BitType BitTypeSet
    {
        set
        {
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
        sprite.sprite = sprites[(int)displayWavelength];
        base.UpdateSprite();
    }
    // Work out display type from wall type and shortlist (make tiles calculate in future)
    private void CalculateDisplayColour()
    {
        string dispType = "";
        CheckIfDisplaying(Wavelength.I, ref dispType);
        CheckIfDisplaying(Wavelength.V, ref dispType);
        CheckIfDisplaying(Wavelength.U, ref dispType);
        // If the wall still exists for any colour
        if (dispType != "")
        {
            displayWavelength = (Wavelength)System.Enum.Parse(typeof(Wavelength), dispType);
            dispType += "Wall";
            displayType = (BitType)System.Enum.Parse(typeof(BitType), dispType);
            collider.enabled = true;
        }
        // If the wall is hidden
        else
        {
            displayType = BitType.Void;
            collider.enabled = false;
        }
    }

    private void CheckIfDisplaying(Wavelength colour, ref string dispType)
    {
        if (wallColours[(int)colour] && !shortList[(int)colour])
        {
            dispType += colour.ToString();
        }
    }
}
