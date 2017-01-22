using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour {


    [SerializeField]
    List<Sprite> sprites = new List<Sprite>();

    [SerializeField]
    List<GameObject> places;

    public float time;
    public int secsUnits = 0;
    public int secsTens = 0;
    public int minsUnits = 0;
    public int minsTens = 0;

    Image secU;
    Image secT;
    Image minU;
    Image minT;

    GameObject portal;
    bool stopTrigger;

    [SerializeField]
    List<int> digits = new List<int>();


    // Use this for initialization
    void Start () {
        digits.Add(secsUnits);
        digits.Add(secsTens);
        digits.Add(minsUnits);
        digits.Add(minsTens);

        time = Time.time;

        secU = places[0].GetComponent<Image>();
        secT = places[1].GetComponent<Image>();
        minU = places[2].GetComponent<Image>();
        minT = places[3].GetComponent<Image>();

        portal = GameObject.Find("EndPortal");
        stopTrigger = portal.GetComponent<EndPortal>().Contact;



    }
	
	// Update is called once per frame
	void Update () {
        addSec();
        //Debug.Log(minsTens + minsUnits + ":" + secsTens + secsUnits);
        secU.sprite = sprites[secsUnits];
        secT.sprite = sprites[secsTens];
        minU.sprite = sprites[minsUnits];
        minT.sprite = sprites[minsTens];

        if(stopTrigger == true)
        {
            //***********************************************
            //INSERT GLENNSCRIPT SAVE SYSTEM HERE
            //***********************************************
            return;
        }
    }

    void addSec()
    {
        //if time is a whole number
        if (Time.time - time >= 1)
        {
            secsUnits++;
            
            if(secsUnits == 10)
            {
                secsTens++;
                //Debug.Log("secsTens: " + secsTens);
                secsUnits = 0;
                if(secsTens == 6)
                {
                    addMin();
                    secsUnits = 0;
                    secsTens = 0;
                }
            }
            time = Time.time;
        }
    }

    void addMin()
    {
        minsUnits++;
        //Debug.Log("minsUnits: " + minsUnits);
        if (minsUnits == 10)
        {
            minsTens++;
            //Debug.Log("minsTens: " + minsTens);
            minsUnits = 0;
            if(minsTens >= 6)
            {
                for (int i = 0; i < digits.Count; i++)
                {
                    secsUnits = 0;
                    secsTens = 0;
                    minsUnits = 0;
                    minsTens = 0;
                }
            }
        }
    }
}
