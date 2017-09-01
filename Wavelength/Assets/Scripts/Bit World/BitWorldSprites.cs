using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldSprites
{
    // Dictionaries of sprites
    Dictionary<BitType, Sprite> bitSprites = new Dictionary<BitType, Sprite>();
    Dictionary<WallShape, Sprite> wallShapeSprites = new Dictionary<WallShape, Sprite>();

    // Private instance of this class (singleton)
    static BitWorldSprites instance;

    // Private constructor (singleton)
    private BitWorldSprites() { }

    // Singleton instatioation method
    static public BitWorldSprites Instantiate
    {
        get
        {
            if (instance == null)
            {
                instance = new BitWorldSprites();
            }
            return instance;
        }
    }

    // Add sprite to dictionary
    public void AddBitSprite(BitType bt, Sprite sprite)
    {
        bitSprites.Add(bt, sprite);
    }

    // Add wall shape to dictionary
    public void AddWallShapeSprite(WallShape ws, Sprite sprite)
    {
        wallShapeSprites.Add(ws, sprite);
    }

    // Get the sprite for a BitType
    public Sprite GetBitSprite(BitType bt)
    {
        return bitSprites[bt];
    }

    // Get the sprite for a WallShape
    public Sprite GetWallShapeSprite(WallShape ws)
    {
        return wallShapeSprites[ws];
    }
}
