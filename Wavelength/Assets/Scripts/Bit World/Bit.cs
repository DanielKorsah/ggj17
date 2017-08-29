using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour {

    bool[] shortList = new bool[3];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReceiveShortList(bool[] shortList)
    {
        this.shortList = shortList;
    }
}
