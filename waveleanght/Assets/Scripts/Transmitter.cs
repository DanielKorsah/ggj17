using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{

    private Inventory inv;
    private List<WallState> scripts = new List<WallState>();
    bool contact;

    [SerializeField]
    int bonusType = 0; //0 - none, 1 - focus, 2 - boost
    [SerializeField]
    enum focusDirection { up, down, left, right };
    focusDirection fd = focusDirection.left;
    [SerializeField]
    enum projectDirection { up, down, left, right };
    projectDirection pd = projectDirection.left;

    [SerializeField]
    public Vector2 activeLocation;

    public List<string> state = new List<string>(new string[] { "infrared", "visible", "ultraviolet" });
    [SerializeField]
    public int i = 0;
    int prev;

    [SerializeField]
    List<Sprite> sprites = new List<Sprite>();

    SpriteRenderer spriterenderer;

    // Use this for initialization
    void Awake()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        { 
            scripts.Add(wall.GetComponent<WallState>());
        }
        SendState();
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        spriterenderer = GetComponent<SpriteRenderer>();
        changeColour();
    }

    int ff = 0;
    //on mouse click
    private void Update()
    {
        ff++;
        // Space for change frequency
        if (contact == true && Input.GetKeyDown(KeyCode.Space))
        {

            StateCycle();

            cullState();
            SendState();
        }
        
        // Focus mod
        if (contact == true && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)))
        {
            if (bonusType == 1)
            {
                //inv.AddFocusPickup();
                if (fd == focusDirection.down)
                {
                    SendNothing();
                    fd = focusDirection.left;
                    spriterenderer.sprite = sprites[2];
                    SendState();
                }
                else if (fd == focusDirection.left)
                {
                    SendNothing();
                    fd = focusDirection.up;
                    spriterenderer.sprite = sprites[4];
                    SendState();
                }
                else if (fd == focusDirection.up)
                {
                    SendNothing();
                    fd = focusDirection.right;
                    spriterenderer.sprite = sprites[3];
                    SendState();
                }
                else if (fd == focusDirection.right)
                {
                    SendNothing();
                    fd = focusDirection.down;
                    spriterenderer.sprite = sprites[1];
                    SendState();
                }
            }
            else if (bonusType == 2)
            {
                //inv.AddBoostPickup();
            }
            else if (inv.FocusPickup > 0)
            {
                inv.SubFocusPickup();


                //cullState();
                SendNothing();
                bonusType = 1;

                spriterenderer.sprite = sprites[4];
                fd = focusDirection.up;

                SendState();
            }

        }


        // Boost mod
        if (contact == true && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && inv.BoostPickup > 0 && bonusType == 0)
        {
            /*
            if (bonusType == 1)
            {
                inv.AddFocusPickup();
            }
            else if (bonusType == 2)
            {
                inv.AddBoostPickup();
            }
            
            */
            inv.SubBoostPickup();

            //cullState();
            SendNothing();
            bonusType = 2;

            spriterenderer.sprite = sprites[5];

            SendState();

        }



        // Project mod
        if (contact == true && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)))
        {
            if (bonusType == 3)
            {
                //inv.AddFocusPickup();
                if (pd == projectDirection.down)
                {
                    SendNothing();
                    pd = projectDirection.left;
                    spriterenderer.sprite = sprites[2];
                    SendState();
                }
                else if (pd == projectDirection.left)
                {
                    SendNothing();
                    pd = projectDirection.up;
                    spriterenderer.sprite = sprites[4];
                    SendState();
                }
                else if (pd == projectDirection.up)
                {
                    SendNothing();
                    pd = projectDirection.right;
                    spriterenderer.sprite = sprites[3];
                    SendState();
                }
                else if (pd == projectDirection.right)
                {
                    SendNothing();
                    pd = projectDirection.down;
                    spriterenderer.sprite = sprites[1];
                    SendState();
                }
            }
            else if (inv.ProjectPickup > 0)
            {
                inv.SubProjectPickup();
                
                //cullState();
                SendNothing();
                bonusType = 3;

                spriterenderer.sprite = sprites[4];
                pd = projectDirection.up;

                SendState();
            }

        }






        // E for pickup
        if (contact && Input.GetKeyDown(KeyCode.E) && bonusType > 0)
        {
            if (bonusType == 1)
            {
                SendNothing();
                bonusType = 0;
                SendState();
                inv.AddFocusPickup();
                spriterenderer.sprite = sprites[0];
            }
            else if (bonusType == 2)
            {
                SendNothing();
                bonusType = 0;
                SendState();
                inv.AddBoostPickup();
                spriterenderer.sprite = sprites[0];
            }
            else if (bonusType == 3)
            {
                SendNothing();
                bonusType = 0;
                SendState();
                inv.AddProjectPickup();
                spriterenderer.sprite = sprites[0];
            }
        }
    }

    //increases the index of state to be applied and loops at the max
    private void StateCycle()
    {
        prev = i;
        ++i;
        if (i == state.Count)
        {
            i = 0;
        }
        changeColour();
    }


    void SendState()
    {
        foreach (WallState script in scripts)
        {
            if (bonusType == 0)
            {
                if (script.gridLocation == activeLocation)
                {
                    script.StateUpdate(state[i]);
                }
            }
            else if (bonusType == 1)
            {
                if (fd == focusDirection.down)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y <= activeLocation.y)
                        script.StateUpdate(state[i]);
                if (fd == focusDirection.left)
                    if (script.gridLocation.x <= activeLocation.x && script.gridLocation.y == activeLocation.y)
                        script.StateUpdate(state[i]);
                if (fd == focusDirection.right)
                    if (script.gridLocation.x >= activeLocation.x && script.gridLocation.y == activeLocation.y)
                        script.StateUpdate(state[i]);
                if (fd == focusDirection.up)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y >= activeLocation.y)
                        script.StateUpdate(state[i]);
            }
            else if (bonusType == 2)
            {
                if (script.gridLocation.x >= activeLocation.x - 1 && script.gridLocation.x <= activeLocation.x + 1
                        && script.gridLocation.y >= activeLocation.y - 1 && script.gridLocation.y <= activeLocation.y + 1)
                {
                    script.StateUpdate(state[i]);
                }
            }
            else if (bonusType == 3)
            {
                if (pd == projectDirection.down)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y == activeLocation.y - 1)
                        script.StateUpdate(state[i]);
                if (pd == projectDirection.left)
                    if (script.gridLocation.x == activeLocation.x - 1 && script.gridLocation.y == activeLocation.y)
                        script.StateUpdate(state[i]);
                if (pd == projectDirection.right)
                    if (script.gridLocation.x == activeLocation.x + 1 && script.gridLocation.y == activeLocation.y)
                        script.StateUpdate(state[i]);
                if (pd == projectDirection.up)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y == activeLocation.y + 1)
                        script.StateUpdate(state[i]);
            }
        }
    }



    void cullState()
    {
        foreach (WallState script in scripts)
        {
            if (bonusType == 0)
            {
                if (script.gridLocation == activeLocation)
                {
                    script.StateCull(state[prev]);
                }
            }
            else if (bonusType == 1)
            {
                if (fd == focusDirection.down)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y <= activeLocation.y)
                        script.StateCull(state[prev]);
                if (fd == focusDirection.left)
                    if (script.gridLocation.x <= activeLocation.x && script.gridLocation.y == activeLocation.y)
                        script.StateCull(state[prev]);
                if (fd == focusDirection.right)
                    if (script.gridLocation.x >= activeLocation.x && script.gridLocation.y == activeLocation.y)
                        script.StateCull(state[prev]);
                if (fd == focusDirection.up)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y >= activeLocation.y)
                        script.StateCull(state[prev]);
            }
            else if (bonusType == 2)
            {
                if (script.gridLocation.x >= activeLocation.x - 1 && script.gridLocation.x <= activeLocation.x + 1
                        && script.gridLocation.y >= activeLocation.y - 1 && script.gridLocation.y <= activeLocation.y + 1)
                {
                    script.StateCull(state[prev]);
                }
            }
            else if (bonusType == 3)
            {
                if (pd == projectDirection.down)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y == activeLocation.y - 1)
                        script.StateCull(state[prev]);
                if (pd == projectDirection.left)
                    if (script.gridLocation.x == activeLocation.x - 1 && script.gridLocation.y == activeLocation.y)
                        script.StateCull(state[prev]);
                if (pd == projectDirection.right)
                    if (script.gridLocation.x == activeLocation.x + 1 && script.gridLocation.y == activeLocation.y)
                        script.StateCull(state[prev]);
                if (pd == projectDirection.up)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y == activeLocation.y + 1)
                        script.StateCull(state[prev]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        contact = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        contact = false;
    }

    //changes the appearence of the sprite based on state
    private void changeColour()
    {
        Color colour;
        if (state[i] == "visible")
        {
            colour = Color.yellow;
        }
        else if (state[i] == "infrared")
        {
            colour = Color.red;
        }
        else
        {
            colour = Color.magenta;
        }

        spriterenderer.color = colour;
    }









    // So dirty
    void SendNothing()
    {
        foreach (WallState script in scripts)
        {
            if (bonusType == 0)
            {
                if (script.gridLocation == activeLocation)
                {
                    script.StateCull(state[i]);
                    script.StateUpdate("");
                }
            }
            else if (bonusType == 1)
            {
                if (fd == focusDirection.down)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y <= activeLocation.y)
                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
                if (fd == focusDirection.left)
                    if (script.gridLocation.x <= activeLocation.x && script.gridLocation.y == activeLocation.y)
                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
                if (fd == focusDirection.right)
                    if (script.gridLocation.x >= activeLocation.x && script.gridLocation.y == activeLocation.y)

                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
                if (fd == focusDirection.up)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y >= activeLocation.y)
                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
            }
            else if (bonusType == 2)
            {
                if (script.gridLocation.x >= activeLocation.x - 1 && script.gridLocation.x <= activeLocation.x + 1
                        && script.gridLocation.y >= activeLocation.y - 1 && script.gridLocation.y <= activeLocation.y + 1)
                {
                    script.StateCull(state[i]);
                    script.StateUpdate("");
                }
            }
            else if (bonusType == 3)
            {

                if (pd == projectDirection.down)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y == activeLocation.y - 1)
                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
                if (pd == projectDirection.left)
                    if (script.gridLocation.x == activeLocation.x - 1 && script.gridLocation.y == activeLocation.y)
                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
                if (pd == projectDirection.right)
                    if (script.gridLocation.x == activeLocation.x + 1 && script.gridLocation.y == activeLocation.y)

                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
                if (pd == projectDirection.up)
                    if (script.gridLocation.x == activeLocation.x && script.gridLocation.y == activeLocation.y + 1)
                    {
                        script.StateCull(state[i]);
                        script.StateUpdate("");
                    }
            }
        }
    }
}
