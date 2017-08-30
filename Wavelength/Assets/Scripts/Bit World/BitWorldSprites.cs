using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldSprites : MonoBehaviour
{
    Dictionary<BitType, Sprite> bitSprites = new Dictionary<BitType, Sprite>();
    Dictionary<WallShape, Sprite> wallShapeSprites = new Dictionary<WallShape, Sprite>();

    // Use this for initialization
    void Start()
    {

    }

    public Sprite GetBitSprite(BitType bt)
    {
        return bitSprites[bt];
    }

    public Sprite GetWallShapeSprite(WallShape ws)
    {
        return wallShapeSprites[ws];
    }
}
