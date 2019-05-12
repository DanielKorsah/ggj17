using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour
{
    protected World world;  // Maybe make static
    protected BitWorldSprites spriteSheet;  // Maybe make static

    protected BitType bitType = BitType.Air;
    protected BitType displayType = BitType.Air;
    protected bool neighbourDependant = false;
    protected bool showColour = true;

    protected bool[] shortList = new bool[3];
    protected Wavelength shortListEnum = Wavelength.None;

    public Vector2Int gridPos = new Vector2Int();
    public Vector2Int worldPos = new Vector2Int();

    public Bit[] neighbours = new Bit[4];
    public bool[] neighboursSimilar = new bool[] { false, false, false, false };
    public BitShape wallShape;

    protected SpriteRenderer sprite;

    protected Time lastUpdate = null;

    protected virtual void Start()
    {
        world = FindObjectOfType<World>();
        spriteSheet = BitWorldSprites.Instantiate;
        sprite = GetComponent<SpriteRenderer>();
        GetNeighbours();
        GetBitShapeString();
        UpdateSprite();
    }
    // Finds the four orthoganal neighbours to this bit
    protected void GetNeighbours()
    {
        // Find adjacent up in adjacent grid
        if (gridPos.y == 9)
        {
            try
            {
                neighbours[(int)Direction.up] = world.GetGrid((int)worldPos.x, (int)worldPos.y + 1).GetBit((int)gridPos.x, 0);
            }
            catch
            {
                Debug.Log("Grid " + (int)worldPos.x + ", " + ((int)worldPos.y + 1) + "does not exist.");
            }
        }
        // Find adjacent up in same grid
        else
        {
            neighbours[(int)Direction.up] = world.GetGrid((int)worldPos.x, (int)worldPos.y).GetBit((int)gridPos.x, (int)gridPos.y + 1);
        }

        // Find adjacent right in adjacent grid
        if (gridPos.x == 9)
        {
            try
            {
                neighbours[(int)Direction.right] = world.GetGrid((int)worldPos.x + 1, (int)worldPos.y).GetBit(0, (int)gridPos.y);
            }
            catch
            {
                Debug.Log("Grid " + ((int)worldPos.x + 1) + ", " + (int)worldPos.y + "does not exist.");
            }
        }
        // Find adjacent right in same grid
        else
        {
            neighbours[(int)Direction.right] = world.GetGrid((int)worldPos.x, (int)worldPos.y).GetBit((int)gridPos.x + 1, (int)gridPos.y);
        }

        // Find adjacent down in adjacent grid
        if (gridPos.y == 0)
        {
            try
            {
                neighbours[(int)Direction.down] = world.GetGrid((int)worldPos.x, (int)worldPos.y - 1).GetBit((int)gridPos.x, 9);
            }
            catch
            {
                Debug.Log("Grid " + (int)worldPos.x + ", " + ((int)worldPos.y - 1) + "does not exist.");
            }
        }
        // Find adjacent down in same grid
        else
        {
            neighbours[(int)Direction.down] = world.GetGrid((int)worldPos.x, (int)worldPos.y).GetBit((int)gridPos.x, (int)gridPos.y - 1);
        }

        // Find adjacent left in adjacent grid
        if (gridPos.x == 0)
        {
            try
            {
                neighbours[(int)Direction.left] = world.GetGrid((int)worldPos.x - 1, (int)worldPos.y).GetBit(9, (int)gridPos.y);
            }
            catch
            {
                Debug.Log("Grid " + ((int)worldPos.x - 1) + ", " + (int)worldPos.y + "does not exist.");
            }
        }
        // Find adjacent left in same grid
        else
        {
            neighbours[(int)Direction.left] = world.GetGrid((int)worldPos.x, (int)worldPos.y).GetBit((int)gridPos.x - 1, (int)gridPos.y);
        }
    }
    // Gets the shape of the bit (badly)
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
                        wallShape = BitShape.x1234;
                    }
                    else
                    {
                        wallShape = BitShape.x123;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = BitShape.x124;
                    }
                    else
                    {
                        wallShape = BitShape.x12;
                    }
                }
            }
            else
            {
                if (directions[(int)Direction.down])
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = BitShape.x134;
                    }
                    else
                    {
                        wallShape = BitShape.x13;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = BitShape.x14;
                    }
                    else
                    {
                        wallShape = BitShape.x1;
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
                        wallShape = BitShape.x234;
                    }
                    else
                    {
                        wallShape = BitShape.x23;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = BitShape.x24;
                    }
                    else
                    {
                        wallShape = BitShape.x2;
                    }
                }
            }
            else
            {
                if (directions[(int)Direction.down])
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = BitShape.x34;
                    }
                    else
                    {
                        wallShape = BitShape.x3;
                    }
                }
                else
                {
                    if (directions[(int)Direction.left])
                    {
                        wallShape = BitShape.x4;
                    }
                    else
                    {
                        wallShape = BitShape.x0;
                    }
                }
            }
        }
    }
    // Gets the shape of the bit
    protected void GetBitShapeString()
    {
        if (neighbourDependant)
        {
            string code = "x";
            // Work out which neighbours are the same type as this bit
            for (int i = 0; i < 4; ++i)
            {
                if (neighbours[i].displayType == displayType)
                {
                    neighboursSimilar[i] = true;
                    code += (i + 1).ToString();
                }
                else
                {
                    neighboursSimilar[i] = false;
                }
            }
            if (code != "x")
            {
                wallShape = (BitShape)System.Enum.Parse(typeof(BitShape), code);
            }
            else
            {
                wallShape = BitShape.x0;
            }
        }
    }
    // For recieving an update from the grid this bit is in
    public void UpdatedByGrid(bool[] shortList, Wavelength shortListEnum)
    {
        this.shortList = shortList;
        this.shortListEnum = shortListEnum;
        UpdateSprite();
        //UpdateNeighbours();
        //UpdateBitShape();
        world.UpdateAdjacentBits(this);
    }
    // Prompt for updating neighbours that may need to change their sprite
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
    // For recieving an update from a neighbouring bit
    public void UpdatedByNeighbour()
    {
        UpdateBitShape();
    }
    // Updates the shape of the bit based on surrounding bits
    protected void UpdateBitShape()
    {
        if (neighbourDependant)
        {
            GetBitShapeString();
            UpdateSprite();
        }
    }
    // Update the sprite colour to match the wavelengths affecting the tile
    protected virtual void UpdateSprite()
    {
        //gameObject.GetComponent<SpriteRenderer>().sprite = spriteSheet.GetAirColourSprites(shortListEnum);
        sprite.sortingOrder = (int)displayType;
        if (displayType == BitType.Air)
        {
            sprite.color = BitWorldKnowledge.Instance.AirColourByWavelength[shortListEnum];
        }
    }
    // Get information about bit location
    public void SetLocationData(int worldX, int worldY, int gridX, int gridY)
    {
        worldPos.x = worldX;
        worldPos.y = worldY;
        gridPos.x = gridX;
        gridPos.y = gridY;
    }

    // BitTypeSet only implemented in coloured walls
    public virtual BitType BitTypeSet { set { } }
    public BitType DisplayTypeGet
    {
        get { return displayType; }
    }

    // Methods for use by beacon bits (incomplete)
    public Wavelength BeaconGetWaveLength()
    {
        return Wavelength.V;
    }

    public Direction BeaconGetFacing()
    {
        return Direction.up;
    }

    public Pickup BeaconGetPickup()
    {
        return Pickup.none;
    }
}