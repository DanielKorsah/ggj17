using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{


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
    void Start()
    {
        digits.Add(secsUnits);
        digits.Add(secsTens);
        digits.Add(minsUnits);
        digits.Add(minsTens);

        time = Time.time;

        secU = places[0].GetComponent<Image>();
        secT = places[1].GetComponent<Image>();
        minU = places[2].GetComponent<Image>();
        minT = places[3].GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        addSec();
        //Debug.Log(minsTens + minsUnits + ":" + secsTens + secsUnits);
        secU.sprite = sprites[secsUnits];
        secT.sprite = sprites[secsTens];
        minU.sprite = sprites[minsUnits];
        minT.sprite = sprites[minsTens];
    }

    void addSec()
    {
        //if time is a whole number
        if (Time.time - time >= 1.0f)
        {
            secsUnits++;

            if (secsUnits == 10)
            {
                secsTens++;
                //Debug.Log("secsTens: " + secsTens);
                secsUnits = 0;
                if (secsTens == 6)
                {
                    addMin();
                    secsTens = 0;
                }
            }
            time += 1.0f;
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
            if (minsTens >= 6)
            {
                minsTens = 0;
            }
        }
    }
}
