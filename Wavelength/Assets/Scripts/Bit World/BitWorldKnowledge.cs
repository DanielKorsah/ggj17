using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldKnowledge
{
    public BitTypetoBool[] neighbourDependantA;

    // Private instance of class (singleton)
    private static BitWorldKnowledge instance;
    // Constructor (private for singleton)
    private BitWorldKnowledge() { }
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
    private Dictionary<Color, BitType> bitTypeByColour = new Dictionary<Color, BitType>();

    private void BitTypeByColourKnowledge()
    {
        
    }
}
