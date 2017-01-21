using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpProject : MonoBehaviour {



    Inventory inv;
    private GameObject player;
    private bool onit;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inv = player.GetComponent<Inventory>();
        onit = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (onit && Input.GetKeyDown(KeyCode.E))
        {
            inv.AddProjectPickup();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onit = true;
        Debug.Log("sdfsddg");
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        onit = false;
    }
}
