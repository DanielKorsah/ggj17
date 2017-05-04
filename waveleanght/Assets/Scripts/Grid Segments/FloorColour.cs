using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColour : MonoBehaviour
{

    SpriteRenderer sprite;

    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeFloorColour(List<string> affectedBy)
    {
        //set sprite renderer to true or false based on if anything affects the tile
        if (affectedBy.Count == 0)
        {
            sprite.enabled = false;
        }
        else
        {
            sprite.enabled = true;

            //make the sprite renderer coloured based on all the types of light added together and dived by their count
            if (affectedBy.Contains("IR") && affectedBy.Contains("V") && affectedBy.Contains("UV"))
            {
                sprite.color = (Color.red + Color.yellow + Color.magenta) / 3;
            }
            else if (affectedBy.Contains("IR") && affectedBy.Contains("V"))
            {
                sprite.color = (Color.red + Color.yellow) / 2;
            }
            else if (affectedBy.Contains("IR") && affectedBy.Contains("UV"))
            {
                sprite.color = (Color.red + Color.magenta) / 2;
            }
            else if (affectedBy.Contains("V") && affectedBy.Contains("UV"))
            {
                sprite.color = (Color.yellow + Color.magenta) / 2;
            }
            else if (affectedBy.Contains("UV"))
            {
                sprite.color = Color.magenta;
            }
            else if (affectedBy.Contains("V"))
            {
                sprite.color = Color.yellow;
            }
            else if (affectedBy.Contains("IR"))
            {
                sprite.color = Color.red;
            }

            sprite.color  = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        }
    }
}
