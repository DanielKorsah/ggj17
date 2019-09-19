using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWInventory : MonoBehaviour
{
    #region Singleton
    static BWInventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one bit-world inventory.");
        }
        instance = this;
    }

    public static BWInventory Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private uint[] pickups = new uint[3] { 0, 0, 0 }; // [pickup.x - 1]
    public delegate void InventoryUpdate(Pickup pickup);
    public InventoryUpdate InventoryUpdateCall;
    
    // Adds a pickup of type to the inventory
    public void AddPickup(Pickup pickup)
    {
        pickups[(int)pickup - 1]++;
        if (InventoryUpdateCall != null)
        {
            InventoryUpdateCall.Invoke(pickup);
        }
    }

    // Gets the number of pickups of a given type
    public uint GetPickupCount(Pickup pickup)
    {
        return pickups[(int)pickup - 1];
    }

    // Returns true if a pickup can and was used, else returns false
    public bool UsePickup(Pickup pickup)
    {
        if(pickups[(int)pickup - 1] == 0)
        {
            return false;
        }
        pickups[(int)pickup - 1]--;
        InventoryUpdateCall?.Invoke(pickup);
        return true;
    }

    // Zeroes all values
    public void ResetInventory()
    {
        pickups = new uint[3] { 0, 0, 0 };

        if (InventoryUpdateCall != null)
        {
            InventoryUpdateCall(Pickup.line);
            InventoryUpdateCall(Pickup.area);
            InventoryUpdateCall(Pickup.displace);
        }
    }
}
