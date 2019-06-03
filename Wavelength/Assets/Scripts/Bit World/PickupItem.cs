using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    Pickup type = Pickup.none;
    bool playerContact = false;
    BWPlayerController player;

    private void Start()
    {
        InputManager.Instance.PickupCall += TryPickup;
    }

    private bool TryPickup()
    {
        if (playerContact)
        {
            // Add pickup to inventory
            player.GivePickup(type);
            Destroy(gameObject);
        }
        return false;
    }

    public void SetType(Pickup value)
    {
        type = value;
        GetComponent<SpriteRenderer>().sprite = FindObjectOfType<BitWorldLibrarian>().PickupSprites[(int)type - 1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerContact = true;
            player = collision.GetComponent<BWPlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerContact = false;
        }
    }
}
