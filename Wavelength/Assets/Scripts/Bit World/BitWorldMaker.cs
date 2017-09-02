using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldMaker : MonoBehaviour
{
    public Texture2D world;
    public Transform bit;
    public Transform grid;
    public Transform gridWorld;
    private Transform instantiatedWorld;
    private World instantiatedWorldScript;

    // Use this for initialization
    void Start()
    {
        MakeWorld();
    }

    private void MakeWorld()
    {
        instantiatedWorld = Instantiate(gridWorld, new Vector3(0, 0, 0), Quaternion.identity);
        instantiatedWorldScript = instantiatedWorld.GetComponent<World>();
        MakeGrids();
        Color mapPixel;
        for (int x = 0; x < world.width; ++x)
        {
            for (int y = 0; y < world.height; ++y)
            {
                mapPixel = world.GetPixel(x, y);
                Debug.Log("pixel x: " + x + ". Pixel y: " + y);
                if (mapPixel.a != 0/*new Color(1, 1, 1)*/)
                {
                    Debug.Log("Making bit at pixel x: " + x + ". Pixel y: " + y);
                    MakeBit(x, y);
                }
            }
        }
    }

    private void MakeGrids()
    {
        int worldWidth = world.width / 10;
        int worldHeight = world.height / 10;
        for (int x = 0; x < worldWidth; ++x)
        {
            for (int y = 0; y < worldHeight; ++y)
            {
                instantiatedWorldScript.AddGridObj(Instantiate(grid, new Vector3(x*2, y*2, 0), Quaternion.identity, instantiatedWorld), x, y);
                //instantiatedWorldScript.AddGrid(Instantiate(grid, new Vector3(0, 0, 0), Quaternion.identity, instantiatedWorld).GetComponent<Grid>(), x, y);
            }
        }
    }

    private void MakeBit(int x, int y)
    {
        int worldX = x / 10;
        int worldY = y / 10;
        int gridX = x % 10;
        int gridY = x % 10;
        instantiatedWorldScript.grids[worldX, worldY].AddBit(Instantiate(bit, new Vector3(x / 5.0f, y / 5.0f, 0), Quaternion.identity, instantiatedWorldScript.gridsObjs[worldX, worldY]).GetComponent<Bit>(), gridX, gridY);
    }
}
