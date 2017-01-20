using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour {

    List<GameObject> Walls = new List<GameObject>();

    [SerializeField]
    public Vector2 activeLocation;

    List<string> state = new List<string>(new string[] { "visible", "infrared", "ultraviolet" });
    string wavelength;
    int i = 0;

    // Use this for initialization
    void Awake() {
        wavelength = state[i];
        foreach(GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Walls.Add(wall);
            Debug.Log("Wall Added: " + wall);
        }
        Debug.Log("object " + GameObject.FindGameObjectsWithTag("Wall"));
    }
    

    //on mouse click
    private void OnMouseDown()
    {
        StateCycle();
        wavelength = state[i];
        Debug.Log("Wavelength is " + wavelength);
        SendState();
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

    void SendState()
    {
        foreach(GameObject wall in Walls)
        {
            wall.GetComponent<WallState>().StateUpdate(state[i]);
        }
    }


}
