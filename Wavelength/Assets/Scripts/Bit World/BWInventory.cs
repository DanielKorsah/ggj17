using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BWInventory 
{
    private static uint[] pickups = new uint[3] { 0, 0, 0 }; // [pickup.x - 1]

    // Adds a pickup of type to the inventory
    public static void AddPickup(Pickup pickup)
    {
        pickups[(int)pickup - 1]++;
    }

    // Gets the number of pickups of a given type
    public static uint GetPickupCount(Pickup pickup)
    {
        return pickups[(int)pickup - 1];
    }

    // Returns true if a pickup can and was used, else returns false
    public static bool UsePickup(Pickup pickup)
    {
        if(pickups[(int)pickup - 1] == 0)
        {
            return false;
        }
        pickups[(int)pickup - 1]--;
        return true;
    }

    // Zeroes all values
    public static void ResetInventory()
    {
        pickups = new uint[3] { 0, 0, 0 };
    }
}
