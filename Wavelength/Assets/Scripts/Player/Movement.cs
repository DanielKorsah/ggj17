using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{

    //vertical and horizontal acceleration numbers
    private float horizontal = 0;
    private float vertical = 0;
    //speed of character
    private float speed = 5.0f;
    //time to get to full speed in seconds
    private float acceleration = 0.2f;
    //transform vector
    Vector3 trans;

    private Image irImage;
    private Image vImage;
    private Image uvImage;
    private Wave waveSpawner;

    private List<string> affectedBy = new List<string>();

    private List<Component> tranmitters = new List<Component>();
    float minDist = 999999999.0f;

    private float maxMag;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Transmitter"))
        {
            tranmitters.Add(t.GetComponent<Transform>());
        }

        irImage = GameObject.Find("IR light").GetComponent<Image>();
        uvImage = GameObject.Find("UV light").GetComponent<Image>();
        vImage = GameObject.Find("V light").GetComponent<Image>();
        waveSpawner = GameObject.FindGameObjectWithTag("WaveSpawn").GetComponent<Wave>();
    }

    // Update is called once per frame
    void Update()
    {
        //  int frq;


        // adjust waveform
        //     frq = closest().GetComponent<Transmitter>().i;
        //  GameObject.FindGameObjectWithTag("WaveSpawn").GetComponent<Wave>().frequency = 1.0f + frq * 1.25f;
        /*

                switch (frq)
                {
                    case 0:
                        // IR
                        break;
                    case 1:
                        // V
                        break;
                    default:
                        //UV
                        break;
                }

        */

        maxMag = speed * Time.deltaTime;
        trans = new Vector3(0.0f, 0.0f, 0.0f);
        //move up
        if (Input.GetKey("w"))
        {
            if (Vertical < 0)
            {
                Vertical = 0;
            }
            Vertical += Time.deltaTime / acceleration;
            trans += new Vector3(0.0f, speed * Vertical, 0.0f) * Time.deltaTime;
        }
        //move down
        if (Input.GetKey("s"))
        {
            if (Vertical > 0)
            {
                Vertical = 0;
            }
            Vertical -= Time.deltaTime / acceleration;
            trans += new Vector3(0.0f, speed * Vertical, 0.0f) * Time.deltaTime;
        }
        //move left
        if (Input.GetKey("a"))
        {
            if (Horizontal > 0)
            {
                Horizontal = 0;
            }
            Horizontal -= Time.deltaTime / acceleration;
            trans += new Vector3(speed * horizontal, 0.0f, 0.0f) * Time.deltaTime;
        }
        //move right
        if (Input.GetKey("d"))
        {
            if (Horizontal < 0)
            {
                Horizontal = 0;
            }
            Horizontal += Time.deltaTime / acceleration;
            trans += new Vector3(speed * horizontal, 0.0f, 0.0f) * Time.deltaTime;
        }

        //normalise?
        if (trans.sqrMagnitude > maxMag * maxMag)
        {
            trans = trans.normalized * maxMag;
        }

        transform.position += trans;
    }

    // On entering next grid send waveform seed
    private void OnTriggerStay2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.tag.Equals("Grid"))
        {
            int frqc;
            frqc = collision.gameObject.GetComponent<Sectors>().getFrqCodes();
            switch (frqc)
            {
                case 0:
                    irImage.enabled = false;
                    vImage.enabled = false;
                    uvImage.enabled = false;
                    break;
                case 1:
                    irImage.enabled = true;
                    vImage.enabled = false;
                    uvImage.enabled = false;
                    break;
                case 2:
                    irImage.enabled = false;
                    vImage.enabled = true;
                    uvImage.enabled = false;
                    break;
                case 3:
                    irImage.enabled = false;
                    vImage.enabled = false;
                    uvImage.enabled = true;
                    break;
                case 4:
                    irImage.enabled = true;
                    vImage.enabled = true;
                    uvImage.enabled = false;
                    break;
                case 5:
                    irImage.enabled = true;
                    vImage.enabled = false;
                    uvImage.enabled = true;
                    break;
                case 6:
                    irImage.enabled = false;
                    vImage.enabled = true;
                    uvImage.enabled = true;
                    break;
                case 7:
                    irImage.enabled = true;
                    vImage.enabled = true;
                    uvImage.enabled = true;
                    break;
            }

            GameObject.FindGameObjectWithTag("WaveSpawn").GetComponent<Wave>().frqc = frqc;
        }*/
    }

    public void AddAffectors(List<string> newAffectors)
    {
        foreach (string wave in newAffectors)
        {
            affectedBy.Add(wave);
        }
        ChangeUILights();
    }

    public void RemoveAffectors(List<string> oldAffectors)
    {
        foreach (string wave in oldAffectors)
        {
            affectedBy.Remove(wave);
        }
        ChangeUILights();
    }

    // Change which lights are lit up on UI
    public void ChangeUILights()
    {
        if (affectedBy.Contains("IR"))
        {
            irImage.enabled = true;
        }
        else
        {
            irImage.enabled = false;
        }

        if (affectedBy.Contains("V"))
        {
            vImage.enabled = true;
        }
        else
        {
            vImage.enabled = false;
        }

        if (affectedBy.Contains("UV"))
        {
            uvImage.enabled = true;
        }
        else
        {
            uvImage.enabled = false;
        }
        TalkToWave();
    }

    private void TalkToWave()
    {
        if (affectedBy.Contains("IR") && affectedBy.Contains("V") && affectedBy.Contains("UV"))
        {
            waveSpawner.frqc = 7;
        }
        else if (affectedBy.Contains("V") && affectedBy.Contains("UV"))
        {
            waveSpawner.frqc = 6;
        }
        else if (affectedBy.Contains("IR") && affectedBy.Contains("UV"))
        {
            waveSpawner.frqc = 5;
        }
        else if (affectedBy.Contains("IR") && affectedBy.Contains("V"))
        {
            waveSpawner.frqc = 4;
        }
        else if (affectedBy.Contains("UV"))
        {
            waveSpawner.frqc = 3;
        }
        else if (affectedBy.Contains("V"))
        {
            waveSpawner.frqc = 2;
        }
        else if (affectedBy.Contains("IR"))
        {
            waveSpawner.frqc = 1;
        }
        else
        {
            waveSpawner.frqc = 0;
        }
    }


    private float Horizontal
    {
        get
        {
            return horizontal;
        }
        set
        {
            horizontal = Mathf.Clamp(value, -1, 1);
        }
    }
    private float Vertical
    {
        get
        {
            return vertical;
        }
        set
        {
            vertical = Mathf.Clamp(value, -1, 1);
        }
    }

    private GameObject Closest()
    {
        GameObject closestTrans = tranmitters[0].gameObject;
        minDist = 9999999.0f;
        foreach (Component t in tranmitters)
            if (Vector3.Distance(gameObject.transform.position, t.gameObject.transform.position) < minDist)
            {
                minDist = Vector3.Distance(gameObject.transform.position, t.gameObject.transform.position);
                closestTrans = t.gameObject;
            }
        return closestTrans;
    }
}
