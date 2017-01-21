using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour {

    List<GameObject> Walls = new List<GameObject>();
    bool contact;

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

        SendState();
    }
    

    //on mouse click
    private void Update()
    {
        if(contact == true && Input.GetMouseButtonDown(0))
        {
            StateCycle();
            wavelength = state[i];
            Debug.Log("Wavelength is " + wavelength);
            SendState();
        }
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
            wall.GetComponent<WallState>().StateUpdate(state[i], activeLocation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        contact = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        contact = false;
    }

}
