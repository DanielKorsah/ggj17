using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour {

    Inventory inv;
    List<GameObject> Walls = new List<GameObject>();
    bool contact;

    int bonusType = 0; //0 - none, 1, - focus, 2 - boost

    [SerializeField]
    public Vector2 activeLocation;

    List<string> state = new List<string>(new string[] { "visible", "infrared", "ultraviolet" });
    string wavelength;
    [SerializeField]
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
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }
    

    //on mouse click
    private void Update()
    {
        if (contact == true && Input.GetKeyDown(KeyCode.Space))
        {
            StateCycle();
            wavelength = state[i];
            Debug.Log("Wavelength is " + wavelength);
            

            int prev = i - 1;                                                                   //HACKY BULLSHIT BEWARE WHEN ADDING MRE STATES
            if (prev < 0)
                prev = 2;

            foreach (GameObject wall in Walls)
            {
                if (wall.GetComponent<WallState>().gridLocation == activeLocation)
                    wall.GetComponent<WallState>().StateCull(state[prev]);
            }

            SendState();
        }


        //its fucked and we're too tired to debug it
        if (contact == true && Input.GetKeyDown(KeyCode.Alpha1) && inv.FocusPickup > 0)
        {
            //inv.SubFocusPickup();
            //bonusType = 1;
            
            //int prev = i - 1;                                                                   //HACKY BULLSHIT BEWARE WHEN ADDING MRE STATES
            //if (prev < 0)
            //    prev = 2;

            //foreach (GameObject wall in Walls)
            //{
            //    if (wall.GetComponent<WallState>().gridLocation == activeLocation)
            //        wall.GetComponent<WallState>().StateCull(state[prev]);
            //}

            //SendState();

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
            if (wall.GetComponent<WallState>().gridLocation == activeLocation)
                wall.GetComponent<WallState>().StateUpdate(state[i]);
            Debug.Log(state[i]);
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
