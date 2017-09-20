using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredWall : Bit {


	void Awake () {
        bitType = BitType.Wall;
        neighbourDependant = true;
        showColour = false;
    }
    // Use this for initialization
    void Start()
    {
        world = FindObjectOfType<World>();
        spriteSheet = BitWorldSprites.Instantiate;
        GetNeighbours();
        GetBitShapeString();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
