using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{

    [SerializeField]
    private string startEmissionType;
    private string emissionType;

    //the status of the beacon's upgrades
    //0 - none, 1 - line, 2 - area, 3 - offset
    [SerializeField]
    private int upgrade = 0;

    //0 - up, 1 - right, 2 - down, 3 - left
    [SerializeField]
    private int rot = 0;

    private int x;
    private int y;

    //the list of grids currently affected by this beacon
    List<grid> affecting = new List<grid>();

    //parent grid
    grid parentG;

    //this beacons sprite renderer
    SpriteRenderer sprite;

    [SerializeField]
    private Sprite spriteNormal;
    [SerializeField]
    private Sprite spriteLine;
    [SerializeField]
    private Sprite spriteArea;
    [SerializeField]
    private Sprite spriteOffset;

    private bool playerOn = false;
    Inventory playerInventory;

    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        parentG = GetComponentInParent<grid>();
        emissionType = startEmissionType;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOn)
        {
            // Logic for adding or removing upgrades
            if (Input.GetKeyDown("1"))
            {
                if (upgrade != 1 && playerInventory.FocusPickup > 0)
                {
                    returnUpgrade();
                    upgrade = 1;
                    playerInventory.SubFocusPickup();
                    beaconChange();
                }
            }
            if (Input.GetKeyDown("2") && playerInventory.BoostPickup > 0)
            {
                if (upgrade != 2)
                {
                    returnUpgrade();
                    upgrade = 2;
                    playerInventory.SubBoostPickup();
                    beaconChange();
                }
            }
            if (Input.GetKeyDown("3") && playerInventory.ProjectPickup > 0)
            {
                if (upgrade != 3)
                {
                    returnUpgrade();
                    upgrade = 3;
                    playerInventory.SubProjectPickup();
                    beaconChange();
                }
            }
            if (Input.GetKeyDown("0"))
            {
                if (upgrade != 0)
                {
                    returnUpgrade();
                    upgrade = 0;
                    beaconChange();
                }
            }

            // Logic for changing the rotation of a beacon
            if (Input.GetKeyDown("i"))
            {
                rot = 0;
                beaconChange();
            }
            if (Input.GetKeyDown("l"))
            {
                rot = 1;
                beaconChange();
            }
            if (Input.GetKeyDown("k"))
            {
                rot = 2;
                beaconChange();
            }
            if (Input.GetKeyDown("j"))
            {
                rot = 3;
                beaconChange();
            }

            // Change the beacon's output colour
            if (Input.GetKeyDown("e"))
            {
                emissionChange();
            }
        }
    }

    //all the things the beacon needs to do while its grid's start function takes place
    public void FirstTimeSetUp(int x, int y)
    {
        this.x = x;
        this.y = y;

        getAffectedGrids();
        
        emitting();
        changeSpriteColour();
        changeSpriteAppearance();
    }

    //set the beacons coordinates
    public void SetCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //methods required to change the power up/rotation
    private void beaconChange()
    {
        emitted();
        getAffectedGrids();
        changeSpriteAppearance();
        emitting();
    }

    //methods required to just change the emission type
    private void emissionChange()
    {
        emitted();
        rotateEmission();
        changeSpriteColour();
        emitting();
    }

    //set the emission type to be next in the cycle
    private void rotateEmission()
    {
        switch (emissionType)
        {

            case ("IR"):
                emissionType = "V";
                break;

            case ("V"):
                emissionType = "UV";
                break;

            case ("UV"):
                emissionType = "IR";
                break;

            default:
                break;
        }
    }

    //to get the grid squares affected by this beacon
    public void getAffectedGrids()
    {
        //clear the current list before adding more entries
        affecting.Clear();
        switch (upgrade)
        {
            case (0):
                affecting.Add(parentG);
                break;

            //line powerup
            case (1):
                switch (rot)
                {
                    //up
                    case (0):
                        for (int up = 0; up < 10; ++up)
                        {
                            try
                            {
                                affecting.Add(GameObject.Find(x.ToString() + "," + (y + up).ToString()).GetComponent<grid>());
                            }
                            catch
                            {
                                Debug.Log("No " + x.ToString() + "," + (y + up).ToString() + " exists");
                                break;
                            }
                        }
                        break;

                    //right
                    case (1):
                        for (int right = 0; right < 10; ++right)
                        {
                            try
                            {
                                affecting.Add(GameObject.Find((x + right).ToString() + "," + y.ToString()).GetComponent<grid>());
                            }
                            catch
                            {
                                Debug.Log("No " + (x + right).ToString() + "," + y.ToString() + " exists");
                                break;
                            }
                        }
                        break;

                    //down
                    case (2):
                        for (int down = 0; down < 10; ++down)
                        {
                            try
                            {
                                affecting.Add(GameObject.Find(x.ToString() + "," + (y - down).ToString()).GetComponent<grid>());
                            }
                            catch
                            {
                                Debug.Log("No " + x.ToString() + "," + (y - down).ToString() + " exists");
                                break;
                            }
                        }
                        break;

                    //left
                    case (3):
                        for (int left = 0; left < 10; ++left)
                        {
                            try
                            {
                                affecting.Add(GameObject.Find((x - left).ToString() + "," + y.ToString()).GetComponent<grid>());
                            }
                            catch
                            {
                                Debug.Log("No " + (x - left).ToString() + "," + y.ToString() + " exists");
                                break;
                            }
                        }
                        break;

                    default:
                        Debug.Log("Rotation switch, line upgrade.");
                        break;

                }
                break;

            //area pickup
            case (2):
                //horizontal changes
                for (int horizontal = -1; horizontal <= 1; ++horizontal)
                {
                    //vertical changes
                    for (int vertical = -1; vertical <= 1; ++vertical)
                    {
                        try
                        {
                            affecting.Add(GameObject.Find((x + horizontal).ToString() + "," + (y + vertical).ToString()).GetComponent<grid>());
                        }
                        catch
                        {
                            Debug.Log("No " + (x + horizontal).ToString() + "," + (y + vertical).ToString() + " exists");
                        }
                    }
                }
                break;

            // jump pickup
            case (3):
                switch (rot)
                {
                    //up
                    case (0):
                        try
                        {
                            affecting.Add(GameObject.Find(x.ToString() + "," + (y + 1).ToString()).GetComponent<grid>());
                        }
                        catch
                        {
                            Debug.Log("No " + x.ToString() + "," + (y + 1).ToString() + " exists");
                        }
                        break;

                    //right
                    case (1):
                        try
                        {
                            affecting.Add(GameObject.Find((x + 1).ToString() + "," + y.ToString()).GetComponent<grid>());
                        }
                        catch
                        {
                            Debug.Log("No " + (x + 1).ToString() + "," + y.ToString() + " exists");
                        }

                        break;

                    //down
                    case (2):
                        try
                        {
                            affecting.Add(GameObject.Find(x.ToString() + "," + (y - 1).ToString()).GetComponent<grid>());
                        }
                        catch
                        {
                            Debug.Log("No " + x.ToString() + "," + (y - 1).ToString() + " exists");
                        }

                        break;

                    //left
                    case (3):
                        try
                        {
                            affecting.Add(GameObject.Find((x - 1).ToString() + "," + y.ToString()).GetComponent<grid>());
                        }
                        catch
                        {
                            Debug.Log("No " + (x - 1).ToString() + "," + y.ToString() + " exists");
                        }

                        break;

                    default:
                        Debug.Log("Rotation switch default, jump upgrade.");
                        break;

                }
                break;

            default:
                //this should never happen
                Debug.Log("Default case in get affected grids.");
                break;
        }
    }

    //tell the grids about what to not block anymore
    private void emitted()
    {
        foreach (grid grid in affecting)
        {
            grid.OldAffector(emissionType);
            grid.ChangeWalls();
        }
    }

    //tell the grids what to block instead
    private void emitting()
    {
        foreach (grid grid in affecting)
        {
            grid.NewAffector(emissionType);
            grid.ChangeWalls();
        }
    }

    //change the beacon's colour
    private void changeSpriteColour()
    {
        switch (emissionType)
        {
            case ("IR"):
                sprite.color = Color.red;
                break;

            case ("V"):
                sprite.color = Color.yellow;
                break;

            case ("UV"):
                sprite.color = Color.magenta;
                break;

            default:
                break;
        }
    }

    //change the sprite of a beacon
    private void changeSpriteAppearance()
    {
        switch (upgrade)
        {
            case (0):
                sprite.sprite = spriteNormal;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case (1):
                sprite.sprite = spriteLine;
                changeSpriteRotation();
                break;
            case (2):
                sprite.sprite = spriteArea;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case (3):
                sprite.sprite = spriteOffset;
                changeSpriteRotation();
                break;
            default:
                break;
        }
    }

    //change the rotation of a beacon
    private void changeSpriteRotation()
    {
        switch (rot)
        {
            case (0):
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case (1):
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case (2):
                transform.rotation = Quaternion.Euler(0, 0, -180);
                break;
            case (3):
                transform.rotation = Quaternion.Euler(0, 0, -270);
                break;
            default:
                break;
        }
    }

    //return an upgrade to the player
    private void returnUpgrade()
    {
        switch (upgrade)
        {
            case (1):
                playerInventory.AddFocusPickup();
                break;
            case (2):
                playerInventory.AddBoostPickup();
                break;
            case (3):
                playerInventory.AddProjectPickup();
                break;
            default:
                break;
        }
    }

    //when the player walks on to the collider, set playerOn to true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerOn = true;
            playerInventory = other.GetComponent<Inventory>();
        }
    }

    //when the player walks on to the collider, set playerOn to false
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerOn = false;
        }
    }

}
