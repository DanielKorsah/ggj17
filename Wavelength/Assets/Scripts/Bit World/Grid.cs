using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Bit[,] contents = new Bit[10, 10];

    List<Wavelength> affectors = new List<Wavelength>();
    bool[] shortList = new bool[3];

    [SerializeField]
    int[] worldPos = new int[2];



    public void AddAffector(Wavelength newAffector)
    {
        affectors.Add(newAffector);
        MakeShortList();
    }

    public void RemoveAffector(Wavelength oldAffector)
    {
        affectors.Remove(oldAffector);
        MakeShortList();
    }

    private void MakeShortList()
    {
        if (affectors.Contains(Wavelength.Infrared)){
            shortList[(int)Wavelength.Infrared] = true;
        }
        else
        {
            shortList[(int)Wavelength.Infrared] = false;
        }

        if (affectors.Contains(Wavelength.Visible))
        {
            shortList[(int)Wavelength.Visible] = true;
        }
        else
        {
            shortList[(int)Wavelength.Visible] = false;
        }

        if (affectors.Contains(Wavelength.Ultraviolet))
        {
            shortList[(int)Wavelength.Ultraviolet] = true;
        }
        else
        {
            shortList[(int)Wavelength.Ultraviolet] = false;
        }
    }

    private void UpdateBits()
    {
        foreach (Bit bit in contents)
        {
            bit.ReceiveShortList(shortList);
        }
    }
}
