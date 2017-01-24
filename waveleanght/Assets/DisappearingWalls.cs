using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWalls : MonoBehaviour
{

    [SerializeField]
    private string wallType;

    private int x;
    private int y;

    SpriteRenderer sprite;
    BoxCollider2D hitBox;
    
    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        hitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //method to show or hide this wall. Input false for hide. Input true for show.
    public void ShowWall(bool show)
    {
        sprite.enabled = show;
        hitBox.enabled = show;
    }

    //for setting the x and y of this wall
    public void SetCoords(int x, int y)
    {
        x = this.x;
        y = this.y;
    }

    //to read the wall type of this wall
    public string WallType
    {
        get
        {
            return wallType;
        }
    }
}
