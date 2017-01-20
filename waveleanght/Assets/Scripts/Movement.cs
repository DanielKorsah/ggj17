using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //vertical and horizontal acceleration numbers
    private float horizontal = 0;
    private float vertical = 0;
    //speed of character
    private float speed = 5.0f;
    //time to get to full speed in seconds
    private float acceleration = 0.2f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //move up
		if (Input.GetKey("w"))
        {
            if (Vertical < 0)
            {
                Vertical = 0;
            }
            Vertical += Time.deltaTime / acceleration;
            transform.position += new Vector3(0.0f, speed * Vertical, 0.0f) * Time.deltaTime;
        }
        //move down
        if (Input.GetKey("s"))
        {
            if (Vertical > 0)
            {
                Vertical = 0;
            }
            Vertical -= Time.deltaTime / acceleration;
            transform.position += new Vector3(0.0f, speed * Vertical, 0.0f) * Time.deltaTime;
        }
        //move left
        if (Input.GetKey("a"))
        {
            if (Horizontal > 0)
            {
                Horizontal = 0;
            }
            Horizontal -= Time.deltaTime/acceleration;
            transform.position += new Vector3(speed * horizontal, 0.0f, 0.0f) * Time.deltaTime;
        }
        //move right
        if (Input.GetKey("d"))
        {
            if (Horizontal < 0)
            {
                Horizontal = 0;
            }
            Horizontal += Time.deltaTime/acceleration;
            transform.position += new Vector3(speed * horizontal, 0.0f, 0.0f) * Time.deltaTime;
        }
    }

    private float Horizontal
    {
        get
        {
            return horizontal;
        }
        set
        {
            horizontal = Mathf.Clamp(value, -1, 1);
        }
    }
    private float Vertical
    {
        get
        {
            return vertical;
        }
        set
        {
            vertical = Mathf.Clamp(value, -1, 1);
        }
    }
}
