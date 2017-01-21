using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallState : MonoBehaviour {

    [SerializeField]
    private string wallType;

    [SerializeField]
    public Vector2 gridLocation;

    private bool visible;

    private void Start()
    {
        
    }
    
	
	// Update is called once per frame
	public void StateUpdate(string transmitterState)
    {
        if(transmitterState == wallType)
        {
            visible = !true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            visible = !false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    
}
