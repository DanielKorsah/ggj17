using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Bit[,] contents = new Bit[10, 10];

    List<Wavelength> affectors = new List<Wavelength>();
    bool[] shortList = new bool[3];
    Wavelength shortListEnum;

    [SerializeField]
    Vector2 worldPos = new Vector2();

    // Adds a bit to contents
    public Bit AddBit(Bit bit, int x, int y)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10)
        {
            if (contents[x, y] == null)
            {
                contents[x, y] = bit;
                return bit;
            }
            else
            {
                throw new System.Exception("Bit already exists.");
            }
        }
        else
        {
            throw new System.Exception("Bit out of grid bounds.");
        }
    }

    public Bit GetBit(int x, int y)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10)
        {
            return contents[x, y];
        }
        return null;
    }

    // Initialise all bits
    public void InitialiseBits()
    {
        foreach(Bit b in contents)
        {
            b.Initialise();
        }
    }

    // When only adding an affector to the list
    public void AddAffector(Wavelength newAffector)
    {
        affectors.Add(newAffector);
        FinishAffectorUpdate();
    }
    // When only removing an affector from the list
    public void RemoveAffector(Wavelength oldAffector)
    {
        affectors.Remove(oldAffector);
        FinishAffectorUpdate();
    }
    // When rotating a beacons colours, and don't want to waste a call to the finish function
    public void SwapAffector(Wavelength newAffector, Wavelength oldAffector)
    {
        affectors.Remove(oldAffector);
        affectors.Add(newAffector);
        FinishAffectorUpdate();
    }
    // Common processes to end all changes to affector list
    private void FinishAffectorUpdate()
    {
        MakeShortList();
        UpdateBits();
        UpdateBitsShape();
    }

    private void MakeShortList()
    {
        string sLE = "";
        if (affectors.Contains(Wavelength.I))
        {
            shortList[(int)Wavelength.I] = true;
            sLE += "I";
        }
        else
        {
            shortList[(int)Wavelength.I] = false;
        }

        if (affectors.Contains(Wavelength.V))
        {
            shortList[(int)Wavelength.V] = true;
            sLE += "V";
        }
        else
        {
            shortList[(int)Wavelength.V] = false;
        }

        if (affectors.Contains(Wavelength.U))
        {
            shortList[(int)Wavelength.U] = true;
            sLE += "U";
        }
        else
        {
            shortList[(int)Wavelength.U] = false;
        }
        if (sLE == "")
        {
            sLE = "None";
        }
        shortListEnum = (Wavelength)System.Enum.Parse(typeof(Wavelength), sLE);
    }

    // Update constituent bits on the colour of the grid
    private void UpdateBits()
    {
        foreach (Bit bit in contents)
        {
            bit.UpdatedByGrid(shortList, shortListEnum);
        }
    }
    // Update constituent bits' shapes after they've all adapted to new colour
    private void UpdateBitsShape()
    {
        foreach (Bit bit in contents)
        {
            bit.UpdatedByNeighbour();
        }
    }

    // Affectors are reset first when resetting level
    public void ResetGridAffectors()
    {
        affectors.Clear();
        FinishAffectorUpdate();
    }
    // Bits are reset second so that beacons don't have some of their effect cancelled
    public void ResetGridBits()
    {
        foreach (Bit b in contents)
        {
            b.ResetBit();
        }
    }
}
