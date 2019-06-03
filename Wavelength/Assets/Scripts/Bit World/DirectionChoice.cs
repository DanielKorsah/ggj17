using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChoice : MonoBehaviour
{
    #region singleton
    private static DirectionChoice instance;
    public static DirectionChoice Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    Transform pickupObj;
    Transform directionObj;

    private void Start()
    {
        // Get game objects in children
        Transform[] transforms =  GetComponentsInChildren<Transform>();
        // Find direction and pickup
        foreach(Transform t in transforms)
        {
            if (t.name == "Direction")
            {
                // Set direction
                directionObj = t;
                // If pickup is already set, break
                if(pickupObj != null)
                {
                    break;
                }
            }
            else if(t.name == "Pickup")
            {
                // Set pickup
                pickupObj = t;
                // If direction is already set, break
                if (directionObj != null)
                {
                    break;
                }
            }
        }
        // Disable direction and pickup
        pickupObj.gameObject.SetActive(false);
        directionObj.gameObject.SetActive(false);
    }

    public void BeginChoice(ChoosingInfo choice, Vector3 pos)
    {
        transform.position = pos;
        // Display correct sprite
        SetActive(choice);
    }

    // Set the right sprites to display
    private void SetActive(ChoosingInfo choice)
    {
        if(choice == ChoosingInfo.direction)
        {
            directionObj.gameObject.SetActive(true);
        }
        else if(choice == ChoosingInfo.pickup)
        {
            pickupObj.gameObject.SetActive(true);
        }
        else
        {
            HideChoice();
        }
    }

    public void MakeChoice(Direction dir, bool successful)
    {
        // ~~~ Animate direction

        // Hide sprites if successful choice
        if (successful)
        {
            HideChoice();
        }
    }

    public void HideChoice()
    {
        // Disable direction and pickup
        pickupObj.gameObject.SetActive(false);
        directionObj.gameObject.SetActive(false);
    }
}
