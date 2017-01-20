using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour {
    [SerializeField]
    List<GameObject> Walls;

    List<string> state = new List<string>(new string[] { "visable", "infrared", "ultraviolet" });
    string wavelength;
    int i = 0;

    // Use this for initialization
    void Start () {
        wavelength = state[i];
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnMouseDown()
    {
        int i = 0;
        wavelength = state[i + 1];
        Debug.Log("Wavelength is " + wavelength);
    }
}
