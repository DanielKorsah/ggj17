﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldMaker : MonoBehaviour
{
    public List<ColourtoPrefab> relations = new List<ColourtoPrefab>();
    public Texture2D world;
    public Transform bit;
    public Transform grid;
    public Transform gridWorld;
    private Transform instantiatedWorld;
    private World instantiatedWorldScript;

    // Upon starting create the world
    void Start()
    {
        MakeWorld();
    }
    // Function for making the world
    private void MakeWorld()
    {
        // The instantiated world prefab. Saving this allows easy parenting.
        instantiatedWorld = Instantiate(gridWorld, new Vector3(0, 0, 0), Quaternion.identity);
        // The world script attached to the prefab.  Saving this prevents "GetComponent<>"
        instantiatedWorldScript = instantiatedWorld.GetComponent<World>();
        // Spawn in the neccessary grids before moving on to bits
        MakeGrids();
        Color mapPixel;
        // Go through pixel by pixel and use their colours to populate the world
        for (int x = 0; x < world.width; ++x)
        {
            for (int y = 0; y < world.height; ++y)
            {
                // Get a pixel from the world map to be tested
                mapPixel = world.GetPixel(x, y);
                // If the pixel is black: instantiate a bit
                //if (mapPixel == new Color(0, 0, 0))
                //{
                //    MakeBit(x, y);
                //}
                FindColour(x, y, mapPixel);
            }
        }
    }
    // Function for instantiating all grids
    private void MakeGrids()
    {
        // Calculate the number of grids by dividing image width and height by the number of bits per grid (10)
        int worldWidth = world.width / 10;
        int worldHeight = world.height / 10;
        for (int x = 0; x < worldWidth; ++x)
        {
            for (int y = 0; y < worldHeight; ++y)
            {
                // Instantiate the grid as a GameObject in the world class' grid array
                instantiatedWorldScript.AddGridObj(Instantiate(grid, new Vector3(x * 2, y * 2, 0), Quaternion.identity, instantiatedWorld), x, y);
            }
        }
    }
    // Function for instantiating a bit
    private void MakeBit(int x, int y, Transform prefab)
    {
        // Get grid coordinates within the world by dividing pixel x & y by width of grids (10)
        int worldX = x / 10;
        int worldY = y / 10;
        // Get bit position within grid by finding the remainder after dividing by grid width (10)
        int gridX = x % 10;
        int gridY = y % 10;
        // Get unity transform coordinates by multiplying pixel coordinates by the size of a bit (0.2)
        float coordX = x * 0.2f;
        float coordY = y * 0.2f;
        // Instantiate the bit in the appropriate grid, with that grid as its parent
        instantiatedWorldScript.GetGrid(worldX, worldY).AddBit(Instantiate(prefab, new Vector3(coordX, coordY, 0), Quaternion.identity, instantiatedWorldScript.gridsObjs[worldX, worldY]).GetComponent<Bit>(), gridX, gridY);
        // Tell the bit where it is in the grid and the world
        instantiatedWorldScript.GetGrid(worldX, worldY).GetBit(gridX, gridY).SetLocationData(worldX, worldY, gridX, gridY);
    }
    // Function to choose the prefab type to make
    private void FindColour(int x, int y, Color pixel)
    {
        foreach (ColourtoPrefab pair in relations)
        {
            if (pixel == pair.colour)
            {
                MakeBit(x, y, pair.prefab);
                break;
            }
        }
    }
}
