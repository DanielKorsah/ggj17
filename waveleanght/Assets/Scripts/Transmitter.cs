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
    void Update() {
    }

    //on mouse click
    private void OnMouseDown()
    {
        StateCycle();
        wavelength = state[i];
        Debug.Log("Wavelength is " + wavelength);
    }

    //increases the index of state to be applied and loops at the max
    private void StateCycle()
    {
        ++i;
        if (i == state.Count)
        {
            i = 0;
        }
    }
}
