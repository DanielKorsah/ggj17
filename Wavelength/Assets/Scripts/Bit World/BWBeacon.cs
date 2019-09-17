using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWBeacon : Bit
{
    bool playerContact = false;

    List<Grid> gridsAffecting = new List<Grid>();
    Wavelength beaconOutput = Wavelength.None;
    Pickup pickup = Pickup.none;
    Direction direction = Direction.up;
    SpriteRenderer beaconSprite;

    static DirectionChoice wheel;
    ChoosingInfo choosing = ChoosingInfo.none;
    bool pickupSetSuccessful = true;

    BitWorldLibrarian librarian;

    private void Awake()
    {
        bitType = BitType.BeaconBL;
        displayType = BitType.Air;
        neighbourDependant = false;
        showColour = true;
    }

    // Start is called before the first frame update
    override public void Initialise()
    {
        if (wheel == null)
        {
            wheel = DirectionChoice.Instance;
        }
        // Find the sprite with the beacon on
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            if (sr.gameObject != gameObject)
            {
                beaconSprite = sr;
                break;
            }
        }

        librarian = FindObjectOfType<BitWorldLibrarian>();
        base.Initialise();

        StartBeaconInfo();
        // Set beacon sprite info
        beaconSprite.color = BitWorldKnowledge.Instance.AirColourByWavelength[beaconOutput];
        beaconSprite.sprite = librarian.BeaconSprites[(int)pickup];
        beaconSprite.transform.eulerAngles = new Vector3(0, 0, 360 - (int)direction * 90);
        // Give late start method to delegate
        FindObjectOfType<BitWorldMaker>().LateStartCall += LateStart;
        // Set input delegates
        InputManager im = InputManager.Instance;
        im.ChangeBeaconCall += RotateOutput;
        im.RotateBeaconCall += RotateDirection;
        im.PickupCall += ChoosingPickup;
        im.ChooseDirectionCall += ChoosingDirection;
        im.SelectionCall += ResolveChoice;
    }

    public override void ResetBit()
    {
        gridsAffecting.Clear();
        StartBeaconInfo();
        // Set beacon sprite info
        beaconSprite.color = BitWorldKnowledge.Instance.AirColourByWavelength[beaconOutput];
        beaconSprite.sprite = librarian.BeaconSprites[(int)pickup];
        beaconSprite.transform.eulerAngles = new Vector3(0, 0, 360 - (int)direction * 90);
        CalculateGrids();
        Choosing = ChoosingInfo.none;
    }

    // Avoids null reference of walls calculting shape before having all neighbours ~~~ will need loading screen to hide first frame change
    void LateStart()
    {
        CalculateGrids();
    }

    private void StartBeaconInfo()
    {
        beaconOutput = neighbours[(int)Direction.up].BeaconGetWaveLength();
        pickup = neighbours[(int)Direction.right].BeaconGetPickup();
        direction = world.GetGrid(worldPos).GetBit(gridPos.x + 1, gridPos.y + 1).BeaconGetFacing();
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
            SwapAffector(value, beaconOutput);
            beaconOutput = value;
            beaconSprite.color = BitWorldKnowledge.Instance.BeaconColourByWavelength[beaconOutput];
        }
    }

    private Pickup Pickup
    {
        set
        {
            if (value == pickup)
            {
                pickupSetSuccessful = true;
            }
            // If the player is removing the pickup, or has that pickup use it
            else if (value == Pickup.none || BWInventory.Instance.UsePickup(value))
            {
                // If the beacon held a pickup, return it
                if (pickup != Pickup.none)
                {
                    BWInventory.Instance.AddPickup(pickup);
                }
                // Set new pickup
                pickup = value;
                // Set new sprite
                beaconSprite.sprite = librarian.BeaconSprites[(int)pickup];
                // Change grids beacon affects
                CalculateGrids();
                pickupSetSuccessful = true;
            }
            else
            {
                pickupSetSuccessful = false;
            }
        }
    }

    private Direction Direction
    {
        set
        {
            direction = value;
            beaconSprite.transform.eulerAngles = new Vector3(0, 0, 360 - (int)direction * 90);
            CalculateGrids();
        }
    }

    private ChoosingInfo Choosing
    {
        set
        {
            // If setting to none, hide wheel
            if (value == ChoosingInfo.none)
            {
                choosing = value;
                wheel.HideChoice();
            }
            // If setting to current value, hide wheel
            else if (choosing == value)
            {
                wheel.HideChoice();
                choosing = ChoosingInfo.none;
            }
            // If setting to new value from none, show wheel
            else if (choosing == ChoosingInfo.none)
            {
                wheel.BeginChoice(value, transform.position + new Vector3(beaconSprite.size.x / 4, beaconSprite.size.y / 4, 0.0f));
                choosing = value;
            }
        }
    }

    // Change beacon direction 90 clockwise (via setter)
    private bool RotateDirection()
    {
        // If player isn't on this beacon, return
        if (!playerContact)
        {
            return false;
        }
        // If no choice being made, rotate
        if (choosing == ChoosingInfo.none)
        {
            switch (direction)
            {
                case Direction.up:
                    Direction = Direction.right;
                    break;
                case Direction.right:
                    Direction = Direction.down;
                    break;
                case Direction.down:
                    Direction = Direction.left;
                    break;
                case Direction.left:
                    Direction = Direction.up;
                    break;
                default:
                    Direction = Direction.up;
                    break;
            }
        }
        // If currently choosing direction, cancel
        else if (choosing == ChoosingInfo.direction)
        {
            Choosing = ChoosingInfo.none;
        }
        return choosing != ChoosingInfo.none;
    }

    // Change beacon output (via variable setter)
    private void RotateOutput()
    {
        // If player isn't on this beacon, return
        if (!playerContact)
        {
            return;
        }
        switch (beaconOutput)
        {
            case Wavelength.U:
                BeaconOutput = Wavelength.I;
                break;
            case Wavelength.V:
                BeaconOutput = Wavelength.U;
                break;
            case Wavelength.I:
                BeaconOutput = Wavelength.V;
                break;
            default:
                BeaconOutput = Wavelength.I;
                break;
        }
    }

    // Start pickup choosing process
    private bool ChoosingPickup()
    {
        // If player is on this beacon, set choosing
        if (playerContact)
        {
            Choosing = ChoosingInfo.pickup;
        }

        return choosing != ChoosingInfo.none;
    }

    // Start direction choosing process
    private bool ChoosingDirection()
    {
        // If player is on this beacon, set choosing
        if (playerContact)
        {
            Choosing = ChoosingInfo.direction;
        }

        return choosing != ChoosingInfo.none;
    }

    // Return true if wheel still open
    private bool ResolveChoice(Direction dir)
    {
        // If choosing pickup to use
        if (choosing == ChoosingInfo.pickup)
        {
            switch (dir)
            {
                case Direction.up:
                    Pickup = Pickup.none;
                    break;
                case Direction.right:
                    Pickup = Pickup.line;
                    break;
                case Direction.down:
                    Pickup = Pickup.area;
                    break;
                case Direction.left:
                    Pickup = Pickup.displace;
                    break;
                default:
                    Pickup = Pickup.none;
                    break;
            }
            wheel.MakeChoice(dir, pickupSetSuccessful);
            if (pickupSetSuccessful)
            {
                // End choosing mode
                Choosing = ChoosingInfo.none;
            }
        }
        // If choosing direction to point
        else if (choosing == ChoosingInfo.direction)
        {
            Direction = dir;
            wheel.MakeChoice(dir, true);
            // End choosing mode
            Choosing = ChoosingInfo.none;
        }
        return choosing != ChoosingInfo.none;
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

    // Swap the wavelength affecting the list of grids
    private void SwapAffector(Wavelength newAffector, Wavelength oldAffector)
    {
        foreach (Grid g in gridsAffecting)
        {
            g.SwapAffector(newAffector, oldAffector);
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
