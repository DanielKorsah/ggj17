using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldLibrarian : MonoBehaviour {

    public BitWorldKnowledge knowledge = BitWorldKnowledge.Instance;
    public BitTypetoBool[] neighbourDependant;
    public Sprite[] WallSpitesByDispWavelength = new Sprite[(int)Wavelength.None + 1];
    public Sprite[] BeaconSprites = new Sprite[(int)Pickup.displace + 1];

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
