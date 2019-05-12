using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldKnowledge
{
    public BitTypetoBool[] neighbourDependantA;

    // Private instance of class (singleton)
    private static BitWorldKnowledge instance;
    // Constructor (private for singleton)
    private BitWorldKnowledge()
    {
        //BitTypeByColourKnowledge();
    }
    // Singleton instantiaion method
    public static BitWorldKnowledge Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BitWorldKnowledge();
            }
            return instance;
        }
    }

    private Dictionary<BitType, bool> neighbourDependant = new Dictionary<BitType, bool>();
    private Dictionary<Wavelength, BitType> wallColour = new Dictionary<Wavelength, BitType>();
    private Dictionary<Wavelength, Color32> airColourByWavelength = new Dictionary<Wavelength, Color32>() {
        { Wavelength.I, new Color32(255, 000, 000, 255) },
        { Wavelength.V, new Color32(255, 255, 000, 255) },
        { Wavelength.U, new Color32(255, 000, 255, 255) },
        { Wavelength.IV, new Color32(255, 128, 000, 255) },
        { Wavelength.IU, new Color32(191, 000, 098, 255) },
        { Wavelength.VU, new Color32(000, 255, 255, 255) },
        { Wavelength.IVU, new Color32(255, 255, 255, 255) },
        { Wavelength.None, new Color32(0, 0, 0, 255) },
    };
    private Dictionary<BitType, Color32> bitTypeByColour = new Dictionary<BitType, Color32>
    {
        { BitType.Void, new Color32(255, 255, 255, 000) },
        { BitType.Air, new Color32(000, 000, 000, 255) },
        { BitType.Wall, new Color32(127, 127, 127, 255) },
        { BitType.IWall, new Color32(255, 000, 000, 255) },
        { BitType.VWall, new Color32(255, 255, 000, 255) },
        { BitType.UWall, new Color32(255, 000, 255, 255) },
        { BitType.IVWall, new Color32(255, 128, 000, 255) },
        { BitType.IUWall, new Color32(191, 000, 098, 255) },
        { BitType.VUWall, new Color32(000, 255, 255, 255) },
        { BitType.IVUWall, new Color32(255, 255, 255, 255) },
        { BitType.BeaconBL, new Color32(000, 000, 255, 255) },
        { BitType.BeaconBR, new Color32(000, 150, 255, 255) },
        { BitType.BeaconTL, new Color32(153, 000, 000, 255) },
        { BitType.BeaconTR, new Color32(000, 135, 204, 255) },
        { BitType.Upgrade, new Color32(000, 000, 153, 255) },
        { BitType.Spawn, new Color32(000, 255, 000, 255) },
        { BitType.Portal, new Color32(000, 102, 000, 255) }
    };

    public Dictionary<Wavelength, Color32> AirColourByWavelength
    {
        get
        {
            return airColourByWavelength;
        }
    }
    public Dictionary<BitType, Color32> BitTypeByColour
    {
        get
        {
            return bitTypeByColour;
        }
    }
}
