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
        BitTypeByColourKnowledge();
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
    private Dictionary<Color32, BitType> bitTypeByColour = new Dictionary<Color32, BitType>();

    private void BitTypeByColourKnowledge()
    {
        bitTypeByColour.Add(new Color32(255, 255, 255, 000), BitType.Void);
        bitTypeByColour.Add(new Color32(000, 000, 000, 255), BitType.Air);
        bitTypeByColour.Add(new Color32(127, 127, 127, 255), BitType.Wall);
        bitTypeByColour.Add(new Color32(255, 000, 000, 255), BitType.IWall);
        bitTypeByColour.Add(new Color32(255, 255, 000, 255), BitType.VWall);
        bitTypeByColour.Add(new Color32(255, 000, 255, 255), BitType.UWall);
        bitTypeByColour.Add(new Color32(255, 128, 000, 255), BitType.IVWall);
        bitTypeByColour.Add(new Color32(191, 000, 098, 255), BitType.IUWall);
        bitTypeByColour.Add(new Color32(000, 255, 255, 255), BitType.VUWall);
        bitTypeByColour.Add(new Color32(255, 255, 255, 255), BitType.IVUWall);
        bitTypeByColour.Add(new Color32(000, 000, 255, 255), BitType.BeaconBL);
        bitTypeByColour.Add(new Color32(000, 150, 255, 255), BitType.BeaconBR);
        bitTypeByColour.Add(new Color32(153, 000, 000, 255), BitType.BeaconTL);
        bitTypeByColour.Add(new Color32(000, 135, 204, 255), BitType.BeaconTR);
        bitTypeByColour.Add(new Color32(000, 000, 153, 255), BitType.Upgrade);
        bitTypeByColour.Add(new Color32(000, 255, 000, 255), BitType.Spawn);
        bitTypeByColour.Add(new Color32(000, 102, 000, 255), BitType.Portal);
    }

    public Dictionary<Color32, BitType> BitTypeByColour
    {
        get
        {
            return bitTypeByColour;
        }
    }
}
