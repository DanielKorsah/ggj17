using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDetailsDisplay : MonoBehaviour
{
    [SerializeField]
    private Text Number;
    [SerializeField]
    private Text Name;

    private LevelDetailsSO Details;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (Text t in GetComponentsInChildren<Text>())
        //{
        //    if (t.name == "Name")
        //    {
        //        Name = t;
        //    }
        //    else
        //    {
        //        Number = t;
        //    }
        //}

        //if (Name == null || Number == null)
        //{
        //    Debug.LogError($"Missing Text component. Name: {(Name == null ? "missing" : "found")}. Number: {(Number == null ? "missing" : "found")}.");
        //}
    }

    public void SetDetails(LevelDetailsSO deets)
    {
        Details = deets;
        Name.text = Details.LevelName;
        Number.text = Details.LevelNumber;
    }
}
