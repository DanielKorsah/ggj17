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

    Animator anim;
    private string[] animatorParameter = new string[] { "UpState", "RightState", "DownState", "LeftState" };

    private void Start()
    {
        // Get game objects in children
        Transform[] transforms = GetComponentsInChildren<Transform>();
        // Find direction and pickup
        foreach (Transform t in transforms)
        {
            if (t.name == "Pickup")
            {
                // Set pickup
                pickupObj = t;
                break;
            }
        }
        // Get animator
        anim = GetComponentInChildren<Animator>();
        // Disable direction and pickup
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
        if (choice != ChoosingInfo.none)
        {
            anim.SetBool("ShowWheel", true);
            anim.SetBool("PickupSprites", choice == ChoosingInfo.pickup);
            for (int i = 0; i < animatorParameter.Length; ++i)
            {
                anim.SetInteger(animatorParameter[i], 0);
            }
        }
        else
        {
            HideChoice();
        }
    }

    public void MakeChoice(Direction dir, bool successful)
    {
        //Animate direction
        int val = 0;
        switch (successful)
        {
            case true:
                val = 1;
                break;
            case false:
                val = -1;
                break;
            default:
                val = 0;
                break;
        }
        anim.SetInteger(animatorParameter[(int)dir], val);

        // Hide sprites if successful choice
        if (successful)
        {
            HideChoice();
        }
    }

    public void ResetParameter(Direction dir)
    {
        anim.SetInteger(animatorParameter[(int)dir], 0);
    }

    public void HideChoice()
    {
        anim.SetBool("ShowWheel", false);
        anim.SetTrigger("HideWheel");
    }
}
