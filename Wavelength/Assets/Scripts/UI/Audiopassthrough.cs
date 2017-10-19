using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiopassthrough : MonoBehaviour
{
    GameObject manager;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("AudioManager");
    }
}
