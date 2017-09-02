using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldLibrarian : MonoBehaviour {

    public BitWorldKnowledge knowledge = BitWorldKnowledge.Instance;
    public BitTypetoBool[] neighbourDependant;

    // Use this for initialization
    void Start () {
        knowledge = BitWorldKnowledge.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
