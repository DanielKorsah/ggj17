using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour
{
    protected World world;
    protected BitWorldSprites spriteSheet;

    protected BitType bitType = BitType.Air;
    public BitType displayType = BitType.Air;
    protected bool neighbourDependant = false;
    protected bool showColour = true;

    protected bool[] shortList = new bool[3];

    protected Vector2 gridPos = new Vector2();
    protected Vector2 worldPos = new Vector2();

    protected Bit[] neighbours = new Bit[4];
    protected WallShape wallShape;

    protected Time lastUpdate = null;

    private void Awake()
    {
    }

    private void Start()
    {
        world = FindObjectOfType<World>();
        spriteSheet = FindObjectOfType<BitWorldSprites>();
        GetNeighbours();
        GetWallTypeString();
    }

    protected void GetNeighbours()
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

    protected void GetWallType()
    {
        bool[] directions = new bool[4];

        // Work out which neighbours are the same type as this bit
        for (int i = 0; i < 4; ++i)
        {
            if (neighbours[i].displayType == displayType)
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

    protected void GetWallTypeString()
    {
        if (neighbourDependant)
        {
            string code = "x";
            // Work out which neighbours are the same type as this bit
            for (int i = 0; i < 4; ++i)
            {
                if (neighbours[i].displayType == displayType)
                {
                    code += (i + 1).ToString();
                }
            }

            wallShape = (WallShape)System.Enum.Parse(typeof(WallShape), code);
        }
    }

    public void UpdatedByGrid(bool[] shortList)
    {
        this.shortList = shortList;

    }

    protected void UpdateNeighbours()
    {
        if (neighbourDependant)
        {
            foreach (Bit neighbour in neighbours)
            {
                neighbour.UpdatedByNeighbour();
            }
        }
    }

    public void UpdatedByNeighbour()
    {
        UpdateWallShape();
    }

    protected void UpdateWallShape()
    {
        GetWallTypeString();
    }

    protected void UpdateSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteSheet.GetBitSprite(displayType);
    }

    protected void DetermineWaveColour()
    {

    }
}