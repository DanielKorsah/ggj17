using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [SerializeField]
    private Image glowUni;
    [SerializeField]
    private Image glowOmni;
    [SerializeField]
    private Image glowJump;

    private int focusPickup;
    private int boostPickup;

	// Use this for initialization
	void Start ()
    {
        focusPickup = 0;
        boostPickup = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public int FocusPickup
    {
        get
        {
            return focusPickup;
        }
    }


    public void AddFocusPickup()
    {
        focusPickup++;
        Debug.Log(focusPickup + "add");
        displayFocus();
    }

    public void SubFocusPickup()
    {
        focusPickup--;
        Debug.Log(focusPickup + "sub");
        displayFocus();
    }


    public int BoostPickup
    {
        get
        {
            return boostPickup;
        }
    }


    public void AddBoostPickup()
    {
        boostPickup++;
        displayBoost();
    }

    public void SubBoostPickup()
    {
        boostPickup--;
        displayBoost();
    }
    
    private void displayBoost()
    {
        if (boostPickup > 0)
        {
            glowOmni.enabled = true;
        }
        else
        {
            glowOmni.enabled = false;
        }
    }

    private void displayFocus()
    {
        if (focusPickup > 0)
        {
            glowUni.enabled = true;
        }
        else
        {
            glowUni.enabled = false;
        }
    }
}
