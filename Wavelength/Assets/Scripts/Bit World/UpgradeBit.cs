using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBit : Bit
{
    Pickup pickup = Pickup.none;

    public Transform pickupPrefab;

    private Transform pickupObject;

    void Awake()
    {
        bitType = BitType.Upgrade;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override public void Initialise()
    {
        base.Initialise();
    }

    public override void ResetBit()
    {
        // Spawn pickup
        SpawnPickup();
    }

    public override void GiveMulticolourInfo(Color32 pixel)
    {
        if (pixel.Equals(new Color32(0, 0, 153, 255)))
        {
            pickup = Pickup.line;
        }
        else if (pixel.Equals(new Color32(0, 0, 102, 255)))
        {
            pickup = Pickup.area;
        }
        else if (pixel.Equals(new Color32(0, 0, 51, 255)))
        {
            pickup = Pickup.displace;
        }

        // Spawn pickup
        SpawnPickup();
    }

    private void SpawnPickup()
    {
        if (pickupObject == null)
        {
            pickupObject = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            pickupObject.GetComponent<PickupItem>().SetType(pickup);
        }
    }
}
