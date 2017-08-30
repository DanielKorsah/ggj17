using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour
{
    World world;

    BitType bitType;
    
    bool[] shortList = new bool[3];

    Vector2 gridPos = new Vector2();
    Vector2 worldPos = new Vector2();

    Bit[] neighbours = new Bit[4];
    WallShape wallShape;

    bool showColour;

    private void Start()
    {

        world = FindObjectOfType<World>();
        GetNeighbours();
        GetWallType();
    }

    private void GetNeighbours()
    {
        // Find adjacent up in adjacent grid
        if (gridPos.y == 9)
        {
            try
            {
                neighbours[(int)Direction.up] = world.grids[(int)worldPos.x, (int)worldPos.y + 1].contents[(int)gridPos.x, 0];
            }
            catch
            {
                Debug.Log("Grid " + (int)worldPos.x + ", " + ((int)worldPos.y + 1) + "does not exist.");
            }
        }
        // Find adjacent up in same grid
        else
        {
            neighbours[(int)Direction.up] = world.grids[(int)worldPos.x, (int)worldPos.y].contents[(int)gridPos.x, (int)gridPos.y + 1];
        }

        // Find adjacent right in adjacent grid
        if (gridPos.x == 9)
        {
            try
            {
                neighbours[(int)Direction.right] = world.grids[(int)worldPos.x + 1, (int)worldPos.y].contents[0, (int)gridPos.y];
            }
            catch
            {
                Debug.Log("Grid " + ((int)worldPos.x + 1) + ", " + (int)worldPos.y + "does not exist.");
            }
        }
        // Find adjacent right in same grid
        else
        {
            neighbours[(int)Direction.right] = world.grids[(int)worldPos.x, (int)worldPos.y].contents[(int)gridPos.x + 1, (int)gridPos.y];
        }

        // Find adjacent down in adjacent grid
        if (gridPos.y == 0)
        {
            try
            {
                neighbours[(int)Direction.down] = world.grids[(int)worldPos.x, (int)worldPos.y - 1].contents[(int)gridPos.x, 0];
            }
            catch
            {
                Debug.Log("Grid " + (int)worldPos.x + ", " + ((int)worldPos.y - 1) + "does not exist.");
            }
        }
        // Find adjacent down in same grid
        else
        {
            neighbours[(int)Direction.down] = world.grids[(int)worldPos.x, (int)worldPos.y].contents[(int)gridPos.x, (int)gridPos.y - 1];
        }

        // Find adjacent left in adjacent grid
        if (gridPos.x == 9)
        {
            try
            {
                neighbours[(int)Direction.left] = world.grids[(int)worldPos.x - 1, (int)worldPos.y].contents[0, (int)gridPos.y];
            }
            catch
            {
                Debug.Log("Grid " + ((int)worldPos.x - 1) + ", " + (int)worldPos.y + "does not exist.");
            }
        }
        // Find adjacent left in same grid
        else
        {
            neighbours[(int)Direction.left] = world.grids[(int)worldPos.x, (int)worldPos.y].contents[(int)gridPos.x - 1, (int)gridPos.y];
        }
    }

    private void GetWallType()
    {
        bool[] directions = new bool[4];

        // Work out which neighbours are the same type as this bit
        for (int i = 0; i < 4; ++i)
        {
            if (neighbours[i].bitType == bitType)
            {
                directions[i] = true;
            }
            else
            {
                directions[i] = false;
            }
        }

        // Save data about neighbouring types
        if (directions[(int)Direction.up])
        {
            if (directions[(int)Direction.right])
            {
                if (directions[(int)Direction.down])
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x1234;
                    }
                    else
                    {
                        wallShape = WallShape.x123;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x124;
                    }
                    else
                    {
                        wallShape = WallShape.x12;
                    }
                }
            }
            else
            {
                if (directions[(int)Direction.down])
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x134;
                    }
                    else
                    {
                        wallShape = WallShape.x13;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x14;
                    }
                    else
                    {
                        wallShape = WallShape.x1;
                    }
                }
            }
        }
        else
        {
            if (directions[(int)Direction.right])
            {
                if (directions[(int)Direction.down])
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x234;
                    }
                    else
                    {
                        wallShape = WallShape.x23;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x24;
                    }
                    else
                    {
                        wallShape = WallShape.x2;
                    }
                }
            }
            else
            {
                if (directions[(int)Direction.down])
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x34;
                    }
                    else
                    {
                        wallShape = WallShape.x3;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = WallShape.x4;
                    }
                    else
                    {
                        wallShape = WallShape.x0;
                    }
                }
            }
        }
    }

    private void GetWallTypeString()
    {
        string code = "x";
        // Work out which neighbours are the same type as this bit
        for (int i = 0; i < 4; ++i)
        {
            if (neighbours[i].bitType == bitType)
            {
                code += (i + 1).ToString();
            }
        }

        wallShape = (WallShape)System.Enum.Parse(typeof(WallShape), code);
    }

    public void ReceiveShortList(bool[] shortList)
    {
        this.shortList = shortList;
    }
}