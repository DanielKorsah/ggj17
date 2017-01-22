using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sectors : MonoBehaviour {


    [SerializeField]
    Vector2 v = new Vector2();
    [SerializeField]
    Sprite Square;
    [SerializeField]
    Sprite EmmasSquare;

    SpriteRenderer sr;

    public List<string> freqs = new List<string>();

    private WallState wall;

    // Use this for initialization
    void Start ()
    {
		foreach (GameObject w in GameObject.FindGameObjectsWithTag("Wall"))
        {
            if (w.GetComponent<WallState>().gridLocation == v)
            {
                wall = w.GetComponent<WallState>();
            }
        }
        sr = gameObject.GetComponent<SpriteRenderer>();
  //      EmmasSquare = Resources.Load("Assets/Sprites and textures/EMMA'S SQUARE!!!!", typeof(Sprite)) as Sprite;
  //      EmmasSquare = Resources.Load("Assets/Sprites and textures/white square", typeof(Sprite)) as Sprite;
    }
	
	// Update is called once per frame
	void Update ()
    {
        freqs = wall.freqs;
        if (Random.Range(1f, 100f) > 95f)
            SelectTile();
	}

    public void SelectTile()
    {
        int tileSelector = getFrqCodes();

        switch (tileSelector)
        {
            case 0:
                sr.sprite = EmmasSquare;
                sr.color = new Color(1f, 1f, 1f, 1f);
                break;
            case 1:
                sr.sprite = Square;
                sr.color = new Color(1f, 0f, 0f, 0.3f);
                break;
            case 2:
                sr.sprite = Square;
                sr.color = new Color(1f, 1f, 0f, 0.3f);
                break;
            case 3:
                sr.sprite = Square;
                sr.color = new Color(1f, 0f, 1f, 0.3f);
                break;
            case 4:
                sr.sprite = Square;
                sr.color = new Color(1f, 0.7f, 0f, 0.55f);
                break;
            case 5:
                sr.sprite = Square;
                sr.color = new Color(1f, 0f, 0.55f, 0.55f);
                break;
            case 6:
                sr.sprite = Square;
                sr.color = new Color(1f, 0.6f, 0.55f, 0.5f);
                break;
            case 7:
                sr.sprite = Square;
                sr.color = new Color(1f, 1f, 1f, 0.4f);
                break;
            default:
                sr.sprite = EmmasSquare;
                sr.color = new Color(1f, 1f, 1f, 1f);
                break;
        }
    }



    // 1 - IR, 2 - V, 3 - UV, 4 - IR+V, 5 - IR+UV, 6 - V+UV, 7 - all 3
    public int getFrqCodes()
    {
        int code = 0;
        foreach (string f in freqs)
        {
            if (f.Equals("infrared"))
            {
                switch (code)
                {
                    case 0:
                        code = 1;
                        break;
                    case 1:
                        break;
                    case 2:
                        code = 4;
                        break;
                    case 3:
                        code = 5;
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        code = 7;
                        break;
                }
            }
            if (f.Equals("visible"))
            {
                switch (code)
                {
                    case 0:
                        code = 2;
                        break;
                    case 1:
                        code = 4;
                        break;
                    case 2:
                        break;
                    case 3:
                        code = 6;
                        break;
                    case 4:
                        break;
                    case 5:
                        code = 7;
                        break;
                }
            }
            if (f.Equals("ultraviolet"))
            {
                switch (code)
                {
                    case 0:
                        code = 3;
                        break;
                    case 1:
                        code = 5;
                        break;
                    case 2:
                        code = 6;
                        break;
                    case 3:
                        break;
                    case 4:
                        code = 7;
                        break;
                }
            }

        }
        return code;
    }

    



}
