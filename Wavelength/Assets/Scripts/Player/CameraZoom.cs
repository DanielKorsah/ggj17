using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    bool toggle = false;

    // Update is called once per frame
    void Update ()
    {

        if(Input.GetKeyDown("c"))
        {
            toggle = !toggle;
        }

        if (toggle)
        {
            Camera.main.orthographicSize = 12;
        }
        else
        {
            Camera.main.orthographicSize = 8;
        }
	}
}
