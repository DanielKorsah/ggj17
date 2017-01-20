using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFocus : MonoBehaviour {

    Inventory inv;
    private GameObject player;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);
        inv = player.GetComponent<Inventory>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inv.AddFocusPickup();
            gameObject.SetActive(false);
        }
    }
}
