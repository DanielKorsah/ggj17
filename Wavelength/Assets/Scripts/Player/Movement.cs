using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    //speed of character
    private float speed = 5.0f;
    //Player physics component
    private Rigidbody2D rigidbody;

    private Image irImage;
    private Image vImage;
    private Image uvImage;
    private Wave waveSpawner;

    private List<string> affectedBy = new List<string>();

    private List<Component> tranmitters = new List<Component>();
    float minDist = 999999999.0f;
    

    // Use this for initialization
    void Start()
    {
        //Setting up movement stuff
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

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
        // Moves the character
        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rigidbody.velocity /= (rigidbody.velocity.magnitude == 0.0f) ? 1.0f : rigidbody.velocity.magnitude;
        rigidbody.velocity *= speed;
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

    // Returns the index for the waveform
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
