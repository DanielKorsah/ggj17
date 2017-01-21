using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {



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
    }

    public void SubFocusPickup()
    {
        focusPickup--;
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
    }

    public void SubBoostPickup()
    {
        boostPickup--;
    }
    
}
