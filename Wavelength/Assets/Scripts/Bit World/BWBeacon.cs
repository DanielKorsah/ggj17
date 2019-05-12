using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWBeacon : Bit
{
    bool playerContact = false;

    List<Grid> gridsAffecting = new List<Grid>();
    Wavelength beaconOutput = Wavelength.I;
    Pickup pickup = Pickup.none;
    Direction direction = Direction.up;
    SpriteRenderer beaconSprite;

    private void Awake()
    {
        bitType = BitType.BeaconBL;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            if(sr.gameObject != gameObject)
            {
                beaconSprite = sr;
                break;
            }
        }
        beaconSprite.color = BitWorldKnowledge.Instance.AirColourByWavelength[beaconOutput];
        base.Start();
        StartCoroutine(LateStart());
    }

    // Avoids null reference of walls calculting shape before having all neighbours ~~~ will need loading screen to hide first frame change
    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        CalculateGrids();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is on and pushes activate ~~~ temporarily not on for testing
        if (!playerContact && Input.GetKeyDown(KeyCode.E))
        {
            RotateOutput();
        }
    }

    // Update grids affected by this beacon and calculate which it should effect
    private void CalculateGrids()
    {
        RemoveAffector();
        gridsAffecting.Clear();
        switch (pickup)
        {
            case Pickup.none:
                // Add this beacon's grid as the sole effected grid
                gridsAffecting.Add(world.GetGrid(worldPos));
                break;
            case Pickup.line:
                {
                    // Find the direction the light travels
                    Vector2Int moveDir = GetLightVector();
                    // Variable to track grid to find
                    Vector2Int nextPos = worldPos;
                    // Grid to add to list
                    Grid nextGrid = world.GetGrid(nextPos);
                    // While a grid has been found
                    while (nextGrid != null)
                    {
                        // Add grid to list
                        gridsAffecting.Add(nextGrid);
                        // Move position once
                        nextPos += moveDir;
                        // Look for next grid
                        nextGrid = world.GetGrid(nextPos);
                    }
                    break;
                }
            case Pickup.area:
                {
                    Grid nextGrid;
                    // Change on the x axis
                    for (int x = -1; x < 2; ++x)
                    {
                        // Change on the y axis
                        for (int y = -1; y < 2; ++y)
                        {
                            // Look for grid at position and add if exists
                            nextGrid = world.GetGrid(worldPos.x + x, worldPos.y + y);
                            if (nextGrid != null)
                            {
                                gridsAffecting.Add(nextGrid);
                            }
                        }
                    }
                    break;
                }
            case Pickup.displace:
                {
                    // Find the direction the light travels
                    Vector2Int moveDir = GetLightVector();
                    // Look for grid at position and add if exists
                    Grid newGrid = world.GetGrid(worldPos + moveDir);
                    if (newGrid != null)
                    {
                        gridsAffecting.Add(newGrid);
                    }
                    break;
                }
            default:
                break;
        }
        AddAffector();
    }

    private Vector2Int GetLightVector()
    {
        switch (direction)
        {
            case Direction.up:
                return new Vector2Int(0, 1);
            case Direction.right:
                return new Vector2Int(1, 0);
            case Direction.down:
                return new Vector2Int(0, -1);
            case Direction.left:
                return new Vector2Int(-1, 0);
            default:
                return new Vector2Int(0, 1);
        }
    }

    // Set for beacon output toggling to avoid forgetting to add/remove appropriately
    private Wavelength BeaconOutput
    {
        set
        {
            RemoveAffector();
            beaconOutput = value;
            beaconSprite.color = BitWorldKnowledge.Instance.AirColourByWavelength[beaconOutput];
            AddAffector();
        }
    }

    // Change beacon output (via variable setter)
    private void RotateOutput()
    {
        switch (beaconOutput)
        {
            case Wavelength.U:
                BeaconOutput = Wavelength.V;
                break;
            case Wavelength.V:
                BeaconOutput = Wavelength.I;
                break;
            case Wavelength.I:
                BeaconOutput = Wavelength.U;
                break;
            default:
                BeaconOutput = Wavelength.I;
                break;
        }
    }

    // Remove affector from list of grids
    private void RemoveAffector()
    {
        foreach (Grid g in gridsAffecting)
        {
            g.RemoveAffector(beaconOutput);
        }
    }

    // Add affector to list of grids
    private void AddAffector()
    {
        foreach (Grid g in gridsAffecting)
        {
            g.AddAffector(beaconOutput);
        }
    }
    // Check if player enters hitbox
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerContact = true;
        }
    }

    // Check if player exits hitbox
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerContact = false;
        }
    }
}
