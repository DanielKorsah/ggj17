using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{

    private int x;
    private int y;

    List<Beacon> beacons = new List<Beacon>();
    List<DisappearingWalls> walls = new List<DisappearingWalls>();
    List<string> affectedBy = new List<string>();
    FloorColour floor;

    // Use this for initialization
    void Start()
    {
        //set this tiles x and y coords based on its name
        string[] coords = name.Split(',');
        int.TryParse(coords[0], out x);
        int.TryParse(coords[1], out y);

        //store all child beacons of this tile in a list and give them x and y coords
        beacons.AddRange(GetComponentsInChildren<Beacon>());

        //store all child walls of this tile in a list and give them x and y coords
        walls.AddRange(GetComponentsInChildren<DisappearingWalls>());
        foreach (DisappearingWalls wall in walls)
        {
            wall.SetCoords(x, y);
        }
        floor = GetComponentInChildren<FloorColour>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time == Time.deltaTime)
        {
            //display walls appropriately on level start 
            foreach (Beacon beacon in beacons)
            {
                beacon.FirstTimeSetUp(x, y);
            }
        }
    }

    //remove a wave type that no longer affects the tile
    public void OldAffector(string waveType)
    {
        affectedBy.Remove(waveType);
    }

    //add a wave type that affects the tile
    public void NewAffector(string waveType)
    {
        affectedBy.Add(waveType);
    }

    //change the walls shown on this tile
    public void ChangeWalls()
    {
        foreach (DisappearingWalls wall in walls)
        {
            //this sends the show wall function true if its type is not found in the list, and false if it is found
            wall.ShowWall(!affectedBy.Contains(wall.WallType));
        }
        floor.ChangeFloorColour(affectedBy);
    }
    
    //gets for x and y so that walls and beacons can set their coords
    public int XCoord
    {
        get
        {
            return x;
        }
    }

    public int YCoord
    {
        get
        {
            return y;
        }
    }
}
