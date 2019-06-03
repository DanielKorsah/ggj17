using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private CreationStage stage = CreationStage.none;
    private float[] creationProgress = new float[] { 1.0f, 0.0f, 0.0f };
    public delegate void UpdateLoadingScreen(CreationStage stage, float progress);
    public UpdateLoadingScreen UpdateLoadScreenCall;
    public delegate void LateStart();
    public LateStart LateStartCall;

    // Upon starting create the world
    void Start()
    {
        NextLevel();
    }

    public void NextLevel()
    {
        // Only go to level if not loading currently
        if (stage == CreationStage.finished || stage == CreationStage.none)
        {
            StartCoroutine(NextLevelCoroutine());
        }
    }

    private void UpdateLoadUI()
    {
        UpdateLoadScreenCall(stage, creationProgress[(int)stage]);
    }
    // For setting current creation progress easily
    private void SetProgress(float progress)
    {
        creationProgress[(int)stage] = progress;
        UpdateLoadUI();
    }
    // Call when level completed
    private IEnumerator NextLevelCoroutine()
    {
        // Show loading screen
        LoadingBar.Instance?.ShowLoadingScreen();
        // Reset loading stages
        creationProgress = new float[] { 0.0f, 0.0f, 0.0f };
        // If a world exists destroy it and advance level iterator
        if (instantiatedWorld != null)
        {
            // Clear world
            yield return StartCoroutine(ClearWorld());
            // Clear input delegates
            InputManager.Instance.ResetManager();
            // Advance level 
            ++currentLevel;
        }
        // If the final level has been completed
        if (currentLevel == levels.Count)
        {
            SceneManager.LoadScene("Completed");
            yield break;
        }
        // Select world png
        world = levels[currentLevel];
        // Load new world
        yield return StartCoroutine(MakeWorld());
        // Initialise new world
        yield return StartCoroutine(InitialiseWorld());
        // Hide loading screen
        LoadingBar.Instance?.HideLoadingScreen();
        stage = CreationStage.finished;
        yield return null;
        BWCamera.Instance.GoToPlayer();
    }

    // Destroy the current version of the world
    private IEnumerator ClearWorld()
    {
        stage = CreationStage.destruction;
        SetProgress(0.0f);
        yield return null; // ~~~ end frame

        // Clear beacons from late start
        LateStartCall = null;
        // Get list of pickups remaining
        PickupItem[] pickups = FindObjectsOfType<PickupItem>();
        // Calculate total grids in world
        int gridCount = world.width / 10 * world.height / 10;
        // Set remaining operations to pickups + grid destruction
        int operations = pickups.Length + gridCount + 1;

        // Get grids from world
        Grid[,] grids = instantiatedWorldScript.grids;
        int cleared = 0;
        // Destroy grids
        foreach (Grid g in grids)
        {
            if (g != null)
            {
                Destroy(g.gameObject);
                cleared++;
                SetProgress((float)cleared / operations);
                yield return null;
            }
        }
        // Destroy world
        Destroy(instantiatedWorld.gameObject);
        SetProgress((float)(cleared + 1) / operations);
        yield return null; // ~~~ end frame
        // Destroy any left-over pickups
        if (pickups.Length > 0)
        {
            for (int i = 0; i < pickups.Length; ++i)
            {
                Destroy(pickups[i].gameObject);
                SetProgress((operations - pickups.Length + i + 1) / operations);
                yield return null; // ~~~ end frame
            }
        }
        yield return new WaitForSeconds(0.3f);
    }

    // Function for making the world
    private IEnumerator MakeWorld()
    {
        stage = CreationStage.creation;
        SetProgress(0.0f);
        yield return null;

        BWCamera.Instance.ViewLoad(world.width / 10, world.height / 10);

        int gridCount = world.width / 10 * world.height / 10;
        int totalWork = world.width * world.height + gridCount;
        // The instantiated world prefab. Saving this allows easy parenting.
        instantiatedWorld = Instantiate(gridWorld, new Vector3(0, 0, 0), Quaternion.identity);
        // The world script attached to the prefab.  Saving this prevents "GetComponent<>"
        instantiatedWorldScript = instantiatedWorld.GetComponent<World>();
        // Spawn in the neccessary grids before moving on to bits
        MakeGrids();
        // Update ui with 1/11th progress
        SetProgress((float)gridCount / totalWork);
        yield return null;

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
            SetProgress((float)(gridCount + (x + 1) * world.height) / totalWork);
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
    }
    // Initialise all the bits (replaced Start()) 
    private IEnumerator InitialiseWorld()
    {
        stage = CreationStage.initialisation;
        SetProgress(0.0f);
        yield return null;

        Grid[,] grids = instantiatedWorldScript.grids;
        int gridCount = world.width / 10 * world.height / 10;
        int initialised = 0;

        foreach (Grid g in grids)
        {
            if (g != null)
            {
                g.InitialiseBits();
                initialised++;
                SetProgress((float)initialised / gridCount);
                yield return null;
            }
        }

        LateStartCall();
        yield return new WaitForSeconds(0.3f);
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
        foreach (BitTypetoPrefab pair in relations)
        {
            if (pair.bitType == bitType)
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
