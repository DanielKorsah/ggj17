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
    public void AddBit(Bit bit, int x, int y)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10)
        {
            if (true/*contents[x, y] == null*/)
            {
                contents[x, y] = bit;
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

    public void AddAffector(Wavelength newAffector)
    {
        affectors.Add(newAffector);
        MakeShortList();
        UpdateBits();
    }

    public void RemoveAffector(Wavelength oldAffector)
    {
        affectors.Remove(oldAffector);
        MakeShortList();
        UpdateBits();
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
        shortListEnum = (Wavelength)System.Enum.Parse(typeof(Wavelength), sLE);
    }

    private void UpdateBits()
    {
        foreach (Bit bit in contents)
        {
            bit.UpdatedByGrid(shortList, shortListEnum);
        }
    }
}
