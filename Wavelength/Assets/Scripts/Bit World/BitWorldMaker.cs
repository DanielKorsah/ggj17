using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitWorldMaker : MonoBehaviour
{
    public List<BitTypetoPrefab> relations = new List<BitTypetoPrefab>();
    public Texture2D world;
    public Transform grid;
    public Transform gridWorld;
    public Transform voidPrefab;
    private Transform instantiatedWorld;
    private World instantiatedWorldScript;
    private Bit mostRecentBit;

    private List<Color32> beaconUpgradeColours = new List<Color32>
            { new Color32( 0, 200, 255, 255 ), new Color32(0, 150, 255, 255), new Color32(0, 100, 255, 255), new Color32(0, 50, 255, 255) };
    private List<Color32> beaconFacingColours = new List<Color32>
            { new Color32( 0, 135, 204, 255 ), new Color32(0, 101, 153, 255), new Color32(0, 67, 102, 255), new Color32(0, 33, 51, 255) };
    private List<Color32> beaconStartingColours = new List<Color32>
            { new Color32( 153, 0, 0, 255 ), new Color32(153, 153, 0, 255), new Color32(153, 0, 153, 255)  };
    private List<Color32> pickupUpgradeColours = new List<Color32>
            { new Color32( 0, 0, 153, 255 ), new Color32(0, 0, 102, 255), new Color32(0, 0, 51, 255)};

    private BitWorldKnowledge knowledge = BitWorldKnowledge.Instance;

    public List<Texture2D> levels = new List<Texture2D>();
    public int currentLevel = 0;

    // Upon starting create the world
    void Start()
    {
        // ~~~ Loading screen
        world = levels[currentLevel];
        MakeWorld();
    }

    // Call when level completed
    public void NextLevel()
    {
        // ~~~ Loading screen
        // clear world
        Destroy(instantiatedWorld.gameObject);
        PickupItem[] pickups = FindObjectsOfType<PickupItem>();
        if (pickups.Length > 0)
        {
            for (int i = 0; i < pickups.Length; ++i)
            {
                Destroy(pickups[i].gameObject);
            }
        }
        // change world
        ++currentLevel;
        if (currentLevel == levels.Count)
        {
            // ~~~ ending level
        }
        world = levels[currentLevel];
        // load new world
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
        Color32 mapPixel;
        // Go through pixel by pixel and use their colours to populate the world
        for (int x = 0; x < world.width; ++x)
        {
            for (int y = 0; y < world.height; ++y)
            {
                // Get a pixel from the world map to be tested
                mapPixel = world.GetPixel(x, y);
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
    private void MakeBit(int x, int y, Transform prefab, BitType bitType)
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
        mostRecentBit = instantiatedWorldScript.GetGrid(worldX, worldY).AddBit(Instantiate(prefab, new Vector3(coordX, coordY, 0), Quaternion.identity, instantiatedWorldScript.gridsObjs[worldX, worldY]).GetComponent<Bit>(), gridX, gridY);
        // Tell the bit where it is in the grid and the world
        mostRecentBit.SetLocationData(worldX, worldY, gridX, gridY);
        mostRecentBit.BitTypeSet = bitType;
    }
    // Function to choose the prefab type to make
    private void FindColour(int x, int y, Color32 pixel)
    {
        bool found = false;
        foreach (BitTypetoPrefab pair in relations)
        {
            if (knowledge.BitTypeByColour.ContainsKey(pair.bitType) && pixel.Equals(knowledge.BitTypeByColour[pair.bitType]))
            {
                MakeBit(x, y, pair.prefab, pair.bitType);
                found = true;
                break;
            }
        }
        if (!found)
        {
            BitType bitType = BitType.Void;
            if (pixel.a != 0)
            {
                if (FindColourInList(beaconFacingColours, pixel))
                {
                    bitType = BitType.BeaconTR;
                }
                else if (FindColourInList(beaconStartingColours, pixel))
                {
                    bitType = BitType.BeaconTL;
                }
                else if (FindColourInList(beaconUpgradeColours, pixel))
                {
                    bitType = BitType.BeaconBR;
                }
                else if (FindColourInList(pickupUpgradeColours, pixel))
                {
                    bitType = BitType.Upgrade;
                }
                if (bitType != BitType.Void)
                {
                    MakeBit(x, y, FindPrefabFromBitType(bitType), bitType);
                    mostRecentBit.GiveMulticolourInfo(pixel);
                    found = true;
                }
            }
            if (!found)
            {
                MakeBit(x, y, voidPrefab, BitType.Void);
            }
        }
    }

    private bool FindColourInList(List<Color32> list, Color32 pixel)
    {
        foreach (Color32 c in list)
        {
            if (pixel.Equals(c))
            {
                return true;
            }
        }
        return false;
    }

    private Transform FindPrefabFromBitType(BitType bitType)
    {
        foreach(BitTypetoPrefab pair in relations)
        {
            if(pair.bitType == bitType)
            {
                return pair.prefab;
            }
        }
        return null;
    }

    private void MulticolourFollowUp(int x, int y, Color32 pixel)
    {
        // Get grid coordinates within the world by dividing pixel x & y by width of grids (10)
        int worldX = x / 10;
        int worldY = y / 10;
        // Get bit position within grid by finding the remainder after dividing by grid width (10)
        int gridX = x % 10;
        int gridY = y % 10;

    }
}
