using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sectors : MonoBehaviour {


    [SerializeField]
    Vector2 v = new Vector2();

    public List<string> freqs = new List<string>();

    private WallState wall;

    // Use this for initialization
    void Start ()
    {
		foreach (GameObject w in GameObject.FindGameObjectsWithTag("Wall"))
        {
            if (w.GetComponent<WallState>().gridLocation == v)
            {
                wall = w.GetComponent<WallState>();
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        freqs = wall.freqs;
	}

    



}
