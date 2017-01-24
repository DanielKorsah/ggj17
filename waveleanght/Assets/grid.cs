using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{

    private int x;
    private int y;

    List<DisappearingWalls> walls = new List<DisappearingWalls>();
    List<string> affectedBy = new List<string>();

    // Use this for initialization
    void Start()
    {
        //set this tiles x and y coords based on its name
        string[] coords = name.Split(',');
        int.TryParse(coords[0], out x);
        int.TryParse(coords[1], out y);
        
        //store all child walls of this tile in a list and give them x and y coords
        walls.AddRange(GetComponentsInChildren<DisappearingWalls>());
        foreach (DisappearingWalls wall in walls)
        {
            wall.SetCoords(x, y);
        }

        affectedBy.Add("V");
        affectedBy.Add("IR");
        affectedBy.Add("UV");
        //display walls appropriately on level start up
        ChangeWalls();
    }

    // Update is called once per frame
    void Update()
    {
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
        foreach(DisappearingWalls wall in walls)
        {
            if (affectedBy.Contains(wall.WallType))
            {
                wall.ShowWall(false);
            }
            else
            {
                wall.ShowWall(true);
            }
        }

    }
}
